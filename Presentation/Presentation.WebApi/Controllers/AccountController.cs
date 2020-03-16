using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Model;
using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Settings;
using Assets.Model.View;
using Core.Application;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Presentation.WebApi.Controllers {
    public class AccountController : BaseController {
        #region ctor


        public AccountController(
            ) {

        }
        #endregion
    
        [HttpPost, Route("signin")]
        public async Task<IActionResult> Invoke([FromBody]SigninBindingModel collection) {
            try {
                Log.Debug($"Message invoked by following data: {JsonConvert.SerializeObject(collection)}");

                return Ok(message: "The message sent.");
            }
            catch(Exception ex) {
                var errmsg = "The message wasn't sent.";
                Log.Error(ex, errmsg);
                return InternalServerError(errmsg);
            }
        }

        [HttpPost, Route("signingooglecallback")]
        public async Task<IActionResult> SigninGoogleCallback() {
            try {
                return Ok(message: "The message sent.");

            }
            catch(Exception ex) {
                var errmsg = "The message wasn't sent.";
                Log.Error(ex, errmsg);
                return InternalServerError(errmsg);
            }
        }
    }
}