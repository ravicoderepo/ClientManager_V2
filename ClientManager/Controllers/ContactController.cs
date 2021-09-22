using DBOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class ContactController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Contact
        public ActionResult Index()
        {
            var contacts = db.Contacts;
            return View(contacts.ToList());
        }
    }
}