using Assets.Model;
using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Resource;
using Assets.Utility;
using Core.Application;
using FastMember;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Presentation.WebApi.FilterAttributes {
    public class ArgumentBinderAttribute: ActionFilterAttribute {
        #region ctor
        private readonly IStringLocalizer _localizer;
        private readonly IAccountDeviceService _accountDeviceService;
        public bool ThrowException { get; set; } = true;

        public ArgumentBinderAttribute() {
            _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer>();
            _accountDeviceService = ServiceLocator.Current.GetInstance<IAccountDeviceService>();
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context) {
            var headerToken = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.Token).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceId = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceId).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceName = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceName).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceType = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HeaderBindingModel.DeviceType).ToLower(CultureInfo.CurrentCulture)
            ).Value;

            var token = headerToken.Value.ToString();
            var deviceId = headerDeviceId.Value.ToString();
            var deviceName = headerDeviceName.Value.ToString();
            var deviceType = headerDeviceType.Value.ToString();

            if((string.IsNullOrWhiteSpace(token)
                || string.IsNullOrWhiteSpace(deviceId)
                || string.IsNullOrWhiteSpace(deviceName)
                || string.IsNullOrWhiteSpace(deviceType))
                && ThrowException) {

                throw new ArgumentNullException(_localizer[ResourceMessage.UnofficialRequest],
                    new Exception { Source = GlobalVariables.SystemGeneratedMessage });
            }
            else {
                var bindedmodel = context.ActionArguments.FirstOrDefault(f => f.Value is HeaderBindingModel);
                if(bindedmodel.Equals(new KeyValuePair<string, object>()) && ThrowException) {
                    throw new ArgumentNullException(_localizer["InvalidBindingModel"],
                        new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                }

                var objectAccessor = ObjectAccessor.Create(bindedmodel.Value);
                objectAccessor[nameof(HeaderBindingModel.Token)] = token;
                objectAccessor[nameof(HeaderBindingModel.DeviceId)] = deviceId;
                objectAccessor[nameof(HeaderBindingModel.DeviceName)] = deviceName;
                objectAccessor[nameof(HeaderBindingModel.DeviceType)] = deviceType;

                if(ThrowException) {
                    var device = _accountDeviceService.First(f => f.Token == token);
                    if(device == null) {
                        throw new ArgumentException(_localizer[ResourceMessage.TokenNotFound],
                            new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                    }
                    if(device.StatusId != Status.Active) {
                        throw new MemberAccessException("You're not an active user, Please confirm your Email or Phone number.",
                            new Exception { Source = GlobalVariables.SystemGeneratedMessage });
                    }

                    objectAccessor[nameof(HeaderBindingModel.AccountHeader)] = new AccountHeader {
                        Id = device.AccountId.Value,
                        DeviceId = device.Id.Value
                    };
                }
            }
        }
    }
}
