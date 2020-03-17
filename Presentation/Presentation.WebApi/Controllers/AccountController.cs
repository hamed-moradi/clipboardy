using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assets.Model;
using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Settings;
using Assets.Model.View;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.WebApi.FilterAttributes;
using Serilog;

namespace Presentation.WebApi.Controllers {
    public class AccountController: BaseController {
        #region ctor
        private readonly IAccountService _accountService;
        private readonly IAccountProfileService _accountProfileService;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;

        public AccountController(
            IAccountService accountService,
            IAccountProfileService accountProfileService,
            RandomMaker randomMaker,
            Cryptograph cryptograph) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
        }
        #endregion

        [HttpPost, Route("signup")]
        public async Task<IActionResult> Signup([FromBody]SignupBindingModel collection) {
            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username)) {
                return BadRequest(message: "Please define your Email or CellPhone number.");
            }
            collection.Username = collection.Username.Trim();

            try {
                if(collection.Username.IsPhoneNumber()) {
                    collection.Phone = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Phone.Equals(collection.Phone)) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: "This Phone number is already registered. If you forgot your Password try to use Forgat Password feature.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    collection.Email = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Email.Equals(collection.Email)) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: "This Email is already registered. If you forgot your Password try to use Forgat Password feature.");
                    }
                }
                else {
                    return BadRequest(message: "Please define a correct Email or CellPhone number.");
                }

                if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define a Password.");
                }

                if(!collection.Password.Equals(collection.ConfirmPassword)) {
                    return BadRequest(message: "Defined Password is not match with repeated one.");
                }

                if(string.IsNullOrEmpty(collection.DeviceId) || string.IsNullOrEmpty(collection.DeviceName)) {
                    // || string.IsNullOrEmpty(collection.IMEI) || string.IsNullOrEmpty(collection.OS)
                    return BadRequest(message: "You're trying to register in the wrong way! buddy.");
                }

                var result = await _accountService.Signup(collection);
                switch(result.Status) {
                    case HttpStatusCode.OK:
                        return Ok(data: result.Data);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(message: result.Message);
                    case HttpStatusCode.InternalServerError:
                    default:
                        return InternalServerError(message: result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("signin")]
        public async Task<IActionResult> Signin([FromBody]SigninBindingModel collection) {
            Log.Debug($"A User is trying to signing in with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username) || string.IsNullOrEmpty(collection.Password)) {
                return BadRequest(message: "Please define your Username and Password.");
            }

            if(string.IsNullOrEmpty(collection.DeviceId) || string.IsNullOrEmpty(collection.DeviceName)) {
                return BadRequest(message: "You're trying to signing in through the wrong way! buddy.");
            }

            try {
                var result = await _accountService.Signin(collection);

                switch(result.Status) {
                    case HttpStatusCode.OK:
                        return Ok(data: result.Data);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(message: result.Message);
                    case HttpStatusCode.InternalServerError:
                    default:
                        return InternalServerError(message: result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("signout")]
        public async Task<IActionResult> Signout() {
            try {
                return Ok();

            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("signingooglecallback")]
        public async Task<IActionResult> SigninGoogleCallback() {
            try {
                return Ok();

            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("changepassword"), ArgumentBinderFilter(ThrowException = true, FillValues = true)]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordBindingModel collection) {
            Log.Debug($"ChangePassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.NewPassword) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define Passwords.");
                }

                if(!collection.NewPassword.Equals(collection.ConfirmPassword)) {
                    return BadRequest(message: "Defined New Password is not match with Confirmed one.");
                }

                var account = await _accountService.FirstAsync(collection.AccountId.Value);
                // todo: check account
                if(_cryptograph.IsEqual(collection.Password, account.Password)) {
                    account.Password = _cryptograph.Encrypt(collection.NewPassword);
                    await _accountService.UpdateAsync(account, needToFetch: false);
                    return Ok(message: "Password is changed successfully.");
                }
                else {
                    return BadRequest(status: HttpStatusCode.Unauthorized, message: "Wrong Password.");
                }
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("changeforgotenpassword")]
        public async Task<IActionResult> ChangeForgotenPassword([FromBody]ChangeForgotenPasswordBindingModel collection) {
            try {
                Log.Debug($"ChangeForgotenPassword => {JsonConvert.SerializeObject(collection)}");

                if(string.IsNullOrEmpty(collection.Username)) {
                    return BadRequest(message: "Please define your Email or CellPhone number.");
                }
                collection.Username = collection.Username.Trim();

                if(string.IsNullOrEmpty(collection.NewPassword) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define Passwords.");
                }

                if(!collection.NewPassword.Equals(collection.ConfirmPassword)) {
                    return BadRequest(message: "Defined NewPassword is not match with Confirmed one.");
                }

                if(string.IsNullOrWhiteSpace(collection.Token)) {
                    return BadRequest(message: "You're trying to change your Password in the wrong way! buddy.");
                }

                AccountProfile accountProfile = null;
                if(collection.Username.IsPhoneNumber()) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Phone.Equals(collection.Username));
                    if(accountProfile == null) {
                        return BadRequest(message: "This Phone number is not found.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Email.Equals(collection.Username));
                    if(accountProfile == null) {
                        return BadRequest(message: "This Email number is not found.");
                    }
                }
                else {
                    return BadRequest(message: "Please define a correct Email or CellPhone number.");
                }

                if(string.IsNullOrWhiteSpace(accountProfile.ForgotPasswordToken)) {
                    return BadRequest(message: "You should request forgoten password first.");
                }

                var account = await _accountService.FirstAsync(accountProfile.AccountId.Value);
                // todo: check account
                if(accountProfile.ForgotPasswordToken.Equals(collection.Token)) {
                    account.Password = _cryptograph.Encrypt(collection.NewPassword);
                    await _accountService.UpdateAsync(account, needToFetch: false);
                    return Ok(message: "Password is changed successfully.");
                }
                else {
                    Log.Warning($"Account => {account}, AccountProfile => {accountProfile}, It tried to change its password with a wrong 'ForgotPasswordToken'");
                    return BadRequest(status: HttpStatusCode.BadRequest, message: "Your request for changing the password is liked an abnormal activity and it's logged.");
                }
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpGet, Route("changepasswordrequested")]
        public async Task<IActionResult> ChangePassword([FromQuery]string username, [FromQuery]string token) {
            try {
                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }

        [HttpPost, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordBindingModel collection) {
            Log.Debug($"ForgotPassword => {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username)) {
                return BadRequest(message: "Please define your Email or CellPhone number.");
            }
            collection.Username = collection.Username.Trim();

            try {
                var itsphone = true;
                AccountProfile accountProfile = null;
                if(collection.Username.IsPhoneNumber()) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Phone.Equals(collection.Username));
                    if(accountProfile == null) {
                        return BadRequest(message: "This Phone number is not found.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    itsphone = false;
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Email.Equals(collection.Username));
                    if(accountProfile == null) {
                        return BadRequest(message: "This Email number is not found.");
                    }
                }
                else {
                    return BadRequest(message: "Please define a correct Email or CellPhone number.");
                }

                var token = _randomMaker.NewToken();
                var changepassurl = $"clipboardy.com/api/account/changepasswordrequested?username={collection.Username}&token={token}";
                accountProfile.ForgotPasswordToken = token;
                await _accountProfileService.UpdateAsync(accountProfile, needToFetch: false);

                if(itsphone) {
                    // send {changepassurl} through sms
                }
                else {
                    // send {changepassurl} through email
                }
                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, GlobalVariables.SomethingWrong);
                return InternalServerError(GlobalVariables.SomethingWrong);
            }
        }
    }
}