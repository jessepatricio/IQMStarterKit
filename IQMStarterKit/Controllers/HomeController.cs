using System.Web.Mvc;

namespace IQMStarterKit.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            Session["email"] = null;

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            Session["email"] = null;

            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            Session["email"] = null;

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Start()
        {
            Session["email"] = null;

            return RedirectToAction("Index", "Module");
        }
    }
}