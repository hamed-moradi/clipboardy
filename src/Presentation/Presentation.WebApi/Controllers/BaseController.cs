using System.Globalization;
using System.Linq;
using System.Net;
using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Model.Header;
using Assets.Resource;
using Assets.Utility;
using Assets.Utility.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Presentation.WebApi.FilterAttributes;

namespace Presentation.WebApi.Controllers {
    [Security, ApiController, Route("api/[controller]")]
    public class BaseController: Controller {
        #region ctor
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<BaseController> _localizer;
        private readonly IWebHostEnvironment _webHostEnvironment;

        protected Account CurrentAccount { get { return (Account)HttpContext.Items[nameof(CurrentAccount)]; } }
        protected string IP { get { return HttpContext.Connection.RemoteIpAddress.ToString(); } }
        protected string URL { get { return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}"; } }

        public BaseController() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
            _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer<BaseController>>();
            _webHostEnvironment = ServiceLocator.Current.GetInstance<IWebHostEnvironment>();
        }
        #endregion

        [ApiExplorerSettings(IgnoreApi = true)]
        protected Device GetDeviceInfosFromHeader() {
            Device device = null;

            var id = HttpContext.Request.Headers
                .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "deviceid").Value;

            var name = HttpContext.Request.Headers
                .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicename").Value;

            var type = HttpContext.Request.Headers
                .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicetype").Value;

            if(id.Any() && name.Any() && type.Any()) {
                device = new Device {
                    DeviceId = id[0],
                    DeviceName = name[0],
                    DeviceType = type[0]
                };
            }
            else if(_webHostEnvironment.IsDevelopment()) {
                device = new Device {
                    DeviceId = id.Any() ? id[0] : "TestDeviceId",
                    DeviceName = name.Any() ? name[0] : "TestDeviceName",
                    DeviceType = type.Any() ? type[0] : "TestDeviceType"
                };
            }

            return device;
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
        //public IActionResult InternalServerError(HttpStatusCode status = HttpStatusCode.InternalServerError, string message = null) {
        //    message ??= _localizer[DataTransferer.InternalServerError().Message];
        //    return Json(new BaseViewModel { Status = status, Message = message });
        //}
    }
}