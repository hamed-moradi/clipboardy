using Assets.Model.Base;
using Assets.Model.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Linq;

namespace Presentation.WebApi.FilterAttributes {
    public class ContextHeaderAttribute: ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            var headerDeviceId = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(ContextHeader.DeviceId).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceName = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(ContextHeader.DeviceName).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceType = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(ContextHeader.DeviceType).ToLower(CultureInfo.CurrentCulture)
            ).Value;

            context?.HttpContext.Items.Add(nameof(ContextHeader), new ContextHeader {
                DeviceId = headerDeviceId.Value.ToString(),
                DeviceName = headerDeviceName.Value.ToString(),
                DeviceType = headerDeviceType.Value.ToString()
            });
        }
    }
}
