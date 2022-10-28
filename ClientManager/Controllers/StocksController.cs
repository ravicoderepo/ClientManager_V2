using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class StocksController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public int GetAvailableQuantity(int itemId = 0)
        {
            return Utility.CommonFunctions.GetAvailableQuantity(itemId);
        }
    }
}