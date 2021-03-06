﻿using System;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        private readonly IMemoryCache _memoryCache;

        public AccountController(
            IAccountService accountService,
            IAccountProfileService accountProfileService,
            RandomMaker randomMaker,
            Cryptograph cryptograph,
            IEmailService emailService,
            ISMSService smsService,
            IMemoryCache memoryCache) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
            _randomMaker = randomMaker;
            _cryptograph = cryptograph;
            _emailService = emailService;
            _smsService = smsService;
            _memoryCache = memoryCache;
        }
        #endregion

        [HttpPost, AllowAnonymous, Route("signup")]
        public async Task<IActionResult> SignupAsync([FromBody] SignupBindingModel collection) {
            // todo: Captcha

            if(collection == null) {
                return BadRequest(_localizer[DataTransferer.DefectiveEntry().Message]);
            }

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(_localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
            }

            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");
            collection.Username = collection.Username.Trim();

            try {
                if(collection.Username.IsPhoneNumber()) {
                    collection.Phone = collection.Username;
                    if(await _accountProfileService.FirstAsync(new AccountProfileGetFirstSchema { LinkedId = collection.Phone }).ConfigureAwait(true) != null) {
                        return BadRequest(_localizer[DataTransferer.CellPhoneAlreadyExists().Message]);
                    }
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    collection.Email = collection.Username;
                    if(await _accountProfileService.FirstAsync(new AccountProfileGetFirstSchema { LinkedId = collection.Email }).ConfigureAwait(true) != null) {
                        return BadRequest(_localizer[DataTransferer.EmailAlreadyExists().Message]);
                    }
                }
                else {
                    return BadRequest(_localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
                }

                if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(_localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.Password != collection.ConfirmPassword) {
                    return BadRequest(_localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                if(string.IsNullOrEmpty(collection.DeviceId) || string.IsNullOrEmpty(collection.DeviceName)) {
                    return BadRequest(_localizer[DataTransferer.UnofficialRequest().Message]);
                }

                var result = await _accountService.SignupAsync(collection).ConfigureAwait(true);
                switch(result.Code) {
                    case 200:
                        return Ok(result.Data);
                    case 400:
                        return BadRequest(result.Message);
                    case 500:
                    default:
                        return Problem(result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, AllowAnonymous, Route("signin")]
        public async Task<IActionResult> SigninAsync([FromBody] SigninBindingModel collection) {
            Log.Debug($"A User is trying to signing in with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username) || string.IsNullOrEmpty(collection?.Password)) {
                return BadRequest(_localizer[DataTransferer.DefectiveUsernameOrPassword().Message]);
            }

            var deviceHeader = GetDeviceInfosFromHeader();
            if(deviceHeader == null) {
                return BadRequest(_localizer[DataTransferer.UnofficialRequest().Message]);
            }

            collection.DeviceId = deviceHeader.DeviceId;
            collection.DeviceName = deviceHeader.DeviceName;
            collection.DeviceType = deviceHeader.DeviceType;

            try {
                var result = await _accountService.SigninAsync(collection).ConfigureAwait(true);
                switch(result.Code) {
                    case 200:
                        return Ok(result.Data);
                    case 400:
                        return BadRequest(result.Message);
                    case 500:
                    default:
                        return Problem(result.Message);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, Route("signout")]
        public async Task<IActionResult> SignoutAsync() {
            await HttpContext.SignOutAsync().ConfigureAwait(false);
            return Ok();
        }

        [HttpPost, HttpHeaderBinder, Route("changepassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordBindingModel collection) {
            Log.Debug($"ChangePassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection?.Password) || string.IsNullOrEmpty(collection.NewPassword) || string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(_localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(_localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                var account = await _accountService.FirstAsync(new AccountGetFirstSchema {
                    Id = CurrentAccount.Id
                }).ConfigureAwait(true);

                if(account == null) {
                    return BadRequest(_localizer[DataTransferer.UserNotFound().Message]);
                }

                if(_cryptograph.IsEqual(collection.Password, account.Password)) {
                    await _accountService.UpdateAsync(new AccountUpdateSchema {
                        Id = account.Id.Value,
                        Password = _cryptograph.RNG(collection.NewPassword)
                    }).ConfigureAwait(false);

                    return Ok(_localizer[DataTransferer.PasswordChanged().Message]);
                }
                else {
                    return Unauthorized(_localizer[DataTransferer.WrongPassword().Message]);
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, AllowAnonymous, Route("changeforgotenpassword")]
        public async Task<IActionResult> ChangeForgotenPasswordAsync([FromBody] ChangeForgotenPasswordBindingModel collection) {
            Log.Debug($"ChangeForgotenPassword => {JsonConvert.SerializeObject(collection)}");

            try {
                if(string.IsNullOrEmpty(collection?.Username)) {
                    return BadRequest(_localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
                }
                collection.Username = collection.Username.Trim();

                if(string.IsNullOrEmpty(collection.NewPassword) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                    return BadRequest(_localizer[DataTransferer.DefectivePassword().Message]);
                }

                if(collection.NewPassword != collection.ConfirmPassword) {
                    return BadRequest(_localizer[DataTransferer.PasswordsMissmatch().Message]);
                }

                if(string.IsNullOrWhiteSpace(collection.Token)) {
                    return BadRequest(_localizer[DataTransferer.UnofficialRequest().Message]);
                }

                var query = new AccountProfileGetFirstSchema { LinkedId = collection.Username };
                if(collection.Username.IsPhoneNumber()) {
                    query.TypeId = AccountProfileType.Phone.ToInt();
                }
                else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                    query.TypeId = AccountProfileType.Email.ToInt();
                }
                else {
                    return BadRequest(_localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
                }

                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(_localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(_localizer[DataTransferer.EmailNotFound().Message]);
                }

                var cachedToken = _memoryCache.Get(collection.Username);
                if(cachedToken == null) {
                    return BadRequest(_localizer[DataTransferer.ChangingPasswordWithoutToken().Message]);
                }

                var account = await _accountService.FirstAsync(new AccountGetFirstSchema {
                    Id = accountProfile.AccountId.Value
                }).ConfigureAwait(true);

                if(account == null) {
                    return BadRequest(_localizer[DataTransferer.UserNotFound().Message]);
                }

                if(collection.Token != cachedToken.ToString()) {
                    Log.Warning($"Account => {account}, AccountProfile => {accountProfile}, It tried to change its password with a wrong 'ForgotPasswordToken'");
                    return BadRequest(_localizer[DataTransferer.ChangingPasswordWithWrongToken().Message]);
                }

                _memoryCache.Remove(collection.Username);

                await _accountService.UpdateAsync(new AccountUpdateSchema {
                    Id = account.Id.Value,
                    Password = _cryptograph.RNG(collection.NewPassword)
                }).ConfigureAwait(false);

                return Ok(_localizer[DataTransferer.PasswordChanged().Message]);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpGet, AllowAnonymous, Route("changepasswordrequested")]
        public async Task<IActionResult> ChangePasswordRequestedAsync([FromQuery] string username, [FromQuery] string token) {
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token)) {
                return BadRequest(_localizer[DataTransferer.DefectiveEntry().Message]);
            }

            var result = new ChangeForgotenPasswordViewModel {
                Username = Encoding.UTF8.GetString(Convert.FromBase64String(username)),
                Token = token
            };
            await Task.CompletedTask.ConfigureAwait(true);

            return Ok(result);
        }

        [HttpPost, AllowAnonymous, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordBindingModel collection) {
            Log.Debug($"ForgotPassword => {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection?.Username)) {
                return BadRequest(_localizer[DataTransferer.DefectiveEmailOrCellPhone().Message]);
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
                    return BadRequest(_localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
                }

                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(_localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(_localizer[DataTransferer.EmailNotFound().Message]);
                }

                var token = _randomMaker.NewToken();
                var username = Convert.ToBase64String(Encoding.UTF8.GetBytes(collection.Username));
                var changepassurl = $"clipboardy.com/api/account/changepasswordrequested?username={username}&token={token}";
                _memoryCache.Set(username, token, DateTime.Now.AddMinutes(10));

                if(query.TypeId == AccountProfileType.Phone.ToInt()) {
                    await _smsService.SendAsync(new SMSModel {
                        PhoneNo = accountProfile.LinkedId,
                        TextBody = $"{DataTransferer.ForgotPasswordSMSBody().Message} \r\n {changepassurl}"
                    }).ConfigureAwait(false);
                }
                else {
                    await _emailService.SendAsync(new EmailModel {
                        Address = accountProfile.LinkedId,
                        Subject = _localizer[DataTransferer.ForgotPasswordEmailSubject().Message],
                        IsBodyHtml = true,
                        Body = $"<p>{DataTransferer.ForgotPasswordEmailBody().Message}</p>" +
                            $"<p>{changepassurl}</p>"
                    }).ConfigureAwait(false);
                }

                return Ok(changepassurl);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme), Route("activationrequest")]
        public async Task<ActionResult> ActivationRequestAsync() {
            var query = new AccountProfileGetFirstSchema { LinkedId = CurrentAccount.Username };
            if(CurrentAccount.Username.IsPhoneNumber()) {
                query.TypeId = AccountProfileType.Phone.ToInt();
            }
            else if(new EmailAddressAttribute().IsValid(CurrentAccount.Username)) {
                query.TypeId = AccountProfileType.Email.ToInt();
            }
            else {
                return BadRequest(_localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
            }

            try {
                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(_localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(_localizer[DataTransferer.EmailNotFound().Message]);
                }

                var activationCode = _randomMaker.NewNumber(10000, 99999);
                _memoryCache.Set(CurrentAccount.Username, activationCode, DateTime.Now.AddMinutes(10));

                if(query.TypeId == AccountProfileType.Phone.ToInt()) {
                    await _smsService.SendAsync(new SMSModel {
                        PhoneNo = accountProfile.LinkedId,
                        TextBody = $"{DataTransferer.ActivationCodeSMSBody().Message} \r\n {activationCode}"
                    }).ConfigureAwait(false);
                }
                else {
                    await _emailService.SendAsync(new EmailModel {
                        Address = accountProfile.LinkedId,
                        Subject = _localizer[DataTransferer.ActivationCodeEmailSubject().Message],
                        IsBodyHtml = true,
                        Body = $"<p>{DataTransferer.ActivationCodeEmailBody().Message}</p>" +
                            $"<p>{activationCode}</p>"
                    }).ConfigureAwait(false);
                }

                return Ok(_localizer[DataTransferer.ActivationCodeRequested().Message]);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem();
            }
        }

        [HttpPost, Authorize, Route("activateaccount")]
        public async Task<ActionResult> ActivateAccountAsync([FromBody] ActivateAccountBindingModel collection) {
            if(collection == null ||
                string.IsNullOrWhiteSpace(collection.Username) ||
                string.IsNullOrWhiteSpace(collection.Code)) {

                return BadRequest(_localizer[DataTransferer.DefectiveEntry().Message]);
            }

            var query = new AccountProfileGetFirstSchema { LinkedId = collection.Username };
            if(collection.Username.IsPhoneNumber()) {
                query.TypeId = AccountProfileType.Phone.ToInt();
            }
            else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                query.TypeId = AccountProfileType.Email.ToInt();
            }
            else {
                return BadRequest(_localizer[DataTransferer.InvalidEmailOrCellPhone().Message]);
            }

            try {
                var accountProfile = await _accountProfileService.FirstAsync(query).ConfigureAwait(true);
                if(accountProfile == null) {
                    if(query.TypeId == AccountProfileType.Phone.ToInt())
                        return BadRequest(_localizer[DataTransferer.PhoneNotFound().Message]);

                    if(query.TypeId == AccountProfileType.Email.ToInt())
                        return BadRequest(_localizer[DataTransferer.EmailNotFound().Message]);
                }

                var activationCode = _memoryCache.Get(collection.Username);
                if(activationCode == null) {
                    return BadRequest(_localizer[DataTransferer.ActivationCodeRequestedNotFound().Message]);
                }

                if(collection.Code != activationCode.ToString()) {
                    return BadRequest(_localizer[DataTransferer.ActivationCodeRequestedNotFound().Message]);
                }

                _memoryCache.Remove(collection.Username);
                var accountProfileQuery = new AccountProfileUpdateSchema {
                    Id = accountProfile.Id.Value,
                    StatusId = Status.Active.ToInt()
                };
                await _accountProfileService.UpdateAsync(accountProfileQuery).ConfigureAwait(true);

                return Ok(_localizer[DataTransferer.AccountActivated().Message]);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem();
            }
        }
    }
}