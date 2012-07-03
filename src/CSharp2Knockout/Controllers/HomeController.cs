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
        public ActionResult Index(string csharp)
        {
            var result = csharp.ToKnockout();

            return Json(result);
        }
    }
}