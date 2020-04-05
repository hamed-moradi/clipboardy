using Assets.Model.Common;
using Assets.Utility.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApi.Controllers {
    [Authorize]//[Route("[controller]")]
    public class HomeController: Controller {
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

        [HttpGet, Route("index")]
        public IActionResult Index() {
            return View();
        }
    }
}