using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CSharp2Knockout.ActionResults;
using CSharp2Knockout.Extensions;
using CSharp2Knockout.Services;
using CSharp2Knockout.Services.NRefactory;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CSharp2Knockout.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string csharp, TranslateOptions options)
        {
            _log.Debug("Starting converting class with options: {0}, class to convert:\n{1}", options.ToFormattedJson(), csharp);

            var x = new NRefactoryCodeConvertion();
            var z = x.ToKnockoutVm(csharp, options);

            //var result = csharp.ToKnockout(options);

            _log.Debug("Returning converted class:\n{0}", z.ToFormattedJson());

            return Json(z, DefaultJsonSettings);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var log = LogManager.GetCurrentClassLogger();

            log.Debug("Handling exception on Controller");

            if(filterContext.ExceptionHandled)
            {
                log.ErrorException("Error occured on controller", filterContext.Exception);
                log.Debug("Exception is handled");
                return;
            }

            if(!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                log.Debug("It's not ajax request, passing handling of exception");
                base.OnException(filterContext);
                return;
            }

            var result = new
            {
                success = false,
                errors = new List<string>(),
                message = filterContext.Exception.Message
            };

            log.ErrorException("Error occured on controller, handling it as AjaxResult", filterContext.Exception);
            filterContext.Result = Json(result);
            filterContext.ExceptionHandled = true;
        }
        protected JsonNetResult Json(object data, JsonSerializerSettings settings)
        {
            return new JsonNetResult(data, settings);
        }

        protected JsonSerializerSettings DefaultJsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                    ,
                    NullValueHandling = NullValueHandling.Include
                    ,
                    Converters = new List<JsonConverter>
                                                {
                                                    new StringEnumConverter()
                                                }
                };
            }
        }

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
    }
}