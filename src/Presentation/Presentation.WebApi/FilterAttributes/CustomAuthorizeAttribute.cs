using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Model.Header;
using Assets.Utility;
using Assets.Utility.Extension;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.WebApi.FilterAttributes {
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
