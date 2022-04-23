using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Index() => (ActionResult)this.View();
    }
}
