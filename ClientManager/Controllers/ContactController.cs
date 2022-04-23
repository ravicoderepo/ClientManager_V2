using DBOperation;
using System.Linq;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class ContactController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        public ActionResult Index() => (ActionResult)this.View((object)this.db.Contacts.ToList<Contact>());
    }
}