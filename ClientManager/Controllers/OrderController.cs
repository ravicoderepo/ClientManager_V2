using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index() => (ActionResult)this.View();
    }
}