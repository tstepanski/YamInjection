using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IntegrationTestProgram;
using YamInjection.Web;

namespace TestWebSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new YamInjectionServiceLocator(new MyInjectionMap()));
        }
    }
}