using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;

namespace Presentation.WebApi.FilterAttributes {
  public class HttpHeaderBinderAttribute: ActionFilterAttribute {
    #region ctor
    private readonly IStringLocalizer _localizer;
    private readonly IAccountService _accountService;
    private readonly JwtHandler _jwtHandler;

    public HttpHeaderBinderAttribute() {
      _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer>();
      _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
      _jwtHandler = ServiceLocator.Current.GetInstance<JwtHandler>();
    }
    #endregion

    public override void OnActionExecuting(ActionExecutingContext context) {
      var token = context?.HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "authorization").Value[0];

      if(string.IsNullOrWhiteSpace(token)) {
        throw new Exception(_localizer["Authorization token not found"]);
      }

      token = token.StartsWith("Bearer ", false, CultureInfo.CurrentCulture) ? token.Remove(0, 7) : token;
      var claims = _jwtHandler.Validate(token);
      var account = claims.ToAccount();

      //var authorizeToken = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
      //    f.Key?.ToLower(CultureInfo.CurrentCulture) == "authorization"
      //).Value).Value.ToString();

      // fetch the user
      //var user = _accountService.GetById(account.Id);

      //validations
      //if(user == null) {
      //    throw new Exception(_localizer["User not found!"]);
      //}
      //if(user.StatusId != Status.Active) {
      //    throw new Exception(_localizer["User is not active!"]);
      //}

      //set user info in defined object for easy access
      context?.HttpContext.Items.Add("CurrentAccount", account);
    }
  }
}
