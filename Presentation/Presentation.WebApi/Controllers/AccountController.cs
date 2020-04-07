using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Assets.Model.Binding;
using Assets.Model.Common;
using Assets.Model.View;
using Assets.Resource;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ContentBodyMaker _contentBodyMaker;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;

        public AccountController(
            IAccountService accountService,
            IAccountProfileService accountProfileService,
            RandomMaker randomMaker,
            Cryptograph cryptograph,
            ContentBodyMaker contentBodyMaker,
            IEmailService emailService,
            ISMSService smsService) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
            _contentBodyMaker = contentBodyMaker;
            _emailService = emailService;
            _smsService = smsService;
        }
        #endregion

        [HttpPost, AllowAnonymous, Route("signup")]
        public async Task<IActionResult> SignupAsync([FromBody]SignupBindingModel collection) {
            // Captcha

            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(message: _localizer[ResourceMessage.DefectiveEmailOrCellPhone]);
            }
            collection.Username = collection.Username.Trim();

            try {
                if(collection.Username.IsPhoneNumber()) {
                    collection.Phone = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Phone == collection.Phone).ConfigureAwait(true) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: _localizer[ResourceMessage.CellPhoneAlreadyExists]);
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    collection.Email = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Email == collection.Email).ConfigureAwait(true) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: _localizer[ResourceMessage.EmailAlreadyExists]);
                    }
                }
                else {
                    return BadRequest(message: _localizer[ResourceMessage.InvalidEmailOrCellPhone]);
                }

                if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define a Password.");
                }

                if(collection.Password != collection.ConfirmPassword) {
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
                Log.Error(ex, ex.Source);
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpPost, AllowAnonymous, Route("signin")]
        public async Task<IActionResult> SigninAsync([FromBody]SigninBindingModel collection) {
            Log.Debug($"A User is trying to signing in with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username) || string.IsNullOrEmpty(collection.Password)) {
                return BadRequest(message: "Please define your Username and Password.");
            }

            if(string.IsNullOrEmpty(collection.DeviceId) || string.IsNullOrEmpty(collection.DeviceName)) {
                return BadRequest(message: "You're trying to signing in through the wrong way! buddy.");
            }

            try {
                var result = await _accountService.Signin(collection).ConfigureAwait(true);

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
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpPost, Route("signout")]
        public async Task<IActionResult> SignoutAsync() {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return Ok();
        }

        [HttpPost, ArgumentBinder, Route("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordBindingModel collection) {
            Log.Debug($"ChangePassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection?.Password) || string.IsNullOrEmpty(collection.NewPassword) || string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define Passwords.");
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(message: "Defined New Password does not match with Confirmed one.");
                }

                var account = await _accountService.FirstAsync(collection.AccountHeader.Id).ConfigureAwait(true);
                // todo: check account
                if(_cryptograph.IsEqual(collection.Password, account.Password)) {
                    account.Password = _cryptograph.RNG(collection.NewPassword);
                    await _accountService.UpdateAsync(account, needToFetch: false).ConfigureAwait(false);
                    return Ok(message: "Password is changed successfully.");
                }
                else {
                    return BadRequest(status: HttpStatusCode.Unauthorized, message: "Wrong Password.");
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpPost, AllowAnonymous, Route("changeforgotenpassword")]
        public async Task<IActionResult> ChangeForgotenPasswordAsync([FromBody]ChangeForgotenPasswordBindingModel collection) {
            Log.Debug($"ChangeForgotenPassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection?.Username)) {
                    return BadRequest(message: "Please define your Email or CellPhone number.");
                }
                collection.Username = collection.Username.Trim();

                if(string.IsNullOrEmpty(collection.NewPassword) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: "Please define Passwords.");
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(message: "Defined NewPassword is not match with Confirmed one.");
                }

                if(string.IsNullOrWhiteSpace(collection.Token)) {
                    return BadRequest(message: "You're trying to change your Password in the wrong way! buddy.");
                }

                AccountProfile accountProfile = null;
                if(collection.Username.IsPhoneNumber()) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Phone == collection.Username).ConfigureAwait(true);
                    if(accountProfile == null) {
                        return BadRequest(message: "This Phone number is not found.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Email == collection.Username).ConfigureAwait(true);
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
                if(accountProfile.ForgotPasswordToken == collection.Token) {
                    accountProfile.ForgotPasswordToken = null;
                    await _accountProfileService.UpdateAsync(accountProfile).ConfigureAwait(false);

                    account.Password = _cryptograph.RNG(collection.NewPassword);
                    await _accountService.UpdateAsync(account, needToFetch: false).ConfigureAwait(false);

                    return Ok(message: "Password is changed successfully.");
                }
                else {
                    Log.Warning($"Account => {account}, AccountProfile => {accountProfile}, It tried to change its password with a wrong 'ForgotPasswordToken'");
                    return BadRequest(status: HttpStatusCode.BadRequest, message: "Your request for changing the password is liked an abnormal activity and it's logged.");
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpGet, AllowAnonymous, Route("changepasswordrequested")]
        public async Task<IActionResult> ChangePasswordRequestedAsync([FromQuery]string username, [FromQuery]string token) {
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token)) {
                return BadRequest(message: _localizer[ResourceMessage.DefectiveEntry]);
            }

            var result = new ChangeForgotenPasswordViewModel {
                Username = Encoding.UTF8.GetString(Convert.FromBase64String(username)),
                Token = token
            };
            await Task.CompletedTask.ConfigureAwait(true);

            return Ok(data: result);
        }

        [HttpPost, AllowAnonymous, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]ForgotPasswordBindingModel collection) {
            Log.Debug($"ForgotPassword => {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(message: "Please define your Email or CellPhone number.");
            }
            collection.Username = collection.Username.Trim();

            try {
                var itsphone = true;
                AccountProfile accountProfile = null;
                if(collection.Username.IsPhoneNumber()) {
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Phone == collection.Username).ConfigureAwait(true);
                    if(accountProfile == null) {
                        return BadRequest(message: "This Phone number is not found.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    itsphone = false;
                    accountProfile = await _accountProfileService.FirstAsync(f => f.Email == collection.Username).ConfigureAwait(true);
                    if(accountProfile == null) {
                        return BadRequest(message: "This Email number is not found.");
                    }
                }
                else {
                    return BadRequest(message: "Please define a correct Email or CellPhone number.");
                }

                var token = _randomMaker.NewToken();
                var username = Convert.ToBase64String(Encoding.UTF8.GetBytes(collection.Username));
                var changepassurl = $"clipboardy.com/api/account/changepasswordrequested?username={username}&token={token}";
                accountProfile.ForgotPasswordToken = token;
                await _accountProfileService.UpdateAsync(accountProfile, needToFetch: false).ConfigureAwait(false);

                if(itsphone) {
                    await _smsService.SendAsync(new SMSModel {
                        PhoneNo = accountProfile.Phone,
                        TextBody = _contentBodyMaker.CommonSMSBody(changepassurl)
                    }).ConfigureAwait(false);
                }
                else {
                    await _emailService.SendAsync(new EmailModel {
                        Name = accountProfile.Email,
                        Address = accountProfile.Email,
                        Subject = _localizer[""],
                        TextBody = _contentBodyMaker.CommonEmailBody(changepassurl)
                    }).ConfigureAwait(false);
                }
                return Ok(data: changepassurl);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }
    }
}