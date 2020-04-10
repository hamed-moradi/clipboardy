using Assets.Model.Base;
using Assets.Model.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Linq;

namespace Presentation.WebApi.FilterAttributes {
    public class ContextHeaderAttribute: ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context) {
            var headerDeviceId = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceId).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceName = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceName).ToLower(CultureInfo.CurrentCulture)
            ).Value;
            var headerDeviceType = context?.HttpContext.Request.Headers.FirstOrDefault(f =>
                f.Key?.ToLower(CultureInfo.CurrentCulture) == nameof(HttpDeviceHeader.DeviceType).ToLower(CultureInfo.CurrentCulture)
            ).Value;

            context?.HttpContext.Items.Add(nameof(HttpDeviceHeader), new HttpDeviceHeader {
                DeviceId = headerDeviceId.Value.ToString(),
                DeviceName = headerDeviceName.Value.ToString(),
                DeviceType = headerDeviceType.Value.ToString()
            });
        }
    }
}
