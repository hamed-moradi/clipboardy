using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Header;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application._App;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Services {
  public class AccountService: BaseService<Account>, IAccountService {
    #region
    private readonly RandomMaker _randomMaker;
    private readonly Cryptograph _cryptograph;
    private readonly IAccountProfileService _accountProfileService;
    private readonly IAccountDeviceService _accountDeviceService;
    private readonly JwtHandler _jwtHandler;

    public AccountService(
        RandomMaker randomMaker,
        Cryptograph cryptograph,
        IAccountProfileService accountProfileService,
        IAccountDeviceService accountDeviceService,
        JwtHandler jwtHandler) {

      _randomMaker = randomMaker;
      _cryptograph = cryptograph;
      _accountProfileService = accountProfileService;
      _accountDeviceService = accountDeviceService;
      _jwtHandler = jwtHandler;
    }
    #endregion

    public AccountProfileTypes DetectAccountTypeByKey(string accountKey) {
      if(accountKey.IsPhoneNumber()) {
        return AccountProfileTypes.PHONE;
      }
      else {
        return AccountProfileTypes.EMAIL;
      }
    }

    public async Task<AccountHeaderModel> AuthenticateAsync(string token) {
      AccountHeaderModel account = null;
      await Task.Run(() => {
        account = _jwtHandler.Validate(token).ToAccount();
      });
      return account;
    }

    public async Task<IServiceResult> SignupAsync(SignupBindingModel signupModel) {
      var username = DateTime.UtcNow.Ticks.ToString();

      var duplicated = _accountProfileService.First(p => p.linked_key == signupModel.AccountKey);
      if(duplicated != null) {
        return BadRequest("Uesr already exists");
      }

      using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {
        var accountModel = new Account {
          password = _cryptograph.RNG(signupModel.Password),
          username = username,
          status = Status.ACTIVE.ToString()
        };
        var account = await AddAsync(accountModel);

        var accountDevice = new AccountDevice {
          account_id = account.id,
          device_key = signupModel.DeviceKey,
          device_name = signupModel.DeviceName,
          device_type = signupModel.DeviceType,
          status = Status.ACTIVE.ToString()
        };
        var device = await _accountDeviceService.AddAsync(accountDevice);

        var accountProfile = new AccountProfile {
          account_id = account.id,
          linked_key = signupModel.AccountKey,
          profile_type = signupModel.AccountType.ToString().ToLower(),
          status = Status.ACTIVE.ToString()
        };
        await _accountProfileService.AddAsync(accountProfile);

        await PostgresContext.SaveChangesAsync();
        transaction.Complete();

        var token = _jwtHandler.Bearer(new AccountHeaderModel(account.id, device.id, username, DateTime.UtcNow)
          .ToClaimsIdentity());
        return Ok(token);
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return InternalError(ex);
      }
    }

    public async Task<IServiceResult> ExternalSignupAsync(ExternalUserBindingModel externalUser) {
      var username = _randomMaker.NewNumber();
      return DataTransferer.Ok();
      //var duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
      //while(duplicated != null) {
      //  username = _randomMaker.NewNumber();
      //  duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
      //}

      //using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
      //  try {
      //    var now = DateTime.UtcNow;

      //    var account = new AccountAddSchema {
      //      ProviderId = externalUser.ProviderId.ToInt(),
      //      Username = username,
      //      CreatedAt = now,
      //      StatusId = Status.Active.ToInt()
      //    };
      //    var accountId = await AddAsync(account);

      //    var accountDevice = new AccountDeviceAddSchema {
      //      AccountId = accountId,
      //      DeviceId = externalUser.DeviceId,
      //      DeviceName = externalUser.DeviceName,
      //      DeviceType = externalUser.DeviceType,
      //      CreatedAt = now,
      //      StatusId = Status.Active.ToInt()
      //    };
      //    var deviceId = await _accountDeviceService.AddAsync(accountDevice);

      //    var accountProfile = new AccountProfileAddSchema {
      //      AccountId = accountId,
      //      TypeId = AccountProfileType.Email.ToInt(),
      //      LinkedId = externalUser.Email,
      //      CreatedAt = now,
      //      StatusId = Status.Active.ToInt()
      //    };
      //    await _accountProfileService.AddAsync(accountProfile);

      //    transaction.Complete();
      //    var token = _jwtHandler.Bearer(new Account(accountId, deviceId, username, now).ToClaimsIdentity());
      //    return DataTransferer.Ok(token);
      //  }
      //  catch(Exception ex) {
      //    var errmsg = "Something went wrong.";
      //    Log.Error(ex, errmsg);
      //    return null;
      //  }
      //}
    }

    public async Task<IServiceResult> SigninAsync(SigninBindingModel signinModel) {
      var now = DateTime.UtcNow;

      using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {
        var accountProfile = _accountProfileService.First(p => p.linked_key == signinModel.AccountKey);
        if(accountProfile == null) {
          return BadRequest("Uesr not found");
        }
        if(accountProfile.status != Status.ACTIVE.ToString()) {
          return BadRequest("Uesr profile is not active");
        }

        var account = First(p => p.id == accountProfile.account_id);
        if(account.status != Status.ACTIVE.ToString()) {
          return BadRequest("Uesr is not active");
        }

        if(!_cryptograph.IsEqual(signinModel.Password, account.password)) {
          return BadRequest("Wrong password");
        }

        var accountDevice = _accountDeviceService.First(
          p => p.account_id == account.id && p.device_key == signinModel.DeviceKey);

        if(accountDevice == null) {
            // create new device for account
            accountDevice = _accountDeviceService.Add(new AccountDevice {
            account_id = accountProfile.account_id,
            device_key = signinModel.DeviceKey,
            device_name = signinModel.DeviceName,
            device_type = signinModel.DeviceType,
            status = Status.ACTIVE.ToString()
          });
        }
        else {
          if(accountDevice.device_name != signinModel.DeviceName
            || accountDevice.device_type != signinModel.DeviceType) {
            accountDevice.device_name = signinModel.DeviceName;
            accountDevice.device_type = signinModel.DeviceType;
          }
        }

        // set last signed in at
        account.last_signedin_at = DateTime.UtcNow;

        await PostgresContext.SaveChangesAsync();
        transaction.Complete();

        var token = _jwtHandler.Bearer(new AccountHeaderModel(accountProfile.account_id, accountDevice.id, signinModel.AccountKey, now)
          .ToClaimsIdentity());
        return Ok(token);
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return InternalError(ex);
      }
    }

    //public async Task<IServiceResult> ExternalSigninAsync(ExternalUserBindingModel externalUser, AccountProfileResult accountProfile) {

    //  if(accountProfile == null && accountProfile.AccountId.HasValue) {
    //    return DataTransferer.DefectiveEntry();
    //  }

    //  if(accountProfile.StatusId != Status.Active) {
    //    return DataTransferer.UserIsNotActive();
    //  }

    //  var now = DateTime.UtcNow;

    //  var accountQuery = new AccountGetFirstSchema {
    //    Id = accountProfile.AccountId,
    //    StatusId = Status.Active.ToInt()
    //  };
    //  var account = await FirstAsync(accountQuery);
    //  if(account == null)
    //    return DataTransferer.UserNotFound();

    //  // todo: check the account
    //  using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
    //    try {
    //      var accountDeviceQuery = new AccountDeviceGetFirstSchema {
    //        AccountId = account.Id,
    //        DeviceId = externalUser.DeviceId
    //      };
    //      var accountDevice = await _accountDeviceService.FirstAsync(accountDeviceQuery);
    //      if(accountDevice == null)
    //        return DataTransferer.DeviceIdNotFound();

    //      int deviceId;
    //      if(accountDevice != null) {
    //        deviceId = accountDevice.Id.Value;
    //        // set a new token
    //        await _accountDeviceService.UpdateAsync(new AccountDeviceUpdateSchema {
    //          Id = accountDevice.Id.Value
    //        });
    //      }
    //      else {
    //        // create new device for account
    //        deviceId = await _accountDeviceService.AddAsync(new AccountDeviceAddSchema {
    //          AccountId = account.Id,
    //          DeviceId = externalUser.DeviceId,
    //          DeviceName = externalUser.DeviceName,
    //          DeviceType = externalUser.DeviceType,
    //          CreatedAt = now,
    //          StatusId = Status.Active.ToInt()
    //        });
    //      }

    //      // clean forgot password tokens
    //      //account.Id.Value, transaction
    //      await _accountProfileService.CleanForgotPasswordTokensAsync(account.Id.Value);
    //      //if(accountProfileCleanTokens.StatusCode != 200) {
    //      //    Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
    //      //    return DataTransferer.SomethingWentWrong();
    //      //}

    //      // set last signed in at
    //      var accountUpdate = new AccountUpdateSchema {
    //        Id = account.Id.Value,
    //        LastSignedinAt = now
    //      };
    //      await UpdateAsync(accountUpdate);
    //      if(accountUpdate.StatusCode != 200)
    //        Log.Error($"Can't update 'LastSignedinAt' after a successfully signing in for AccountId={account.Id}");

    //      transaction.Complete();
    //      var token = _jwtHandler.Bearer(new Account(account.Id.Value, deviceId, account.Username, now).ToClaimsIdentity());
    //      return DataTransferer.Ok(token);
    //    }
    //    catch(Exception ex) {
    //      Log.Error(ex, ex.Source);
    //      return DataTransferer.InternalServerError(ex);
    //    }
    //  }
    //}
  }
}
