using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class SharedController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Shared
        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer", "Store Admin","Accounts Manager")]
        public ActionResult GetNotifications(int userId)
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            var saleNotifications = db.SaleActivities.AsNoTracking().AsEnumerable().Where(wh => Convert.ToDateTime(wh.AnticipatedClosingDate).ToUniversalTime().Date.ToShortDateString() == DateTime.UtcNow.Date.ToShortDateString() && wh.CreatedBy == currentUser.Id).Select(sel => new SaleNotification
            {
                Id = sel.Id,
                //Status = sel.SalesStatu.Description,
                ClientName = sel.ClientName,
                ProductName = sel.ProductName
            }).ToList();

            return PartialView(saleNotifications);

        }
    }
}