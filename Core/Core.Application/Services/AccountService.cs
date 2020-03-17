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
        private readonly MsSqlDbContext _msSqlDbContext;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;

        public AccountService(
            MsSqlDbContext msSqlDbContext,
            RandomMaker randomMaker,
            Cryptograph cryptograph) {

            _msSqlDbContext = msSqlDbContext;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
        }
        #endregion

        public async Task<BaseViewModel> Signup(SignupBindingModel signupModel) {
            var response = new BaseViewModel();

            var username = _randomMaker.NewNumber();
            var duplicated = await FirstAsync(f => f.Username.Equals(username));
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = await FirstAsync(f => f.Username.Equals(username));
            }

            using(var dbcontext = await _msSqlDbContext.Database.BeginTransactionAsync()) {
                try {
                    var now = DateTime.UtcNow;

                    var account = await _msSqlDbContext.Accounts.AddAsync(new Account {
                        Password = _cryptograph.Encrypt(signupModel.Password),
                        ProviderId = AccountProvider.Clipboard,
                        Username = username,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });
                    await account.Context.SaveChangesAsync();

                    var token = _randomMaker.NewToken();
                    var accountDevice = await _msSqlDbContext.AccountDevices.AddAsync(new AccountDevice {
                        AccountId = account.Entity.Id,
                        DeviceId = signupModel.DeviceId,
                        DeviceName = signupModel.DeviceName,
                        IMEI = signupModel.IMEI,
                        OS = signupModel.OS,
                        Token = token,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });
                    await accountDevice.Context.SaveChangesAsync();

                    var accountProfile = await _msSqlDbContext.AccountProfiles.AddAsync(new AccountProfile {
                        AccountId = account.Entity.Id,
                        Email = signupModel.Email,
                        ConfirmedEmail = false,
                        Phone = signupModel.Phone,
                        ConfirmedPhone = false,
                        CreatedAt = now,
                        StatusId = Status.Active
                    });
                    await accountProfile.Context.SaveChangesAsync();

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
                accountProfile = await _msSqlDbContext.AccountProfiles.FirstOrDefaultAsync(f =>
                    f.Phone.Equals(signinModel.Username)
                    && f.StatusId.Equals(Status.Active));
            }
            else if(new EmailAddressAttribute().IsValid(signinModel.Username)) {
                accountProfile = await _msSqlDbContext.AccountProfiles.FirstOrDefaultAsync(f =>
                    f.Email.ToLower().Equals(signinModel.Username.ToLower())
                    && f.StatusId.Equals(Status.Active));
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
            var account = await FirstAsync(f => f.Id.Equals(accountProfile.AccountId) && f.StatusId.Equals(Status.Active));
            // todo: check the account

            using(var dbcontext = await _msSqlDbContext.Database.BeginTransactionAsync()) {
                try {
                    // check password
                    if(_cryptograph.IsEqual(signinModel.Password, account.Password)) {
                        var accountDevice = await _msSqlDbContext.AccountDevices.FirstOrDefaultAsync(f => f.AccountId.Equals(account.Id));
                        // todo: check the accountDevice

                        if(accountDevice.DeviceId.Equals(signinModel.DeviceId)) {
                            // set new token
                            accountDevice.Token = token;
                            var changedAccountDevice = _msSqlDbContext.AccountDevices.Update(accountDevice);
                            await changedAccountDevice.Context.SaveChangesAsync();
                        }
                        else {
                            // create new device for account
                            var newAccountDevice = await _msSqlDbContext.AccountDevices.AddAsync(new AccountDevice {
                                AccountId = account.Id,
                                DeviceId = signinModel.DeviceId,
                                DeviceName = signinModel.DeviceName,
                                IMEI = signinModel.IMEI,
                                OS = signinModel.OS,
                                Token = token,
                                CreatedAt = now,
                                StatusId = Status.Active
                            });
                            await newAccountDevice.Context.SaveChangesAsync();
                        }
                    }
                    else {
                        response.Message = "Wrong password.";
                        return response;
                    }


                    // clean forgot password requests
                    var accountProfiles = _msSqlDbContext.AccountProfiles.Where(w => w.AccountId.Equals(account.Id));
                    await accountProfiles.ForEachAsync(e => e.ForgotPasswordToken = null);
                    _msSqlDbContext.AccountProfiles.UpdateRange(accountProfiles);

                    // set last signed in at
                    account.LastSignedinAt = now;
                    var changedAccount = _msSqlDbContext.Accounts.Update(account);
                    await changedAccount.Context.SaveChangesAsync();
                }
                catch(Exception ex) {
                    var errmsg = "Something went wrong.";
                    Log.Error(ex, errmsg);
                    response.Message = errmsg;
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
