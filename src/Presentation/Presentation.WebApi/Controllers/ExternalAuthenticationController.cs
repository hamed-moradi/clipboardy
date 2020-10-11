using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Assets.Resource;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Web;
using Microsoft.AspNetCore.Http;
using Core.Application;
using Assets.Model.Binding;
using System.Collections.Generic;
using FastMember;
using Assets.Model;
using Assets.Utility.Extension;
using Assets.Model.Base;
using System.Text;
using Newtonsoft.Json;
using Presentation.WebApi.FilterAttributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Core.Domain.StoredProcedure.Schema;
using Assets.Model.Common;
using Assets.Utility.Infrastructure;
using Assets.Model.Header;
using System.Globalization;

namespace Presentation.WebApi.Controllers {
    [Route("api/[controller]")]
    public class ExternalAuthenticationController: BaseController {
        #region ctor
        private readonly IAccountService _accountService;
        private readonly IAccountProfileService _accountProfileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExternalAuthenticationController(
            IAccountService accountService,
            IAccountProfileService accountProfileService,
            IWebHostEnvironment webHostEnvironment) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region private

        private ExternalUserBindingModel GetExternalUser(IEnumerable<Claim> claims) {
            var result = new ExternalUserBindingModel();
            var objectAccessor = ObjectAccessor.Create(result);
            foreach(var item in claims) {
                var claim = claims.FirstOrDefault(x => x.Type == item.Value);
                objectAccessor[item.Type] = claim?.Value;
                if(result.ProviderId == AccountProvider.Clipboardy)
                    result.ProviderId = claim.Issuer.ToProvider();
            }
            return result;
        }

        #endregion

        [HttpGet("signin"), AllowAnonymous, HttpHeaderBinder]
        public async Task<IActionResult> Signin([FromQuery] string provider = "Google") {
            var deviceHeader = GetDeviceInfosFromHeader();
            if(deviceHeader == null && _webHostEnvironment.IsDevelopment()) {
                deviceHeader = new Device {
                    DeviceId = "DeviceId",
                    DeviceName = "DeviceName",
                    DeviceType = "DeviceType"
                };
            }
            else if(deviceHeader == null) {
                return BadRequest(DataTransferer.DefectiveEntry().Message);
            }

            var userdata = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceHeader)));
            var redirecturl = $"/api/externalauthentication/signincallback?userdata={userdata}";

            var authprops = new AuthenticationProperties {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                IsPersistent = true,
                RedirectUri = redirecturl
            };

            Response.Redirect(redirecturl);
            await Response.CompleteAsync().ConfigureAwait(true);

            return Challenge(authprops, provider);
        }

        [HttpPost, Route("signout")]
        public async Task<IActionResult> Signout() {
            Log.Debug($"User {User.Identity.Name} signed out at {DateTime.UtcNow}");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);

            return RedirectToAction("index", "home");
        }

        [HttpGet, AllowAnonymous, Route("signinfailure")]
        public async Task<IActionResult> SigninFailureAsync([FromQuery] string provider = "Google") {
            var msg = $"Failed to signin to '{provider}'";
            await Task.CompletedTask.ConfigureAwait(false);
            return Problem(msg);
        }

        [HttpGet, AllowAnonymous, Route("signincallback")]
        public async Task<IActionResult> SigninCallbackAsync(string userData = null, string remoteError = null) {
            try {
                if(string.IsNullOrWhiteSpace(userData)) {
                    Log.Error("userData is not defined");
                    return BadRequest(_localizer[DataTransferer.DefectiveEntry().Message]);
                }

                var headerbindingmodel = JsonConvert.DeserializeObject<Device>(Encoding.UTF8.GetString(Convert.FromBase64String(userData)));
                if(headerbindingmodel == null
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceId)
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceName)
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceType)) {

                    Log.Error("userData is not valid");
                    return BadRequest(_localizer[DataTransferer.DefectiveEntry().Message]);
                }

                // read external identity from the temporary cookie
                var authenticationResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(true);

                if(!authenticationResult.Succeeded) {
                    Log.Error("External authentication failed");
                    return Problem(_localizer[DataTransferer.ExternalAuthenticationFailed().Message]);
                }

                // retrieve claims of the external user
                var claimPrincipal = authenticationResult.Principal;
                if(claimPrincipal == null) {
                    Log.Error("External authentication user principal error");
                    return Problem(_localizer[DataTransferer.ExternalAuthenticationUserError().Message]);
                }

                // transform claims list to model
                var externalUser = GetExternalUser(claimPrincipal.Claims);

                if(string.IsNullOrWhiteSpace(externalUser.Email)) {
                    Log.Error("External authentication user email not found");
                    return Problem(_localizer[DataTransferer.ExternalAuthenticationEmailError().Message]);
                }

                if(externalUser.ProviderId == AccountProvider.Clipboardy) {
                    Log.Error("External signup with unknown ProviderId");
                    return Problem(_localizer[DataTransferer.ExternalAuthenticationWithUnknownProvider().Message]);
                }

                externalUser.DeviceId = headerbindingmodel.DeviceId;
                externalUser.DeviceName = headerbindingmodel.DeviceName;
                externalUser.DeviceType = headerbindingmodel.DeviceType;

                var accountprofile = await _accountProfileService.FirstAsync(new AccountProfileGetFirstSchema {
                    TypeId = AccountProfileType.Email.ToInt(),
                    LinkedId = externalUser.Email
                }).ConfigureAwait(true);
                if(accountprofile == null) {
                    Log.Debug($"User {User.Identity.Name} try to sign in for first time at {DateTime.UtcNow}");
                    return Ok(_accountService.ExternalSignupAsync(externalUser));
                }
                else {
                    Log.Debug($"Account with Id={accountprofile.AccountId} try to sign in at {DateTime.UtcNow}");
                    var result = await _accountService.ExternalSigninAsync(externalUser, accountprofile).ConfigureAwait(false);
                    switch(result.Code) {
                        case 200:
                            return Ok(result.Data);
                        case 500:
                            return Problem(_localizer[result.Message]);
                        default:
                            return BadRequest(_localizer[result.Message]);
                    }
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }

        [HttpPost, AllowAnonymous, Route("signinconfirmation")]
        public async Task<IActionResult> SigninConfirmationAsync() {
            try {


                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(_localizer[DataTransferer.SomethingWentWrong().Message]);
            }
        }
    }
}