using ClientManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientManager.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer", "Store Admin")]
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";

            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer", "Store Admin")]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer", "Store Admin")]
        public ActionResult InternalServerError()
        {
            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer", "Store Admin")]
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}