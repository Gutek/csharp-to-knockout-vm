using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;

namespace CSharp2Knockout
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            _log.ErrorException("Unhandled error", ex);
        }

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
    }
}