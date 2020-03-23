using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Model.Binding;
using Assets.Model.View;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.FilterAttributes;
using Serilog;

namespace Presentation.WebApi.Controllers {
    public class ClipboardController: BaseController {
        #region ctor
        private readonly IClipboardService _clipboardService;

        public ClipboardController(
            IClipboardService clipboardService) {

            _clipboardService = clipboardService;
        }
        #endregion

        [HttpGet, ArgumentBinder, Route("get")]
        public async Task<IActionResult> Get([FromQuery]ClipboardGetBindingModel collection) {
            try {
                var model = _mapper.Map<Clipboard>(collection);
                var clipboards = await _clipboardService.GetPagingAsync<ClipboardViewModel>(model, collection.QuerySetting).ConfigureAwait(true);
                return Ok(data: clipboards.Result, totalPages: clipboards.TotalPage);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError();
            }
        }

        [HttpPost, ArgumentBinder, Route("add")]
        public async Task<IActionResult> Add([FromBody]ClipboardAddBindingModel collection) {
            try {
                var model = _mapper.Map<Clipboard>(collection);
                model.DeviceId = collection.Account.DeviceId;
                var clipboard = await _clipboardService.AddAsync<ClipboardViewModel>(model).ConfigureAwait(true);
                return Ok(data: clipboard);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError();
            }
        }
    }
}