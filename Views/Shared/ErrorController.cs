using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreEmptyProject1.Views.Shared
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry , the resource you requested could not be found";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    _logger.LogError($"\n 404 Error occured. \n Path = {statusCodeResult.OriginalPath} \n QS: {statusCodeResult.OriginalQueryString}");
                    break;
            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //ViewBag.ExceptionMeessage = exceptionDetails.Error.Message;
            //ViewBag.ExceptionPath = exceptionDetails.Path;
            //ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

            _logger.LogError($"\n hello from logger \n Error Msg: {exceptionDetails.Error.Message} \n ErrorPAth is:{exceptionDetails.Path} \n StackTrace is:{exceptionDetails.Error.StackTrace}");
            return View("Error");
        }
    }
}
