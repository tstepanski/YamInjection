using System.Web.Mvc;
using IntegrationTestProgram;

namespace TestWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISomeService _someService;

        public HomeController(ISomeService someService)
        {
            _someService = someService;
        }

        public ActionResult Index()
        {
            var someString = _someService.GetSomeString();

            return View("Index", (object) someString);
        }
    }
}