using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Presentation.WebApi.FilterAttributes {
  public class CustomAuthenticationAttribute: ActionFilterAttribute {
    #region ctor
    private readonly IStringLocalizer<CustomAuthenticationAttribute> _localizer;
    private readonly IAccountService _accountService;
    private readonly JwtHandler _jwtHandler;

    public CustomAuthenticationAttribute() {
      _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer<CustomAuthenticationAttribute>>();
      _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
      _jwtHandler = ServiceLocator.Current.GetInstance<JwtHandler>();
    }
    #endregion

    public override void OnActionExecuting(ActionExecutingContext context) {
      var allowAnonymous = (context.ActionDescriptor as ControllerActionDescriptor)
        .MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>();
      if(allowAnonymous.Any()) return;

      var deviceKey = (context?.HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicekey").Value).ToString();
      if(string.IsNullOrWhiteSpace(deviceKey)) {
        throw new Exception(_localizer["DeviceKey not found"]);
      }

      var token = (context?.HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "authorization").Value)?.ToString();

      if(string.IsNullOrWhiteSpace(token)) {
        throw new Exception(_localizer["Authorization token not found"]);
      }

      token = token.Replace("Bearer ", string.Empty);
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
