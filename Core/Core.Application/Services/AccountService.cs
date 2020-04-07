using Assets.Model;
using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Domain;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class AccountService: GenericService<Account>, IAccountService {
        #region
        private readonly MsSQLDbContext _msSQLDbContext;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;
        private readonly IAccountProfileService _accountProfileService;
        private readonly IAccountDeviceService _accountDeviceService;

        public AccountService(
            MsSQLDbContext msSQLDbContext,
            RandomMaker randomMaker,
            Cryptograph cryptograph,
            IAccountProfileService accountProfileService,
            IAccountDeviceService accountDeviceService) {

            _msSQLDbContext = msSQLDbContext;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
            _accountProfileService = accountProfileService;
            _accountDeviceService = accountDeviceService;
        }
        #endregion

        public async Task<BaseViewModel> Signup(SignupBindingModel signupModel) {
            var response = new BaseViewModel();

            var username = _randomMaker.NewNumber();
            var duplicated = await FirstAsync(f => f.Username == username);
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = await FirstAsync(f => f.Username == username);
            }

            using(var dbcontext = await _msSQLDbContext.Database.BeginTransactionAsync()) {
                try {
                    var now = DateTime.UtcNow;

                    var account = await AddAsync(new Account {
                        Password = _cryptograph.RNG(signupModel.Password),
                        ProviderId = AccountProvider.Clipboard,
                        Username = username,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    var token = _randomMaker.NewToken();
                    await _accountDeviceService.AddAsync(new AccountDevice {
                        AccountId = account.Id,
                        DeviceId = signupModel.DeviceId,
                        DeviceName = signupModel.DeviceName,
                        DeviceType = signupModel.DeviceType,
                        Token = token,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    await _accountProfileService.AddAsync(new AccountProfile {
                        AccountId = account.Id,
                        Email = signupModel.Email,
                        ConfirmedEmail = false,
                        Phone = signupModel.Phone,
                        ConfirmedPhone = false,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    await dbcontext.CommitAsync();
                    response.Status = HttpStatusCode.OK;
                    response.Data = token;
                }
                catch(Exception ex) {
                    var errmsg = "Something went wrong.";
                    Log.Error(ex, errmsg);
                    response.Message = errmsg;
                    response.Status = HttpStatusCode.InternalServerError;
                    await dbcontext.RollbackAsync();
                }
            }

            return response;
        }

        public async Task<BaseViewModel> ExternalSignup(ExternalUserBindingModel externalUser) {
            var response = new BaseViewModel();

            if(externalUser.ProviderId == AccountProvider.Clipboard) {
                response.Message = "Unknown provider";
                Log.Error(response.Message);
                return response;
            }

            var username = _randomMaker.NewNumber();
            var duplicated = await FirstAsync(f => f.Username == username);
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = await FirstAsync(f => f.Username == username);
            }

            using(var dbcontext = await _msSQLDbContext.Database.BeginTransactionAsync()) {
                try {
                    var now = DateTime.UtcNow;
                    
                    var account = await AddAsync(new Account {
                        ProviderId = externalUser.ProviderId,
                        Username = username,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    var token = _randomMaker.NewToken();
                    await _accountDeviceService.AddAsync(new AccountDevice {
                        AccountId = account.Id,
                        DeviceId = externalUser.DeviceId,
                        DeviceName = externalUser.DeviceName,
                        DeviceType = externalUser.DeviceType,
                        Token = token,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    await _accountProfileService.AddAsync(new AccountProfile {
                        AccountId = account.Id,
                        Email = externalUser.Email,
                        ConfirmedEmail = true,
                        ConfirmedPhone = false,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });

                    await dbcontext.CommitAsync();
                    response.Status = HttpStatusCode.OK;
                    response.Data = token;
                }
                catch(Exception ex) {
                    var errmsg = "Something went wrong.";
                    Log.Error(ex, errmsg);
                    response.Message = errmsg;
                    response.Status = HttpStatusCode.InternalServerError;
                    await dbcontext.RollbackAsync();
                }
            }

            return response;
        }

        public async Task<BaseViewModel> Signin(SigninBindingModel signinModel) {
            var response = new BaseViewModel();
            AccountProfile accountProfile = null;

            if(signinModel.Username.IsPhoneNumber()) {
                accountProfile = await _msSQLDbContext.AccountProfiles.FirstOrDefaultAsync(f =>
                    f.Phone == signinModel.Username
                    && f.StatusId == Status.Active);
            }
            else if(new EmailAddressAttribute().IsValid(signinModel.Username)) {
                accountProfile = await _msSQLDbContext.AccountProfiles.FirstOrDefaultAsync(f =>
                    f.Email.ToLower() == signinModel.Username.ToLower()
                    && f.StatusId == Status.Active);
            }
            else {
                response.Message = $"Username is not valid or its not in '{Status.Active}' state.";
                return response;
            }

            if(accountProfile == null && accountProfile.AccountId.HasValue) {
                response.Message = $"User not found or its not in '{Status.Active}' state.";
                return response;
            }

            var now = DateTime.UtcNow;
            var token = _randomMaker.NewToken();
            var account = await FirstAsync(f => f.Id == accountProfile.AccountId && f.StatusId == Status.Active);
            // todo: check the account

            using(var dbcontext = await _msSQLDbContext.Database.BeginTransactionAsync()) {
                try {
                    // check password
                    if(_cryptograph.IsEqual(signinModel.Password, account.Password)) {

                        var accountDevice = await _accountDeviceService.FirstAsync(f => f.AccountId == account.Id && f.DeviceId == signinModel.DeviceId);
                        // todo: check the accountDevice

                        if(accountDevice != null) {
                            // set new token
                            accountDevice.Token = token;
                            await _accountDeviceService.UpdateAsync(accountDevice);
                        }
                        else {
                            // create new device for account
                            await _accountDeviceService.AddAsync(new AccountDevice {
                                AccountId = account.Id,
                                DeviceId = signinModel.DeviceId,
                                DeviceName = signinModel.DeviceName,
                                DeviceType = signinModel.DeviceType,
                                Token = token,
                                CreatedAt = now,
                                StatusId = Status.Active
                            });
                        }
                    }
                    else {
                        response.Message = "Wrong password.";
                        return response;
                    }

                    // clean forgot password tokens
                    var accountProfilesUpdated = await _accountProfileService.CleanForgotPasswordTokensAsync(account.Id.Value);
                    if(!accountProfilesUpdated) {
                        Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
                        await dbcontext.RollbackAsync();
                    }

                    // set last signed in at
                    account.LastSignedinAt = now;
                    var changedAccount = _msSQLDbContext.Accounts.Update(account);
                    await changedAccount.Context.SaveChangesAsync();

                    await dbcontext.CommitAsync();
                }
                catch(Exception ex) {
                    Log.Error(ex, ex.Source);
                    response.Message = GlobalVariables.UnknownError;
                    response.Status = HttpStatusCode.InternalServerError;
                    await dbcontext.RollbackAsync();
                }
            }

            response.Status = HttpStatusCode.OK;
            response.Data = token;
            return response;
        }

        public async Task<BaseViewModel> ExternalSignin(ExternalUserBindingModel externalUser, AccountProfile accountProfile) {
            var response = new BaseViewModel();

            if(accountProfile == null && accountProfile.AccountId.HasValue) {
                response.Message = $"User not found.";
                return response;
            }

            if(accountProfile.StatusId != Status.Active) {
                response.Message = $"User is not in '{Status.Active}' state.";
                return response;
            }

            var now = DateTime.UtcNow;
            var token = _randomMaker.NewToken();
            var account = await FirstAsync(f => f.Id == accountProfile.AccountId && f.StatusId == Status.Active);
            // todo: check the account

            using(var dbcontext = await _msSQLDbContext.Database.BeginTransactionAsync()) {
                try {

                    var accountDevice = await _accountDeviceService.FirstAsync(f => f.AccountId == account.Id && f.DeviceId == externalUser.DeviceId);
                    // todo: check the accountDevice

                    if(accountDevice != null) {
                        // set a new token
                        accountDevice.Token = token;
                        await _accountDeviceService.UpdateAsync(accountDevice);
                    }
                    else {
                        // create new device for account
                        await _accountDeviceService.AddAsync(new AccountDevice {
                            AccountId = account.Id,
                            DeviceId = externalUser.DeviceId,
                            DeviceName = externalUser.DeviceName,
                            DeviceType = externalUser.DeviceType,
                            Token = token,
                            CreatedAt = now,
                            StatusId = Status.Active
                        });
                    }

                    // clean forgot password tokens
                    var accountProfilesUpdated = await _accountProfileService.CleanForgotPasswordTokensAsync(account.Id.Value);
                    if(!accountProfilesUpdated) {
                        Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
                        await dbcontext.RollbackAsync();
                    }

                    // set last signed in at
                    account.LastSignedinAt = now;
                    var changedAccount = _msSQLDbContext.Accounts.Update(account);
                    await changedAccount.Context.SaveChangesAsync();

                    await dbcontext.CommitAsync();
                }
                catch(Exception ex) {
                    Log.Error(ex, ex.Source);
                    response.Message = GlobalVariables.UnknownError;
                    response.Status = HttpStatusCode.InternalServerError;
                    await dbcontext.RollbackAsync();
                }
            }

            response.Status = HttpStatusCode.OK;
            response.Data = token;
            return response;
        }
    }
}
