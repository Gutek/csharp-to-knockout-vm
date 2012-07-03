using System.Web.Mvc;
using CSharp2Knockout.Extensions;

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
        public ActionResult Index(string csharp, bool? onlyPublic, bool? publicGetter, bool? includeEnums, bool? includeDataIf)
        {
            var result = csharp.ToKnockout(
                onlyPublic.OrDefault(true)
                , publicGetter.OrDefault(true)
                , includeEnums.OrDefault(false)
                , includeDataIf.OrDefault(false)
            );

            return Json(result);
        }
    }
}