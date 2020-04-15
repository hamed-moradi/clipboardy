using Assets.Model.Common;
using Assets.Utility;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
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
            // get device infos
            var deviceId = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceId).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();
            var deviceName = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceName).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();
            var deviceType = (context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceType).ToLower(CultureInfo.CurrentCulture)
            ).Value).Value.ToString();

            // set device info in defined object for easy access
            context?.HttpContext.Items.Add(nameof(HttpDeviceHeader), new HttpDeviceHeader {
                DeviceId = deviceId,
                DeviceName = deviceName,
                DeviceType = deviceType
            });

            if(HttpAccountHeader) {
                // get token
                var token = (context?.HttpContext.Request.Headers.FirstOrDefault(f => f.Key.ToLower() == "token").Value).Value.ToString();

                // find user account based on token
                var schema = new AccountAuthenticateSchema { Token = token };
                var result = _accountService.AuthenticateAsync(schema).GetAwaiter().GetResult();
                switch(schema.StatusCode) {
                    case 404:
                        throw new Exception(_localizer[DataTransferer.UserNotFound().Message]);
                    case 401:
                        throw new Exception(_localizer[DataTransferer.UserIsNotActive().Message]);
                    case 200:
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
