using System;
using System.Threading.Tasks;
using Assets.Model.Binding;
using Assets.Model.View;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApi.FilterAttributes;
using Serilog;

namespace Presentation.WebApi.Controllers {
    [Route("api/[controller]")]
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
            var t = ContextHeader;
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
                model.DeviceId = collection.AccountHeader.DeviceId;

                var duplicated = await _clipboardService.FirstAsync(
                    f => f.AccountId == collection.AccountHeader.Id
                    && f.Content == model.Content).ConfigureAwait(true);

                if(duplicated != null) {
                    duplicated.Priority = DateTime.Now;
                    await _clipboardService.UpdateAsync(duplicated).ConfigureAwait(false);
                }
                else {
                    await _clipboardService.AddAsync<ClipboardViewModel>(model).ConfigureAwait(true);
                }

                return Ok();
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return InternalServerError();
            }
        }
    }
}