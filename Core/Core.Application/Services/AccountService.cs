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
            var duplicated = FirstAsync(f => f.Username.Equals(username));
            while(duplicated != null) {
                username = _randomMaker.NewNumber();
                duplicated = FirstAsync(f => f.Username.Equals(username));
            }

            using(var dbcontext = await _msSqlDbContext.Database.BeginTransactionAsync()) {
                try {

                    var account = await _msSqlDbContext.Accounts.AddAsync(new Account {
                        Password = _cryptograph.Encrypt(signupModel.Password),
                        ProviderId = AccountProvider.Clipboard,
                        Username = username
                    });

                    var accountDevice = await _msSqlDbContext.AccountDevices.AddAsync(new AccountDevice {
                        AccountId = account.Entity.Id,
                        DeviceId = signupModel.DeviceId,
                        DeviceName = signupModel.DeviceName,
                        IMEI = signupModel.IMEI,
                        OS = signupModel.OS
                    });

                    var accountProfile = await _msSqlDbContext.AccountProfiles.AddAsync(new AccountProfile {
                        AccountId = account.Entity.Id,
                        Email = signupModel.Email,
                        Phone = signupModel.Phone
                    });

                    await dbcontext.CommitAsync();
                    response.Status = HttpStatusCode.OK;
                    response.Data = username;
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
                accountProfile = await _msSqlDbContext.AccountProfiles.FirstOrDefaultAsync(f => f.Phone.Equals(signinModel.Username));
            }
            else {
                accountProfile = await _msSqlDbContext.AccountProfiles.FirstOrDefaultAsync(f => f.Email.ToLower().Equals(signinModel.Username.ToLower()));
            }

            if(accountProfile == null && accountProfile.AccountId.HasValue) {
                response.Message = "User not found";
                return response;
            }


            if(_cryptograph.IsEqual(signinModel.Password, accountProfile.Account.Password)) {
                response.Message = "Wrong password";
                return response;
            }

            response.Status = HttpStatusCode.OK;
            response.Data = accountProfile;
            return response;
        }
    }
}
