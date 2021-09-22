using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;

namespace ClientManager.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult AdminDashboard()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult MyDashboard()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            var sales = (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin")) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            var salesReport = db.GetMonthlySalesReport().ToList();
            //ViewBag.salesReportJSON = new JavaScriptSerializer().Serialize(salesReport).ToString();

            MonthlySalesReport objMonthlySalesReport = new MonthlySalesReport();

            objMonthlySalesReport.Mname = salesReport.Select(sel => sel.mname.Value).ToArray();
            objMonthlySalesReport.Calls = salesReport.Select(sel => sel.calls.Value).ToArray();
            objMonthlySalesReport.Orders = salesReport.Select(sel => sel.orders.Value).ToArray();
            objMonthlySalesReport.Cancels = salesReport.Select(sel => sel.cancels.Value).ToArray();

            //= salesReport.Select(sel => new MonthlySalesReport() { Calls = sel.calls, Cancels = sel.cancels, Mname = sel.mname, Orders = sel.orders });

            return View(new Dashboard { TotalSales = sales.Count(), TotalOrders = 0, CancelledRate = sales.Where(wh => wh.Status == 4).Count() > 0 ? (sales.Where(wh => wh.Status == 4).Count() * 100/ sales.Count()) :0, TotalCalls = sales.Sum(su => su.NoOfFollowUps).HasValue ? sales.Sum(su=> su.NoOfFollowUps).Value : 0, Closed= sales.Where(wh => wh.Status == 6).Count(), InDiscussion= sales.Where(wh => wh.Status == 2).Count(), InitialCall= sales.Where(wh => wh.Status == 1).Count(), PendingfromCustomer= sales.Where(wh => wh.Status == 3).Count(), POReceivedWIP= sales.Where(wh => wh.Status == 5).Count(), MonthlySalesReport = objMonthlySalesReport }); 
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult ManagerDashboard()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult About()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult Contact()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";

            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult InternalServerError()
        {
            return View();
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}