using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.Settings;
using Assets.Model.View;
using Assets.Utility.Extension;
using Core.Application;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Presentation.WebApi.Controllers {
    public class AccountController: BaseController {
        #region ctor
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService) {

            _accountService = accountService;
        }
        #endregion

        [HttpPost, Route("signup")]
        public async Task<IActionResult> Signup([FromBody]SignupBindingModel collection) {
            Log.Debug($"A User is trying to register with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username)) {
                return BadRequest(message: "Please define your Email or CellPhone number.");
            }
            collection.Username = collection.Username.Trim();

            if(collection.Username.IsPhoneNumber()) {
                collection.Phone = collection.Username;
            }
            else if(new EmailAddressAttribute().IsValid(collection.Username)) {
                collection.Email = collection.Username;
            }
            else {
                return BadRequest(message: "Please define a correct Email or CellPhone number.");
            }

            if(string.IsNullOrEmpty(collection.Password) && string.IsNullOrEmpty(collection.ConfirmPassword)) {
                return BadRequest(message: "Please define a Password.");
            }

            if(!collection.Password.Equals(collection.ConfirmPassword)) {
                return BadRequest(message: "Defined Password is not match with repeated one.");
            }

            if(string.IsNullOrEmpty(collection.DeviceId)
                || string.IsNullOrEmpty(collection.DeviceName)
                || string.IsNullOrEmpty(collection.IMEI)
                || string.IsNullOrEmpty(collection.OS)) {
                return BadRequest(message: "You're trying to register in the wrong way! buddy.");
            }

            var result = await _accountService.Signup(collection);
            switch(result.Status) {
                case HttpStatusCode.OK:
                    return Ok(data: result.Data);
                case HttpStatusCode.BadRequest:
                    return BadRequest(message: result.Message);
                case HttpStatusCode.InternalServerError:
                default:
                    return InternalServerError(message: result.Message);
            }
        }

        [HttpPost, Route("signin")]
        public async Task<IActionResult> Signin([FromBody]SigninBindingModel collection) {
            Log.Debug($"A User is trying to signing in with this data: {JsonConvert.SerializeObject(collection)}");

            if(string.IsNullOrEmpty(collection.Username) || string.IsNullOrEmpty(collection.Password)) {
                return BadRequest(message: "Please define your Username and Password.");
            }

            try {
                var result = await _accountService.Signin(collection);

                switch(result.Status) {
                    case HttpStatusCode.OK:
                        return Ok(data: result.Data);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(message: result.Message);
                    case HttpStatusCode.InternalServerError:
                    default:
                        return InternalServerError(message: result.Message);
                }
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