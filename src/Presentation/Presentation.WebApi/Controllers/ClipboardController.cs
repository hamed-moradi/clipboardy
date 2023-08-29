using Assets.Model.Binding;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.WebApi.Controllers
{
    public class ClipboardController : BaseController
    {
        #region ctor
        private readonly IClipboardService _clipboardService;

        public ClipboardController(
            IClipboardService clipboardService)
        {

            _clipboardService = clipboardService;
        }
        #endregion

        [HttpGet("get")]
        public async Task<IActionResult> Get([FromQuery] ClipboardGetBindingModel collection)
        {
            try
            {
                collection.AccountId = CurrentAccount.Id;
                collection.DeviceId = CurrentAccount.DeviceId;
                var (list, totalCount, pageCount) = await _clipboardService.GetPagingAsync(collection);
                return Ok(new { list, totalCount, pageCount });
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
                return Problem();
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ClipboardAddBindingModel collection)
        {
            try
            {
                if (collection.Content?.Length >= 5242880)
                {
                    return BadRequest(_localizer["SizeExceeded"]); // "Content is too long!"
                }

                var model = _mapper.Map<Clipboard>(collection);
                model.account_id = CurrentAccount.Id;
                model.device_id = CurrentAccount.DeviceId;
                await _clipboardService.AddAsync(model);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
                return Problem(ex.Message);
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ClipboardUpdateBindingModel ClipboardViewModel)
        {
            try
            {
                if (ClipboardViewModel.Content?.Length >= 5242880)
                {
                    return BadRequest(_localizer["SizeExceeded"]); // "Content is too long!"
                }

                var model = await _clipboardService.GetClipboardByID(ClipboardViewModel.Id);
                model.content = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClipboardViewModel.Content));               
                await _clipboardService.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Source);
                return Problem(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id )
        {
            try {
                var idIsValid = int.TryParse(id.ToString(), out id);
                if(idIsValid)
                {
                    await _clipboardService.DeleteClipboard(id);
                    await _clipboardService.SaveAsync();
                    return Ok();
                }
                else
                    return BadRequest();
            }

            catch (Exception ex) {
                Log.Error(ex, ex.Source);
                return Problem(ex.Message);
            }
        }
    }
}