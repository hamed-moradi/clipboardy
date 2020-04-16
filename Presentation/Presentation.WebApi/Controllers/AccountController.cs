using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Common;
using Assets.Model.View;
using Assets.Resource;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
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
            // todo: Captcha

            if(collection == null) {
                return BadRequest(message: _localizer[DataTransferer.DefectiveEntry().Message]);
            }
            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(message: _localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
            }
            collection.Username = collection.Username.Trim();

            try {
                if(collection.Username.IsPhoneNumber()) {
                    collection.Phone = collection.Username;
                    if(await _accountProfileService.FirstAsync(new AccountProfileGetFirstSchema { LinkedId = collection.Phone }).ConfigureAwait(true) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: _localizer[DataTransferer.CellPhoneAlreadyExists().Message]);
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    collection.Email = collection.Username;
                    if(await _accountProfileService.FirstAsync(new AccountProfileGetFirstSchema { LinkedId = collection.Email }).ConfigureAwait(true) != null) {
                        return Ok(status: HttpStatusCode.NonAuthoritativeInformation,
                            message: _localizer[DataTransferer.EmailAlreadyExists().Message]);
                    }
                }
                else {
                    return BadRequest(message: _localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
                }

                if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: _localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.Password != collection.ConfirmPassword) {
                    return BadRequest(message: _localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                if(string.IsNullOrEmpty(collection.DeviceId) || string.IsNullOrEmpty(collection.DeviceName)) {
                    return BadRequest(message: _localizer[DataTransferer.UnofficialRequest().Message]);
                }

                var result = await _accountService.SignupAsync(collection).ConfigureAwait(true);
                switch(result.Code) {
                    case 200:
                        return Ok(result.Data);
                    case 400:
                        return BadRequest(result.Message);
                    case 500:
                    default:
                        return InternalServerError(message: result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, AllowAnonymous, Route("signin")]
        public async Task<IActionResult> SigninAsync([FromBody]SigninBindingModel collection) {
            Log.Debug($"A User is trying to signing in with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username) || string.IsNullOrEmpty(collection?.Password)) {
                return BadRequest(message: _localizer[DataTransferer.DefectiveUsernameOrPassword().Message]);
            }

            if(string.IsNullOrEmpty(HttpDeviceHeader.DeviceId) ||
                string.IsNullOrEmpty(HttpDeviceHeader.DeviceName) ||
                string.IsNullOrEmpty(HttpDeviceHeader.DeviceType)) {

                return BadRequest(message: _localizer[DataTransferer.UnofficialRequest().Message]);
            }

            try {
                var result = await _accountService.SigninAsync(collection).ConfigureAwait(true);
                switch(result.Code) {
                    case 200:
                        return Ok(data: result.Data);
                    case 400:
                        return BadRequest(message: result.Message);
                    case 500:
                    default:
                        return InternalServerError(message: result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[DataTransferer.SomethingWentWrong().Message]);
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
                    return BadRequest(message: _localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(message: _localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                var account = await _accountService.FirstAsync(new AccountGetFirstSchema {
                    Id = HttpAccountHeader.Id
                }).ConfigureAwait(true);

                if(account == null) {
                    return BadRequest(_localizer[DataTransferer.UserNotFound().Message]);
                }

                if(_cryptograph.IsEqual(collection.Password, account.Password)) {
                    await _accountService.UpdateAsync(new AccountUpdateSchema {
                        Id = account.Id.Value,
                        Password = _cryptograph.RNG(collection.NewPassword)
                    }).ConfigureAwait(false);

                    return Ok(message: _localizer[DataTransferer.PasswordChanged().Message]);
                }
                else {
                    return BadRequest(status: HttpStatusCode.Unauthorized, message: _localizer[DataTransferer.WrongPassword().Message]);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, AllowAnonymous, Route("changeforgotenpassword")]
        public async Task<IActionResult> ChangeForgotenPasswordAsync([FromBody]ChangeForgotenPasswordBindingModel collection) {
            Log.Debug($"ChangeForgotenPassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection?.Username)) {
                    return BadRequest(message: _localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
                }
                collection.Username = collection.Username.Trim();

                if(string.IsNullOrEmpty(collection.NewPassword) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(message: _localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(message: _localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                if(string.IsNullOrWhiteSpace(collection.Token)) {
                    return BadRequest(message: _localizer[DataTransferer.UnofficialRequest().Message]);
                }

                var query = new AccountProfileGetFirstSchema { LinkedId = collection.Username };
                if(collection.Username.IsPhoneNumber()) {
                    query.TypeId = AccountProfileType.Phone.ToInt();
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    query.TypeId = AccountProfileType.Email.ToInt();
                }
                else {
                    return BadRequest(message: _localizer[DataTransferer.BadEmailOrCellphone().Message]);
                }

                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(message: _localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(message: _localizer[DataTransferer.EmailNotFound().Message]);
                }

                if(string.IsNullOrWhiteSpace(accountProfile.ForgotPasswordToken)) {
                    return BadRequest(message: _localizer[DataTransferer.ChangingPasswordWithoutToken().Message]);
                }

                var account = await _accountService.FirstAsync(new AccountGetFirstSchema {
                    Id = accountProfile.AccountId.Value
                }).ConfigureAwait(true);

                if(account == null) {
                    return BadRequest(_localizer[DataTransferer.UserNotFound().Message]);
                }

                if(accountProfile.ForgotPasswordToken == collection.Token) {
                    await _accountProfileService.UpdateAsync(new AccountProfileUpdateSchema {
                        Id = accountProfile.Id.Value,
                        ForgotPasswordToken = string.Empty
                    }).ConfigureAwait(false);

                    await _accountService.UpdateAsync(new AccountUpdateSchema {
                        Id = account.Id.Value,
                        Password = _cryptograph.RNG(collection.NewPassword)
                    }).ConfigureAwait(false);

                    return Ok(message: _localizer[DataTransferer.PasswordChanged().Message]);
                }
                else {
                    Log.Warning($"Account => {account}, AccountProfile => {accountProfile}, It tried to change its password with a wrong 'ForgotPasswordToken'");
                    return BadRequest(status: HttpStatusCode.BadRequest, message: _localizer[DataTransferer.ChangingPasswordWithWrongToken().Message]);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpGet, AllowAnonymous, Route("changepasswordrequested")]
        public async Task<IActionResult> ChangePasswordRequestedAsync([FromQuery]string username, [FromQuery]string token) {
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token)) {
                return BadRequest(message: _localizer[DataTransferer.DefectiveEntry().Message]);
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
                return BadRequest(message: _localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
            }
            collection.Username = collection.Username.Trim();

            try {
                var query = new AccountProfileGetFirstSchema { LinkedId = collection.Username };
                if(collection.Username.IsPhoneNumber()) {
                    query.TypeId = AccountProfileType.Phone.ToInt();
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    query.TypeId = AccountProfileType.Email.ToInt();
                }
                else {
                    return BadRequest(message: _localizer[DataTransferer.BadEmailOrCellphone().Message]);
                }

                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(message: _localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(message: _localizer[DataTransferer.EmailNotFound().Message]);
                }

                var token = _randomMaker.NewToken();
                var username = Convert.ToBase64String(Encoding.UTF8.GetBytes(collection.Username));
                var changepassurl = $"clipboardy.com/api/account/changepasswordrequested?username={username}&token={token}";
                await _accountProfileService.UpdateAsync(new AccountProfileUpdateSchema {
                    Id = accountProfile.Id.Value,
                    ForgotPasswordToken = string.Empty
                }).ConfigureAwait(false);

                if(query.TypeId == AccountProfileType.Phone.ToInt()) {
                    await _smsService.SendAsync(new SMSModel {
                        PhoneNo = accountProfile.LinkedId,
                        TextBody = _contentBodyMaker.CommonSMSBody(changepassurl)
                    }).ConfigureAwait(false);
                }
                else {
                    await _emailService.SendAsync(new EmailModel {
                        Name = accountProfile.LinkedId,
                        Address = accountProfile.LinkedId,
                        Subject = _localizer[""],
                        TextBody = _contentBodyMaker.CommonEmailBody(changepassurl)
                    }).ConfigureAwait(false);
                }
                return Ok(data: changepassurl);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }
    }
}