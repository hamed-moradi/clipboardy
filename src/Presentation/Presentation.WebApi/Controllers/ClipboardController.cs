using System;
using System.Threading.Tasks;
using Assets.Model.Binding;
using Assets.Resource;
using Assets.Utility.Infrastructure;
using Core.Application;
using Core.Domain.StoredProcedure.Schema;
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

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] ClipboardGetBindingModel collection) {
            try {
                var query = _mapper.Map<ClipboardGetPagingSchema>(collection);
                var result = await _clipboardService.PagingAsync(query).ConfigureAwait(true);
                return Ok(new { result, query.TotalPages });
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem();
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ClipboardAddBindingModel collection) {
            try {
                var model = _mapper.Map<ClipboardAddSchema>(collection);

                var query = new ClipboardGetFirstSchema {
                    AccountId = CurrentAccount.Id,
                    Content = model.Content
                };
                var duplicated = await _clipboardService.FirstAsync(query).ConfigureAwait(true);

                if(duplicated == null) {
                    await _clipboardService.AddAsync(model).ConfigureAwait(true);
                    return Ok();
                }

                return BadRequest(_localizer[DataTransferer.DuplicatedValueFound().Message]);
            }
            catch(Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem();
            }
        }
    }
}