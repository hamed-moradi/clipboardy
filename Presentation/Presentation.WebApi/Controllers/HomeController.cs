using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Assets.Model.Common;
using Assets.Utility.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Presentation.WebApi.Controllers {
    public class HomeController: BaseController {
        #region ctor
        private readonly AppSetting _appSetting;
        private readonly CompressionHandler _compressionHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(
            AppSetting appSetting,
            CompressionHandler compressionHandler,
            IWebHostEnvironment webHostEnvironment) {

            _appSetting = appSetting;
            _compressionHandler = compressionHandler;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        [HttpGet, Route("getalltokens")]
        public async Task<IActionResult> AllTokens() {
            if(_webHostEnvironment.IsDevelopment()) {
                var result = new List<dynamic>();

                return Ok(data: result);
            }
            return BadRequest();
        }

        [HttpGet, Route("getserverpattern")]
        public IActionResult GetServerPatternAsync() {
            var srvpat = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appSetting.SignalR.ServerPattern));
            return Ok(data: srvpat);
        }

        [HttpGet, Route("index")]
        public IActionResult Index() {
            return View();
        }
    }
}