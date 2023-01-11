using Assets.Model.Binding;
using Core.Application;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

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
        //var query = _mapper.Map<ClipboardGetPagingSchema>(collection);
        //var result = await _clipboardService.PagingAsync(query).ConfigureAwait(true);
        //return Ok(new { result, query.TotalPages });
        return Ok();
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return Problem();
      }
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ClipboardAddBindingModel collection) {
      try {
        //var model = _mapper.Map<ClipboardAddSchema>(collection);

        //var query = new ClipboardGetFirstSchema {
        //  AccountId = CurrentAccount.Id,
        //  Content = model.Content
        //};
        //var duplicated = await _clipboardService.FirstAsync(query).ConfigureAwait(true);

        //if(duplicated == null) {
        //  await _clipboardService.AddAsync(model).ConfigureAwait(true);
        //  return Ok();
        //}

        //return BadRequest(_localizer[DataTransferer.DuplicatedValueFound().Message]);
        return Ok();
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return Problem();
      }
    }
  }
}