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
        collection.AccountId = CurrentAccount.Id;
        collection.DeviceId = CurrentAccount.DeviceId;
        var (list, totalCount, pageCount) = await _clipboardService.GetPagingAsync(collection);
        return Ok(new { list, totalCount, pageCount });
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
        model.account_id = CurrentAccount.Id;
        model.device_id = CurrentAccount.DeviceId;
        await _clipboardService.AddAsync(model);

        return Ok();
      }
      catch(Exception ex) {
        Log.Error(ex, ex.Source);
        return Problem(ex.Message);
      }
    }
  }
}