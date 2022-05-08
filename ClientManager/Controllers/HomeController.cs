using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        [CustomAuthorize("Super Admin", "Super User")]
        public ActionResult AdminDashboard()
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            IQueryable<SaleActivity> source = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu));
            List<GetMonthlySalesReport_Result> list = this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard dashboard = new Dashboard();
            dashboard.TotalSales = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            dashboard.TotalOrders = 0;
            //Dashboard dashboard1 = model;
            int num1;
            if (source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                num1 = 0;
            else
                num1 = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();
            dashboard.CancelledRate = num1;
            //Dashboard dashboard2 = model;
            int num2;
            if (!source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps)).HasValue)
                num2 = 0;
            else
                num2 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps)).Value;
            int? nullable = new int?(num2);
            dashboard.TotalCalls = nullable;
            dashboard.Closed = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            dashboard.InDiscussion = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 2)).Count<SaleActivity>();
            dashboard.InitialCall = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 1)).Count<SaleActivity>();
            dashboard.PendingfromCustomer = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 3)).Count<SaleActivity>();
            dashboard.POReceivedWIP = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 5)).Count<SaleActivity>();
            dashboard.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View(dashboard);
        }

        [CustomAuthorize("Super Admin", "Super User")]
        public ActionResult FinanceDashboard()
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            IQueryable<SaleActivity> source = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu));
            List<GetMonthlySalesReport_Result> list = this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard dashboard = new Dashboard();
            dashboard.TotalSales = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            dashboard.TotalOrders = 0;
            //Dashboard dashboard1 = model;
            int num1;
            if (source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                num1 = 0;
            else
                num1 = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();
            dashboard.CancelledRate = num1;
            //Dashboard dashboard2 = model;
            int num2;
            if (!source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps)).HasValue)
                num2 = 0;
            else
                num2 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps)).Value;
            int? nullable = new int?(num2);
            dashboard.TotalCalls = nullable;
            dashboard.Closed = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            dashboard.InDiscussion = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 2)).Count<SaleActivity>();
            dashboard.InitialCall = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 1)).Count<SaleActivity>();
            dashboard.PendingfromCustomer = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 3)).Count<SaleActivity>();
            dashboard.POReceivedWIP = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 5)).Count<SaleActivity>();
            dashboard.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View(dashboard);
        }

        [CustomAuthorize(new string[] { "Super User","Super Admin", "Sales Manager", "Sales Engineer","Store Admin","Accounts Manager" })]
        public ActionResult MyDashboard()
        {
            UserDetails currentUser = (UserDetails)this.Session["UserDetails"];
            IQueryable<SaleActivity> source = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu)).Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.CreatedBy == currentUser.Id));
            List<GetMonthlySalesReport_Result> list = this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard model = new Dashboard();
            model.TotalSales = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            model.TotalOrders = 0;
            Dashboard dashboard1 = model;
            int num1;
            if (source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                num1 = 0;
            else
                num1 = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();
            dashboard1.CancelledRate = num1;
            Dashboard dashboard2 = model;
            int? nullable1 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps));
            int num2;
            if (!nullable1.HasValue)
            {
                num2 = 0;
            }
            else
            {
                nullable1 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps));
                num2 = nullable1.Value;
            }
            int? nullable2 = new int?(num2);
            dashboard2.TotalCalls = nullable2;
            model.Closed = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            model.InDiscussion = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 2)).Count<SaleActivity>();
            model.InitialCall = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 1)).Count<SaleActivity>();
            model.PendingfromCustomer = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 3)).Count<SaleActivity>();
            model.POReceivedWIP = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 5)).Count<SaleActivity>();
            model.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View((object)model);
        }

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager"})]
        public ActionResult ManagerDashboard()
        {
            UserDetails currentUser = (UserDetails)this.Session["UserDetails"];
            IQueryable<SaleActivity> queryable;
            if (!currentUser.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>)(wh => wh.RoleName.ToLower() == "super admin")))
            {
                if (!currentUser.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>)(wh => wh.RoleName.ToLower() == "sales manager")))
                    queryable = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu)).Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.CreatedBy == currentUser.Id));
                else
                    queryable = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu)).Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)));
            }
            else
                queryable = this.db.SaleActivities.Include<SaleActivity, SalesStatu>((Expression<Func<SaleActivity, SalesStatu>>)(s => s.SalesStatu));
            IQueryable<SaleActivity> source = queryable;
            List<GetMonthlySalesReport_Result> list = this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard model = new Dashboard();
            model.TotalSales = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            model.TotalOrders = 0;
            Dashboard dashboard1 = model;
            int num1;
            if (source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                num1 = 0;
            else
                num1 = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();
            dashboard1.CancelledRate = num1;
            Dashboard dashboard2 = model;
            int? nullable1 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps));
            int num2;
            if (!nullable1.HasValue)
            {
                num2 = 0;
            }
            else
            {
                nullable1 = source.Sum<SaleActivity>((Expression<Func<SaleActivity, int?>>)(su => su.NoOfFollowUps));
                num2 = nullable1.Value;
            }
            int? nullable2 = new int?(num2);
            dashboard2.TotalCalls = nullable2;
            model.Closed = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            model.InDiscussion = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 2)).Count<SaleActivity>();
            model.InitialCall = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 1)).Count<SaleActivity>();
            model.PendingfromCustomer = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 3)).Count<SaleActivity>();
            model.POReceivedWIP = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 5)).Count<SaleActivity>();
            model.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View((object)model);
        }

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager", "Sales Engineer", "Store Admin" })]
        public ActionResult About() => (ActionResult)this.View();

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager", "Sales Engineer", "Store Admin" })]
        public ActionResult Contact() => (ActionResult)this.View();

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer", "Store Admin")]
        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";

            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer","Store Admin")]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer","Store Admin")]
        public ActionResult InternalServerError()
        {
            return View();
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer","Store Admin")]
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}