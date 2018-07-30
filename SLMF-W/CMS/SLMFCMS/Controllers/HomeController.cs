using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FailError()
        {
            return View();
        }

        public ActionResult Error400()
        {
            return View();
        }

        public ActionResult Error401()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}