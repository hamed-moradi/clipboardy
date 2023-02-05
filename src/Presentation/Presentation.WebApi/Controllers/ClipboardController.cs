using Assets.Model.Binding;
using Core.Application.Interfaces;
using Core.Domain.Entities;
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
        var (list, pageCount) = await _clipboardService.GetPagingAsync(collection);
        return Ok(new { list, pageCount });
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return Problem();
      }
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] ClipboardAddBindingModel collection) {
      try {
        if(collection.Content?.Length >= 5242880) {
          return BadRequest(_localizer["SizeExceeded"]); // "Content is too long!"
        }

        var model = _mapper.Map<Clipboard>(collection);
        await _clipboardService.AddAsync(model);

        return Ok();
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return Problem();
      }
    }
  }
}