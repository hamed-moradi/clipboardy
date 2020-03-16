using System.Net;
using Assets.Model.Base;
using Assets.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.FilterAttributes;

namespace Presentation.WebApi.Controllers {
    [SecurityFilter, ApiController, Route("api/[controller]")]
    public class BaseController: Controller {
        #region ctor
        protected readonly IMapper _mapper;
        protected string IP { get { return HttpContext.Connection.RemoteIpAddress.ToString(); } }
        protected string URL { get { return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}"; } }

        public BaseController() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
        }
        #endregion

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Ok(HttpStatusCode status = HttpStatusCode.OK, string message = null, object data = null, int? totalPages = null) {
            message = message ?? "Okay";
            return Json(new BaseViewModel { Status = status, Message = message, Data = data, TotalPages = totalPages });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BadRequest(string message = null) {
            message= message ?? "Bad Request";
            return Json(new BaseViewModel { Status = HttpStatusCode.BadRequest, Message = message });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult InternalServerError(string message = null) {
            message= message ?? "Internal Server Error";
            return Json(new BaseViewModel { Status = HttpStatusCode.InternalServerError, Message = message });
        }
    }
}