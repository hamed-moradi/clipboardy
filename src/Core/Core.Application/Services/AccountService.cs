using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Header;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Services {
  public class AccountService: IAccountService {
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

    public async Task<Account> AuthenticateAsync(string token) {
      Account account = null;
      await Task.Run(() => {
        account = _jwtHandler.Validate(token).ToAccount();
      });
      return account;
    }

    public async Task<IServiceResult> SignupAsync(SignupBindingModel signupModel) {
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
      //      Password = _cryptograph.RNG(signupModel.Password),
      //      ProviderId = AccountProvider.Clipboardy.ToInt(),
      //      Username = username,
      //      CreatedAt = now,
      //      StatusId = Status.Active.ToInt()
      //    };
      //    var accountId = await AddAsync(account);

      //    var accountDevice = new AccountDeviceAddSchema {
      //      AccountId = accountId,
      //      DeviceId = signupModel.DeviceId,
      //      DeviceName = signupModel.DeviceName,
      //      DeviceType = signupModel.DeviceType,
      //      CreatedAt = now,
      //      StatusId = Status.Active.ToInt()
      //    };
      //    var deviceId = await _accountDeviceService.AddAsync(accountDevice);

      //    var accountProfile = new AccountProfileAddSchema {
      //      AccountId = accountId,
      //      LinkedId = string.IsNullOrWhiteSpace(signupModel.Email) ? signupModel.Phone : signupModel.Email,
      //      TypeId = string.IsNullOrWhiteSpace(signupModel.Email) ? AccountProfileType.Phone.ToInt() : AccountProfileType.Email.ToInt(),
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
      //    return DataTransferer.InternalServerError(ex);
      //  }
      //}
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

      using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
        try {
          //var query = new AccountProfileGetFirstSchema {
          //  LinkedId = signinModel.Username
          //  //StatusId = Status.Active
          //};
          //if(signinModel.Username.IsPhoneNumber()) {
          //  query.TypeId = AccountProfileType.Phone.ToInt();
          //}
          //else if(new EmailAddressAttribute().IsValid(signinModel.Username)) {
          //  query.TypeId = AccountProfileType.Email.ToInt();
          //}
          //else {
          //  return DataTransferer.InvalidEmailOrCellPhone();
          //}

          //var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
          //if(accountProfile == null) {
          //  if(query.TypeId == AccountProfileType.Phone.ToInt())
          //    return DataTransferer.PhoneNotFound();
          //  else
          //    return DataTransferer.EmailNotFound();
          //}

          //var account = await FirstAsync(new AccountGetFirstSchema {
          //  Id = accountProfile.AccountId,
          //  StatusId = Status.Active.ToInt()
          //}).ConfigureAwait(true);

          //if(account == null)
          //  return DataTransferer.UserIsNotActive();

          //var deviceId = 0;
          //// check password
          //if(_cryptograph.IsEqual(signinModel.Password, account.Password)) {
          //  var accountDeviceQuery = new AccountDeviceGetFirstSchema {
          //    AccountId = account.Id,
          //    DeviceId = signinModel.DeviceId
          //  };
          //  var accountDevice = await _accountDeviceService.FirstAsync(accountDeviceQuery);
          //  // todo: check the accountDevice

          //  if(accountDevice != null) {
          //    deviceId = accountDevice.Id.Value;
          //    // set new token
          //    await _accountDeviceService.UpdateAsync(new AccountDeviceUpdateSchema {
          //      Id = accountDevice.Id.Value,
          //    });
          //  }
          //  else {
          //    // create new device for account
          //    deviceId = await _accountDeviceService.AddAsync(new AccountDeviceAddSchema {
          //      AccountId = account.Id,
          //      DeviceId = signinModel.DeviceId,
          //      DeviceName = signinModel.DeviceName,
          //      DeviceType = signinModel.DeviceType,
          //      CreatedAt = now,
          //      StatusId = Status.Active.ToInt()
          //    });
          //  }
          //}
          //else {
          //  return DataTransferer.WrongPassword();
          //}

          //// clean forgot password tokens
          //await _accountProfileService.CleanForgotPasswordTokensAsync(account.Id.Value);
          ////if(accountProfilesQuery.StatusCode != 200) {
          ////    Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
          ////    return DataTransferer.SomethingWentWrong();
          ////}

          //// set last signed in at
          //var changedAccount = UpdateAsync(new AccountUpdateSchema {
          //  LastSignedinAt = now
          //});

          //transaction.Complete();
          //var token = _jwtHandler.Bearer(new Account(account.Id.Value, deviceId, signinModel.Username, now).ToClaimsIdentity());
          //return DataTransferer.Ok(token);
          return DataTransferer.Ok();
        }
        catch(Exception ex) {
          Log.Error(ex, ex.Source);
          return DataTransferer.InternalServerError();
        }
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
