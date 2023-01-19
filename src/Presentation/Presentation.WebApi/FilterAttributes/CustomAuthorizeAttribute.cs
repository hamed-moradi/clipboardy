using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Presentation.WebApi.FilterAttributes
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute {
    #region ctor
    private readonly IStringLocalizer _localizer;
    private readonly IAccountService _accountService;
    private readonly JwtHandler _jwtHandler;

    public CustomAuthorizeAttribute() {
      _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer>();
      _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
      _jwtHandler = ServiceLocator.Current.GetInstance<JwtHandler>();
    }
    #endregion
  }
}
