using System.Net;
using Assets.Model.Base;
using Assets.Model.Common;
using Assets.Resource;
using Assets.Utility;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Presentation.WebApi.FilterAttributes;

namespace Presentation.WebApi.Controllers {
    [ContextHeader, Security, ApiController, Route("api/[controller]")]//, Authorize
    public class BaseController: Controller {
        #region ctor
        protected readonly IMapper _mapper;
        protected readonly IStringLocalizer<BaseController> _localizer;
        protected ContextHeader ContextHeader { get { return (ContextHeader)HttpContext.Items[nameof(ContextHeader)]; } }
        protected string IP { get { return HttpContext.Connection.RemoteIpAddress.ToString(); } }
        protected string URL { get { return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}{HttpContext.Request.QueryString}"; } }

        public BaseController() {
            _mapper = ServiceLocator.Current.GetInstance<IMapper>();
            _localizer = ServiceLocator.Current.GetInstance<IStringLocalizer<BaseController>>();
        }
        #endregion

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Raw(BaseViewModel viewModel) {
            return Json(viewModel);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Ok(HttpStatusCode status = HttpStatusCode.OK, string message = null, object data = null, long? totalPages = null) {
            message ??= _localizer[ResourceMessage.Ok];
            return Json(new BaseViewModel { Status = status, Message = message, Data = data, TotalPages = totalPages });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BadRequest(HttpStatusCode status = HttpStatusCode.BadRequest, string message = null) {
            message ??= _localizer[ResourceMessage.BadRequest];
            return Json(new BaseViewModel { Status = status, Message = message });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult InternalServerError(HttpStatusCode status = HttpStatusCode.InternalServerError, string message = null) {
            message ??= _localizer[ResourceMessage.InternalServerError];
            return Json(new BaseViewModel { Status = status, Message = message });
        }
    }
}