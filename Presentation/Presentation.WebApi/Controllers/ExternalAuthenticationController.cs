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

namespace Presentation.WebApi.Controllers {
    [Route("api/[controller]")]
    public class ExternalAuthenticationController: BaseController {
        #region ctor

        public ExternalAuthenticationController(
            ) {

        }
        #endregion

        #region private
        private Task<AuthenticateResult> AuthenticationManager {
            get {
                return HttpContext.AuthenticateAsync();
            }
        }
        #endregion

        [HttpGet, AllowAnonymous, Route("signin")]
        public async Task<IActionResult> Signin([FromQuery]string provider = "Google", string returnUrl = null) {
            var properties = new AuthenticationProperties {
                RedirectUri = Url.Action("signincallback", "externalauthentication", new { returnUrl })
                //RedirectUri = $"http://localhost:2020/api/externalauthentication/signincallback?returnUrl={returnUrl}"
            };

            return new ChallengeResult(provider, properties);

            //Context.GetOwinContext().Authentication.Challenge(authenticationProperties, provider);
            //Response.StatusCode = 401;
            //Response.End();

            //return Challenge(properties);
        }

        [HttpGet, AllowAnonymous, Route("signinfailure")]
        public async Task<IActionResult> SigninFailureAsync([FromQuery]string provider = "Google") {
            var msg = $"Failed to signin to '{provider}'";
            await Task.CompletedTask.ConfigureAwait(false);
            return InternalServerError(message: msg);
        }

        [HttpGet, AllowAnonymous, Route("signincallback")]
        public async Task<IActionResult> SigninCallbackAsync(string returnUrl = null, string remoteError = null) {
            try {
                //var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();

                var request = HttpContext.Request;
                //Here we can retrieve the claims
                // read external identity from the temporary cookie
                //var authenticateResult = HttpContext.GetOwinContext().Authentication.AuthenticateAsync("ExternalCookie");
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if(result.Succeeded != true) {
                    throw new Exception("External authentication error");
                }

                // retrieve claims of the external user
                var externalUser = result.Principal;
                if(externalUser == null) {
                    throw new Exception("External authentication error");
                }

                // retrieve claims of the external user
                var claims = externalUser.Claims.ToList();

                // try to determine the unique id of the external user - the most common claim type for that are the sub claim and the NameIdentifier
                // depending on the external provider, some other claim type might be used
                //var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
                var userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if(userIdClaim == null) {
                    throw new Exception("Unknown userid");
                }

                var externalUserId = userIdClaim.Value;
                var externalProvider = userIdClaim.Issuer;

                // use externalProvider and externalUserId to find your user, or provision a new user

                //return RedirectToAction("Privacy", "Home");


                var result2 = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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