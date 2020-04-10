using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Resource;
using Assets.Utility;
using Core.Application;
using Core.Domain.StoredProcSchema;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Presentation.WebApi.FilterAttributes {
    public class ArgumentBinderAttribute: ActionFilterAttribute {
        #region ctor
        private readonly IStringLocalizer _localizer;
        private readonly IAccountService _accountService;
        public bool HttpAccountHeader { get; set; } = true;

        public ArgumentBinderAttribute() {
            _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer>();
            _accountService = ServiceLocator.Current.GetInstance<IAccountService>();
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context) {
            var token = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.Token).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();
            var deviceId = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceId).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();
            var deviceName = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceName).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();
            var deviceType = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceType).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();

            context?.HttpContext.Items.Add(nameof(HttpDeviceHeader), new HttpDeviceHeader {
                DeviceId = deviceId,
                DeviceName = deviceName,
                DeviceType = deviceType
            });

            if(HttpAccountHeader) {
                var schema = new AccountAuthenticateSchema { Token = token };
                var result = _accountService.Authenticate(schema);
                switch(schema.StatusCode) {
                    case HttpStatusCode.NotFound:
                        throw new Exception(_localizer[ResourceMessage.UserNotFound]);
                    case HttpStatusCode.Unauthorized:
                        throw new Exception(_localizer[ResourceMessage.UserIsNotActive]);
                    case HttpStatusCode.OK:
                        context?.HttpContext.Items.Add(nameof(HttpAccountHeader), new HttpAccountHeader {
                            Id = result.Id,
                            Username = result.Username,
                            LastSignedinAt = result.LastSignedinAt,
                            DeviceId = result.DeviceId
                        });
                        break;
                }
            }
        }
    }
}
