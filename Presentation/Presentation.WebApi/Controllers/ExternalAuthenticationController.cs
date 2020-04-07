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

namespace Presentation.WebApi.Controllers {
    [Route("api/[controller]")]
    public class ExternalAuthenticationController: BaseController {
        #region ctor
        private readonly IAccountService _accountService;
        private readonly IAccountProfileService _accountProfileService;

        public ExternalAuthenticationController(
            IAccountService accountService,
            IAccountProfileService accountProfileService) {

            _accountService = accountService;
            _accountProfileService = accountProfileService;
        }
        #endregion

        #region private
        private Task<AuthenticateResult> AuthenticationManager {
            get {
                return HttpContext.AuthenticateAsync();
            }
        }

        private ExternalUserBindingModel GetExternalUser(IEnumerable<Claim> claims) {
            var result = new ExternalUserBindingModel();
            var objectAccessor = ObjectAccessor.Create(result);
            foreach(var item in CustomClaimTypes.List) {
                var claim = claims.FirstOrDefault(x => x.Type == item.Value);
                objectAccessor[item.Key] = claim?.Value;
                if(result.ProviderId == AccountProvider.Clipboard)
                    result.ProviderId = claim.Issuer.ToProvider();
            }
            return result;
        }
        #endregion

        [HttpGet, AllowAnonymous, Route("signin")]
        public async Task<IActionResult> Signin([FromQuery]string provider = "Google") {
            if(string.IsNullOrWhiteSpace(ContextHeader.DeviceId)) ContextHeader.DeviceId = "DeviceId";
            if(string.IsNullOrWhiteSpace(ContextHeader.DeviceName)) ContextHeader.DeviceName = "DeviceName";
            if(string.IsNullOrWhiteSpace(ContextHeader.DeviceType)) ContextHeader.DeviceType = "DeviceType";

            var userdata = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ContextHeader)));
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
        public async Task<IActionResult> SigninFailureAsync([FromQuery]string provider = "Google") {
            var msg = $"Failed to signin to '{provider}'";
            await Task.CompletedTask.ConfigureAwait(false);
            return InternalServerError(message: msg);
        }

        [HttpGet, AllowAnonymous, Route("signincallback")]
        public async Task<IActionResult> SigninCallbackAsync(string userData = null, string remoteError = null) {
            try {
                if(string.IsNullOrWhiteSpace(userData)) {
                    Log.Error("userData is not defined");
                    return BadRequest(message: _localizer[ResourceMessage.DefectiveEntry]);
                }

                var headerbindingmodel = JsonConvert.DeserializeObject<BaseHeaderBindingModel>(Encoding.UTF8.GetString(Convert.FromBase64String(userData)));
                if(headerbindingmodel == null
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceId)
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceName)
                    || string.IsNullOrWhiteSpace(headerbindingmodel.DeviceType)) {

                    Log.Error("userData is not valid");
                    return BadRequest(message: _localizer[ResourceMessage.DefectiveEntry]);
                }

                // read external identity from the temporary cookie
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(true);

                if(!result.Succeeded) {
                    Log.Error("External authentication failure");
                    return InternalServerError(message: _localizer[ResourceMessage.ExternalAuthenticationError]);
                }

                // retrieve claims of the external user
                var claimPrincipal = result.Principal;
                if(claimPrincipal == null) {
                    Log.Error("External authentication user principal error");
                    return InternalServerError(message: _localizer[ResourceMessage.ExternalAuthenticationError]);
                }

                // transform claims list to model
                var externalUser = GetExternalUser(claimPrincipal.Claims);
                if(string.IsNullOrWhiteSpace(externalUser.Email)) {
                    Log.Error("External authentication user email not found");
                    return InternalServerError(message: _localizer[ResourceMessage.ExternalAuthenticationError]);
                }
                externalUser.DeviceId = headerbindingmodel.DeviceId;
                externalUser.DeviceName = headerbindingmodel.DeviceName;
                externalUser.DeviceType = headerbindingmodel.DeviceType;

                var accountprofile = await _accountProfileService.FirstAsync(f => f.Email == externalUser.Email).ConfigureAwait(true);
                if(accountprofile == null) {
                    Log.Debug($"User {User.Identity.Name} try to sign in for first time at {DateTime.UtcNow}");
                    return Ok(_accountService.ExternalSignup(externalUser));
                }
                else {
                    Log.Debug($"Account with Id={accountprofile.AccountId} try to sign in at {DateTime.UtcNow}");
                    return Ok(_accountService.ExternalSignin(externalUser, accountprofile));
                }
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }

        [HttpPost, AllowAnonymous, Route("signinconfirmation")]
        public async Task<IActionResult> SigninConfirmationAsync() {
            try {


                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError(message: _localizer[ResourceMessage.SomethingWentWrong]);
            }
        }
    }
}