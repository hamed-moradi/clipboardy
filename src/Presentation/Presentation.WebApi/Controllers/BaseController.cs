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

namespace Presentation.WebApi.Controllers {
  [Security, ApiController, Route("api/[controller]")]
  public class BaseController: Controller {
    #region ctor
    protected readonly IMapper _mapper;
    protected readonly IStringLocalizer<BaseController> _localizer;
    protected readonly IWebHostEnvironment _webHostEnvironment;
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
      var deviceId = HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "deviceid").Value;

      var deviceName = HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicename").Value;

      var deviceType = HttpContext.Request.Headers
          .FirstOrDefault(f => f.Key.ToLower(CultureInfo.CurrentCulture) == "devicetype").Value;

      if(_webHostEnvironment.IsDevelopment()) {
        deviceId = string.IsNullOrWhiteSpace(deviceId) ? "deviceId" : deviceId.ToString();
        deviceName = string.IsNullOrWhiteSpace(deviceName) ? "deviceName" : deviceName.ToString();
        deviceType = string.IsNullOrWhiteSpace(deviceType) ? "deviceType" : deviceType.ToString();
      }

      return (string.IsNullOrWhiteSpace(deviceId) || string.IsNullOrWhiteSpace(deviceName))// || string.IsNullOrWhiteSpace(deviceType)
          ? null
          : new Device {
            DeviceId = deviceId,
            DeviceName = deviceName,
            DeviceType = deviceType
          };
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