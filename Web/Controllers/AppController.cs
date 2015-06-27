using System.Web.Mvc;

namespace Demo.Web.Controllers
{
    /// <summary>
    /// Dashboard controller to bootstrap the AngularJS app module
    /// </summary>
    public class AppController : Controller
    {

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}