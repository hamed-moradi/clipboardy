using Assets.Model.Base;
using Assets.Model.Binding;
using Core.Domain.StoredProcedure.Result;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Domain.StoredProcedure.Schema;
using Serilog;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Transactions;
using Core.Application.Infrastructure;

namespace Core.Application.Services {
    public class AccountService: IAccountService {
        #region
        private readonly IStoredProcedureService _storedProcedure;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;
        private readonly IAccountProfileService _accountProfileService;
        private readonly IAccountDeviceService _accountDeviceService;

        public AccountService(
            IStoredProcedureService storedProcedure,
            RandomMaker randomMaker,
            Cryptograph cryptograph,
            IAccountProfileService accountProfileService,
            IAccountDeviceService accountDeviceService) {

            _storedProcedure = storedProcedure;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
            _accountProfileService = accountProfileService;
            _accountDeviceService = accountDeviceService;
        }
        #endregion

        public async Task<AccountAuthenticateResult> AuthenticateAsync(AccountAuthenticateSchema schema) {
            var result = await _storedProcedure.QueryFirstAsync<AccountAuthenticateSchema, AccountAuthenticateResult>(schema);
            return result;
        }

        public async Task<AccountResult> FirstAsync(AccountGetFirstSchema account) {
            var result = await _storedProcedure.QueryFirstAsync<AccountGetFirstSchema, AccountResult>(account);
            return result;
        }

        public async Task<int> AddAsync(AccountAddSchema account) {
            var result = await _storedProcedure.ExecuteScalarAsync<AccountAddSchema, int>(account);
            return result;
        }

        public async Task UpdateAsync(AccountUpdateSchema account) {
            await _storedProcedure.ExecuteAsync(account);
        }

        public async Task<IServiceResult> SignupAsync(SignupBindingModel signupModel) {
            var username = _randomMaker.NewNumber();
            var duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
            }

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try {
                    var now = DateTime.UtcNow;

                    var account = new AccountAddSchema {
                        Password = _cryptograph.RNG(signupModel.Password),
                        ProviderId = AccountProvider.Clipboard.ToInt(),
                        Username = username,
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    var accountId = await AddAsync(account);

                    var token = _randomMaker.NewToken();
                    var accountDevice = new AccountDeviceAddSchema {
                        AccountId = accountId,
                        DeviceId = signupModel.DeviceId,
                        DeviceName = signupModel.DeviceName,
                        DeviceType = signupModel.DeviceType,
                        Token = token,
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    await _accountDeviceService.AddAsync(accountDevice);

                    var accountProfile = new AccountProfileAddSchema {
                        AccountId = accountId,
                        LinkedId = string.IsNullOrWhiteSpace(signupModel.Email) ? signupModel.Phone : signupModel.Email,
                        TypeId = string.IsNullOrWhiteSpace(signupModel.Email) ? AccountProfileType.Phone.ToInt() : AccountProfileType.Email.ToInt(),
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    await _accountProfileService.AddAsync(accountProfile);

                    transaction.Complete();
                    return DataTransferer.Ok(token);
                }
                catch(Exception ex) {
                    var errmsg = "Something went wrong.";
                    Log.Error(ex, errmsg);
                    return DataTransferer.InternalServerError(ex);
                }
            }
        }

        public async Task<IServiceResult> ExternalSignupAsync(ExternalUserBindingModel externalUser) {
            var username = _randomMaker.NewNumber();
            var duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = await FirstAsync(new AccountGetFirstSchema { Username = username });
            }

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try {
                    var now = DateTime.UtcNow;

                    var account = new AccountAddSchema {
                        ProviderId = externalUser.ProviderId.ToInt(),
                        Username = username,
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    var accountId = await AddAsync(account);

                    var token = _randomMaker.NewToken();
                    var accountDevice = new AccountDeviceAddSchema {
                        AccountId = accountId,
                        DeviceId = externalUser.DeviceId,
                        DeviceName = externalUser.DeviceName,
                        DeviceType = externalUser.DeviceType,
                        Token = token,
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    await _accountDeviceService.AddAsync(accountDevice);

                    var accountProfile = new AccountProfileAddSchema {
                        AccountId = accountId,
                        TypeId = AccountProfileType.Email.ToInt(),
                        LinkedId = externalUser.Email,
                        CreatedAt = now,
                        StatusId = Status.Active.ToInt()
                    };
                    await _accountProfileService.AddAsync(accountProfile);

                    transaction.Complete();
                    return DataTransferer.Ok(token);
                }
                catch(Exception ex) {
                    var errmsg = "Something went wrong.";
                    Log.Error(ex, errmsg);
                    return null;
                }
            }
        }

        public async Task<IServiceResult> SigninAsync(SigninBindingModel signinModel) {
            var now = DateTime.UtcNow;
            var token = _randomMaker.NewToken();

            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try {
                    var query = new AccountProfileGetFirstSchema {
                        LinkedId = signinModel.Username
                        //StatusId = Status.Active
                    };
                    if(signinModel.Username.IsPhoneNumber()) {
                        query.TypeId = AccountProfileType.Phone.ToInt();
                    }
                    else if(new EmailAddressAttribute().IsValid(signinModel.Username)) {
                        query.TypeId = AccountProfileType.Email.ToInt();
                    }
                    else {
                        return DataTransferer.BadEmailOrCellphone();
                    }

                    var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                    if(accountProfile == null) {
                        if(query.TypeId == AccountProfileType.Phone.ToInt())
                            return DataTransferer.PhoneNotFound();
                        else
                            return DataTransferer.EmailNotFound();
                    }

                    var account = await FirstAsync(new AccountGetFirstSchema {
                        Id = accountProfile.AccountId,
                        StatusId = Status.Active.ToInt()
                    }).ConfigureAwait(true);

                    if(account == null)
                        return DataTransferer.UserIsNotActive();

                    // check password
                    if(_cryptograph.IsEqual(signinModel.Password, account.Password)) {
                        var accountDeviceQuery = new AccountDeviceGetFirstSchema {
                            AccountId = account.Id,
                            DeviceId = signinModel.DeviceId
                        };
                        var accountDevice = await _accountDeviceService.FirstAsync(accountDeviceQuery);
                        // todo: check the accountDevice

                        if(accountDevice != null) {
                            // set new token
                            await _accountDeviceService.UpdateAsync(new AccountDeviceUpdateSchema {
                                Id = accountDevice.Id.Value,
                                Token = token
                            });
                        }
                        else {
                            // create new device for account
                            await _accountDeviceService.AddAsync(new AccountDeviceAddSchema {
                                AccountId = account.Id,
                                DeviceId = signinModel.DeviceId,
                                DeviceName = signinModel.DeviceName,
                                DeviceType = signinModel.DeviceType,
                                Token = token,
                                CreatedAt = now,
                                StatusId = Status.Active.ToInt()
                            });
                        }
                    }
                    else {
                        return DataTransferer.WrongPassword();
                    }

                    // clean forgot password tokens
                    var accountProfilesQuery = new AccountProfileCleanTokensSchema {
                        AccountId = account.Id.Value
                    };
                    await _accountProfileService.CleanForgotPasswordTokensAsync(accountProfilesQuery);
                    if(accountProfilesQuery.StatusCode != 200) {
                        Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
                        return DataTransferer.SomethingWentWrong();
                    }

                    // set last signed in at
                    var changedAccount = UpdateAsync(new AccountUpdateSchema {
                        LastSignedinAt = now
                    });

                    transaction.Complete();
                    return DataTransferer.Ok(token);
                }
                catch(Exception ex) {
                    Log.Error(ex, ex.Source);
                    return DataTransferer.InternalServerError();
                }
            }
        }

        public async Task<IServiceResult> ExternalSigninAsync(ExternalUserBindingModel externalUser, AccountProfileResult accountProfile) {

            if(accountProfile == null && accountProfile.AccountId.HasValue) {
                return DataTransferer.DefectiveEntry();
            }

            if(accountProfile.StatusId != Status.Active) {
                return DataTransferer.UserIsNotActive();
            }

            var now = DateTime.UtcNow;
            var token = _randomMaker.NewToken();

            var accountQuery = new AccountGetFirstSchema {
                Id = accountProfile.AccountId,
                StatusId = Status.Active.ToInt()
            };
            var account = await FirstAsync(accountQuery);
            if(account == null)
                return DataTransferer.UserNotFound();

            // todo: check the account
            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) {
                try {
                    var accountDeviceQuery = new AccountDeviceGetFirstSchema {
                        AccountId = account.Id,
                        DeviceId = externalUser.DeviceId
                    };
                    var accountDevice = await _accountDeviceService.FirstAsync(accountDeviceQuery);
                    if(accountDevice == null)
                        return DataTransferer.DeviceIdNotFound();

                    if(accountDevice != null) {
                        // set a new token
                        await _accountDeviceService.UpdateAsync(new AccountDeviceUpdateSchema {
                            Id = accountDevice.Id.Value,
                            Token = token
                        });
                    }
                    else {
                        // create new device for account
                        await _accountDeviceService.AddAsync(new AccountDeviceAddSchema {
                            AccountId = account.Id,
                            DeviceId = externalUser.DeviceId,
                            DeviceName = externalUser.DeviceName,
                            DeviceType = externalUser.DeviceType,
                            Token = token,
                            CreatedAt = now,
                            StatusId = Status.Active.ToInt()
                        });
                    }

                    // clean forgot password tokens
                    //account.Id.Value, transaction
                    var accountProfileCleanTokens = new AccountProfileCleanTokensSchema {
                        AccountId = account.Id.Value
                    };
                    await _accountProfileService.CleanForgotPasswordTokensAsync(accountProfileCleanTokens);
                    if(accountProfileCleanTokens.StatusCode != 200) {
                        Log.Error($"Can't update 'ForgotPasswordTokens' to NULL for AccountId={account.Id}");
                        return DataTransferer.SomethingWentWrong();
                    }

                    // set last signed in at
                    var accountUpdate = new AccountUpdateSchema {
                        Id = account.Id.Value,
                        LastSignedinAt = now
                    };
                    await UpdateAsync(accountUpdate);
                    if(accountUpdate.StatusCode != 200)
                        Log.Error($"Can't update 'LastSignedinAt' after a successfully signing in for AccountId={account.Id}");

                    transaction.Complete();
                    return DataTransferer.Ok(token);
                }
                catch(Exception ex) {
                    Log.Error(ex, ex.Source);
                    return DataTransferer.InternalServerError(ex);
                }
            }
        }
    }
}
