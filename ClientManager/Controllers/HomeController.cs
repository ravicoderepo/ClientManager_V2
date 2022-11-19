using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using Microsoft.Ajax.Utilities;

namespace ClientManager.Controllers
{
    [CustomAuthenticationFilter]
    public class HomeController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize("Super Admin", "Super User")]
        public ActionResult AdminDashboard()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            string[] superroles = { "Super Admin", "Super User" };
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            var source = (currentUser.UserRoles.Any(wh => superroles.Contains(wh.RoleName))) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);
            List<SelectListItem> selesPersonList = new List<SelectListItem>();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin" || wh.RoleName.ToLower() == "super user"))
            {
                //string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => superroles.Contains(rl.Role.RoleName)).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }
            ViewBag.SalesPerson = selesPersonList;

            List<GetMonthlySalesReport_Result> list = this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard dashboard = new Dashboard();

            //Dashboard dashboard1 = model;

            var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            dashboard.Closed = source1.Where(wh => wh.Status == 6).Count<SaleActivity>();
            dashboard.InDiscussion = source1.Where(wh => wh.Status == 2).Count<SaleActivity>();
            dashboard.InitialCall = source1.Where(wh => wh.Status == 1).Count<SaleActivity>();
            dashboard.PendingfromCustomer = source1.Where(wh => wh.Status == 3).Count<SaleActivity>();
            dashboard.POReceivedWIP = source1.Where(wh => wh.Status == 5).Count<SaleActivity>();
            dashboard.TotalOrders = 0;
            dashboard.TotalSales = source1.Count();
            dashboard.TotalCalls = source1.Where(wh => wh.Status != 4 && wh.Status != 6).Count();
            int icancelledRate;
            if (source1.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                icancelledRate = 0;
            else
                icancelledRate = source1.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();

            dashboard.CancelledRate = icancelledRate;
            dashboard.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View(dashboard);
        }

        [CustomAuthorize("Super Admin", "Super User", "Store Admin", "Accounts Manager")]
        public ActionResult FinanceDashboard()
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            var expenceTracker = db.ExpenseTrackers.Include(p => p.User).Include(p => p.User1);

            //Current Month and Year
            var MonthlyTotalPettyCashAmount = db.PettyCashes.Where(wh => wh.AmountRecivedDate.Month == DateTime.Now.Month && wh.AmountRecivedDate.Year == DateTime.Now.Year).ToList();
            var MonthlyTotalApprovedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Verified").ToList();
            var MonthlyTotalUnApprovedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Pending").ToList();
            var MonthlyTotalUnVerifiedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Approved").ToList();
            decimal? MonthlyTotalPettyCash = (MonthlyTotalPettyCashAmount != null && MonthlyTotalPettyCashAmount.Count > 0) ? MonthlyTotalPettyCashAmount.Sum(S => S.AmountReceived) : 0;
            decimal? MonthlyTotalApprovedExpence = (MonthlyTotalApprovedExpenceAmount != null && MonthlyTotalApprovedExpenceAmount.Count > 0) ? MonthlyTotalApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? MonthlyTotalUnApprovedExpence = (MonthlyTotalUnApprovedExpenceAmount != null && MonthlyTotalUnApprovedExpenceAmount.Count > 0) ? MonthlyTotalUnApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? MonthlyTotalUnVerifiedExpence = (MonthlyTotalUnVerifiedExpenceAmount != null && MonthlyTotalUnVerifiedExpenceAmount.Count > 0) ? MonthlyTotalUnVerifiedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;

            Dashboard dashboard = new Dashboard();
            dashboard.MonthlyTotalExpenses = (MonthlyTotalApprovedExpence.Value + MonthlyTotalUnApprovedExpence.Value + MonthlyTotalUnVerifiedExpence.Value);
            dashboard.MonthlyTotalPettyCash = MonthlyTotalPettyCash.Value;
            dashboard.MonthlyUnApprovedExpenses = MonthlyTotalUnApprovedExpence.Value;
            dashboard.MonthlyVerifiedExpenses = MonthlyTotalApprovedExpence.Value;
            dashboard.MonthlyUnVerifiedExpenses = MonthlyTotalUnVerifiedExpence.Value;
            dashboard.MonthlyPendingPettyCash = (MonthlyTotalUnApprovedExpence.Value + MonthlyTotalUnVerifiedExpence.Value);
            dashboard.MonthlyAvailablePettyCash = (MonthlyTotalPettyCash.Value - (MonthlyTotalUnApprovedExpence.Value + MonthlyTotalUnVerifiedExpence.Value));
            dashboard.CurrentMonthAndYear = Utility.ConstantData.ToShortMonthName(DateTime.Now) + "/" + DateTime.Now.Year;

            //Total
            //Current Month and Year
            var TotalPettyCashAmount = db.PettyCashes.ToList();
            var TotalApprovedExpenceAmount = expenceTracker.Where(wh => wh.Status == "Verified").ToList();
            var TotalUnApprovedExpenceAmount = expenceTracker.Where(wh => wh.Status == "Pending").ToList();
            var TotalUnVerifiedExpenceAmount = expenceTracker.Where(wh => wh.Status == "Approved").ToList();
            decimal? TotalPettyCash = (TotalPettyCashAmount != null && TotalPettyCashAmount.Count > 0) ? TotalPettyCashAmount.Sum(S => S.AmountReceived) : 0;
            decimal? TotalApprovedExpence = (TotalApprovedExpenceAmount != null && TotalApprovedExpenceAmount.Count > 0) ? TotalApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? TotalUnApprovedExpence = (TotalUnApprovedExpenceAmount != null && TotalUnApprovedExpenceAmount.Count > 0) ? TotalUnApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? TotalUnVerifiedExpence = (TotalUnVerifiedExpenceAmount != null && TotalUnVerifiedExpenceAmount.Count > 0) ? TotalUnVerifiedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;

            dashboard.TotalPettyCash = TotalPettyCash.Value;
            dashboard.UnApprovedExpenses = TotalUnApprovedExpence.Value;
            dashboard.VerifiedExpenses = TotalApprovedExpence.Value;
            dashboard.UnVerifiedExpenses = TotalUnVerifiedExpence.Value;
            dashboard.PendingPettyCash = (TotalUnApprovedExpence.Value + TotalUnVerifiedExpence.Value);
            dashboard.AvailablePettyCash = TotalUnApprovedExpence.Value + TotalApprovedExpence.Value + TotalUnVerifiedExpence.Value;
            dashboard.CurrentMonthAndYear = Utility.ConstantData.ToShortMonthName(DateTime.Now) + "/" + DateTime.Now.Year;

            return (ActionResult)this.View(dashboard);
        }

        //[CustomAuthorize(new string[] { "Super User","Super Admin", "Sales Manager", "Sales Engineer","Store Admin", "Accounts Manager" })]
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

            model.TotalOrders = 0;
            //Dashboard dashboard1 = model;


            var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            model.Closed = source1.Where(wh => wh.Status == 6).Count<SaleActivity>();
            model.InDiscussion = source1.Where(wh => wh.Status == 2).Count<SaleActivity>();
            model.InitialCall = source1.Where(wh => wh.Status == 1).Count<SaleActivity>();
            model.PendingfromCustomer = source1.Where(wh => wh.Status == 3).Count<SaleActivity>();
            model.POReceivedWIP = source1.Where(wh => wh.Status == 5).Count<SaleActivity>();
            model.TotalCalls = source1.Where(wh => wh.Status != 4 && wh.Status != 6).Count();
            model.TotalSales = source1.Count();
            int icancelledRate;
            if (source1.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() <= 0)
                icancelledRate = 0;
            else
                icancelledRate = source1.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 4)).Count<SaleActivity>() * 100 / source.Count<SaleActivity>();

            model.CancelledRate = icancelledRate;
            model.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View((object)model);
        }

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager" })]
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
            //model.Closed = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 6)).Count<SaleActivity>();
            //model.InDiscussion = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 2)).Count<SaleActivity>();
            //model.InitialCall = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 1)).Count<SaleActivity>();
            //model.PendingfromCustomer = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 3)).Count<SaleActivity>();
            //model.POReceivedWIP = source.Where<SaleActivity>((Expression<Func<SaleActivity, bool>>)(wh => wh.Status == 5)).Count<SaleActivity>();
            var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            model.Closed = source1.Where(wh => wh.Status == 6).Count<SaleActivity>();
            model.InDiscussion = source1.Where(wh => wh.Status == 2).Count<SaleActivity>();
            model.InitialCall = source1.Where(wh => wh.Status == 1).Count<SaleActivity>();
            model.PendingfromCustomer = source1.Where(wh => wh.Status == 3).Count<SaleActivity>();
            model.POReceivedWIP = source1.Where(wh => wh.Status == 5).Count<SaleActivity>();
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
        public ActionResult NotAuthorized()
        {
            return View();
        }

        public ActionResult GetUserPerformanceReport(int userId)
        {
            var result = from sale in this.db.SaleActivities
                         join usr in this.db.Users on sale.CreatedBy equals usr.Id
                         group sale by new { sale.InvoiceAmount, sale.CreatedBy, usr.FullName, usr.SaleTarget } into g
                         where g.Key.CreatedBy == userId
                         select new {                            
                             Name = g.Key.FullName,
                             SaleTarget = g.Key.SaleTarget,
                             Amount = g.Sum(s => s.InvoiceAmount)
                         };

            var finalResult = new { target = result.Select(sel=> sel.SaleTarget).ToList(), achived = result.Select(sel => sel.Amount).ToList() };
            
            return (ActionResult)this.Json((object)finalResult, JsonRequestBehavior.AllowGet);
        }
    }
}