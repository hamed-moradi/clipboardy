using Assets.Model.Base;
using Assets.Model.Header;
using Assets.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Presentation.WebApi.FilterAttributes;
using System.Globalization;
using System.Linq;

namespace Presentation.WebApi.Controllers
{
    [Security, CustomAuthentication, ApiController, Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        #region ctor
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<BaseController> _localizer;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        protected AccountHeaderModel CurrentAccount { get { return (AccountHeaderModel)HttpContext.Items[nameof(CurrentAccount)]; } }
        protected string IP { get { return HttpContext.Connection.RemoteIpAddress.ToString(); } }
        protected string URL { get { return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}"; } }
       
        public BaseController()
        {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
            _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer<BaseController>>();
            _webHostEnvironment = ServiceLocator.Current.GetInstance<IWebHostEnvironment>();
        }
        #endregion

        [ApiExplorerSettings(IgnoreApi = true)]
        protected Device GetDeviceInfosFromHeader()
        {
            var deviceKey = HttpContext.Request.Headers
                .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicekey").Value;

            var deviceName = HttpContext.Request.Headers
                .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicename").Value;

            var deviceType = HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicetype").Value;

            if (_webHostEnvironment.IsDevelopment())
            {
                deviceKey = string.IsNullOrWhiteSpace(deviceKey) ? "deviceId" : deviceKey.ToString();
                deviceName = string.IsNullOrWhiteSpace(deviceName) ? "deviceName" : deviceName.ToString();
                deviceType = string.IsNullOrWhiteSpace(deviceType) ? "deviceType" : deviceType.ToString();
            }

            return (string.IsNullOrWhiteSpace(deviceKey) || string.IsNullOrWhiteSpace(deviceName))// || string.IsNullOrWhiteSpace(deviceType)
                ? null
                : new Device
                {
                    DeviceKey = deviceKey,
                    DeviceName = deviceName,
                    DeviceType = deviceType
                };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Respond(IServiceResult serviceResult)
        {
            switch (serviceResult.Code)
            {
                case 200:
                    {
                        return new ObjectResult(serviceResult.Data)
                        {
                            StatusCode = serviceResult.Code,
                        };
                    }
                default:
                case 400:
                case 500:
                    {
                        return new ObjectResult(serviceResult.Message)
                        {
                            StatusCode = serviceResult.Code,
                        };
                    }
            }
        }
    }

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public IActionResult Raw(BaseViewModel viewModel) {
    //    return Json(viewModel);
    //}

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public IActionResult Ok(HttpStatusCode status = HttpStatusCode.OK, string message = null, object data = null, long? totalPages = null) {
    //    message ??= _localizer[DataTransferer.Ok().Message];
    //    return Json(new BaseViewModel { Status = status, Message = message, Data = data, TotalPages = totalPages });
    //}

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public IActionResult BadRequest(HttpStatusCode status = HttpStatusCode.BadRequest, string message = null) {
    //    message ??= _localizer[DataTransferer.BadRequest().Message];
    //    return Json(new BaseViewModel { Status = status, Message = message });
    //}

    //[ApiExplorerSettings(IgnoreApi = true)]
    //public IActionResult Problem(Exception exception) {
    //    Log.Error(exception, exception?.Source);
    //    return Problem(detail: _localizer[DataTransferer.InternalServerError().Message]);
    //}
}