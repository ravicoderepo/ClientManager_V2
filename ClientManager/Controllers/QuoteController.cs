using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class QuoteController : Controller
    {
        public ActionResult Index() => (ActionResult)this.View();
    }
}
