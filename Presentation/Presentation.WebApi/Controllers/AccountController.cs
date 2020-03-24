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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Presentation.WebApi.FilterAttributes;
using Serilog;

namespace Presentation.WebApi.Controllers {
    [Route("api/[controller]")]
    public class AccountController: BaseController {
        #region ctor
        private readonly IAccountService _accountService;
        private readonly IAccountProfileService _accountProfileService;
        private readonly RandomMaker _randomMaker;
        private readonly Cryptograph _cryptograph;
        private readonly ContentBodyMaker _contentBodyMaker;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        //private readonly SignInManager<Account> _signInManager;

        public AccountController(
            IAccountService accountService,
            IAccountProfileService accountProfileService,
            RandomMaker randomMaker,
            Cryptograph cryptograph,
            ContentBodyMaker contentBodyMaker,
            IEmailService emailService,
            ISMSService smsService
            //,SignInManager<Account> signInManager
            ) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
            _contentBodyMaker = contentBodyMaker;
            _emailService = emailService;
            _smsService = smsService;
            //_signInManager = signInManager;
        }
        #endregion

        #region private
        private Task<AuthenticateResult> AuthenticationManager {
            get {
                return HttpContext.AuthenticateAsync();
            }
        }
        #endregion

        [HttpPost, AllowAnonymous, Route("signup")]
        public async Task<IActionResult> SignupAsync([FromBody]SignupBindingModel collection) {
            // Captcha

            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(message: "Please define your Email or CellPhone number.");
            }
            collection.Username = collection.Username.Trim();

            try {
                if(collection.Username.IsPhoneNumber()) {
                    collection.Phone = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Phone == collection.Phone).ConfigureAwait(true) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: "This Phone number is already registered. If you forgot your Password try to use Forgat Password feature.");
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    collection.Email = collection.Username;
                    if(await _accountProfileService.FirstAsync(f => f.Email == collection.Email).ConfigureAwait(true) != null) {
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

        #region external authentication

        [HttpGet, AllowAnonymous, Route("externalsignin")]
        public IActionResult ExternalSigninAsync([FromQuery]string provider = "Google") {
            return new ChallengeResult(provider);
        }

        [HttpGet, AllowAnonymous, Route("externalsigninfailure")]
        public async Task<IActionResult> ExternalSigninFailureAsync([FromQuery]string provider = "Google") {
            var msg = $"Failed to signin to '{provider}'";
            await Task.CompletedTask.ConfigureAwait(false);
            return InternalServerError(message: msg);
        }

        [HttpGet, AllowAnonymous, Route("test")]
        public async Task<IActionResult> Test() {
            var authResult = await HttpContext.AuthenticateAsync().ConfigureAwait(true);
            var authProperties = authResult.Properties.Items;

            int.Parse("a");

            return Ok();
        }

        [HttpGet, AllowAnonymous, Route("externalsignincallback")]
        public async Task<IActionResult> ExternalSigninCallbackAsync(string returnUrl) {
            try {

                var authResult = await HttpContext.AuthenticateAsync().ConfigureAwait(true);
                var authProperties = authResult.Properties.Items;

                var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme).ConfigureAwait(true);
                var items = auth?.Properties?.Items;
                if(auth?.Principal == null || items == null || !items.ContainsKey("LoginProviderKey")) { }

                //var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
                //if(loginInfo == null) {
                //    return RedirectToAction("signin");
                //}

                //var userRegister = ServiceHelper.Instance.LoginByExternalProvider("google", loginInfo.Email);

                //try {
                //    var webAddr = webServiceUrl + "LoginByExternalProvider";
                //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                //    httpWebRequest.ContentType = "application/json";
                //    httpWebRequest.Method = "POST";
                //    streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                //    var postData = "{\"request\":{\"Provider\":\"" + provider + "\",\"Email\":\"" + email + "\"}}";
                //    streamWriter.Write(postData);
                //    streamWriter.Flush();
                //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //    var streamReader = new StreamReader(httpResponse.GetResponseStream());
                //    var serviceResult = streamReader.ReadToEnd();
                //    JObject jsonResult = JObject.Parse(serviceResult);
                //    JObject value = (JObject)jsonResult["Result"];
                //    return value.ToObject<UserLogin>();
                //}
                //catch(Exception ex) {
                //    Logger.LogFile("RegisterByExternalProvider", $"Date:{DateTime.Now},Error:{ex.Message}");
                //    return null;
                //}

                //if(userRegister == null || string.IsNullOrWhiteSpace(userRegister.Token)) {
                //    return RedirectToAction("Error", "Home");
                //}
                //await SignInManager.SignInAsync(userRegister, isPersistent: false, rememberBrowser: false);
                //return RedirectToLocal(returnUrl);

                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpPost, AllowAnonymous, Route("externalsigninconfirmation")]
        public async Task<IActionResult> ExternalSigninConfirmationAsync() {
            try {


                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        #endregion
    }
}