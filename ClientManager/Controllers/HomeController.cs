using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
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

        [CustomAuthorize("Super Admin", "Super User", "Sales Manager")]
        public ActionResult AdminDashboard()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            string[] superroles = { "Super Admin", "Super User" };
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            var source = (currentUser.UserRoles.Any(wh => superroles.Contains(wh.RoleName))) ? db.SaleActivities : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);
            List<SelectListItem> selesPersonList = new List<SelectListItem>();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin" || wh.RoleName.ToLower() == "super user"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && rl.User1.IsActive == true).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }
            else if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && rl.User1.IsActive == true && currentUser.ReportingToMe.Contains(rl.UserId) || rl.UserId == currentUser.Id).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }

            ViewBag.SalesPerson = selesPersonList;
            selesPersonList.Insert(0, (new SelectListItem { Text = userDetails.FullName, Value = userDetails.Id.ToString() }));
            //TODO
            List<GetMonthlySalesReport_Result> list = new List<GetMonthlySalesReport_Result>(); // this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();
            Dashboard dashboard = new Dashboard();

            //Dashboard dashboard1 = model;

            //var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            dashboard.Closed = source.Where(wh => wh.Status == 6).Count<SaleActivity>();
            dashboard.InDiscussion = source.Where(wh => wh.Status == 2).Count<SaleActivity>();
            dashboard.InitialCall = source.Where(wh => wh.Status == 1).Count<SaleActivity>();
            dashboard.PendingfromCustomer = source.Where(wh => wh.Status == 3).Count<SaleActivity>();
            dashboard.POReceivedWIP = source.Where(wh => wh.Status == 5).Count<SaleActivity>();
            dashboard.TotalOrders = 0;
            dashboard.TotalCallsMade = source.Count();
            dashboard.TotalActiveCalls = source.Where(wh => wh.Status != 4 && wh.Status != 6).Count();

            if (source.Where(wh => wh.Status == 4).Count() > 0)
            {
                dashboard.CancelledRate = source.Where(wh => wh.Status == 4).Count();
            }
            else
            {
                dashboard.CancelledRate = 0;
            }

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
            var source = this.db.SaleActivities.Where(wh => wh.CreatedBy == currentUser.Id);
            //TODO
            List<GetMonthlySalesReport_Result> list = new List<GetMonthlySalesReport_Result>(); // this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select(sel => sel.mname.Value).ToArray();
            monthlySalesReport.Calls = list.Select(sel => sel.calls.Value).ToArray();
            monthlySalesReport.Orders = list.Select(sel => sel.orders.Value).ToArray();
            monthlySalesReport.Cancels = list.Select(sel => sel.cancels.Value).ToArray();
            Dashboard model = new Dashboard();

            model.TotalOrders = 0;
            //Dashboard dashboard1 = model;

            //var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            model.Closed = source.Where(wh => wh.Status == 6).Count<SaleActivity>();
            model.InDiscussion = source.Where(wh => wh.Status == 2).Count();
            model.InitialCall = source.Where(wh => wh.Status == 1).Count();
            model.PendingfromCustomer = source.Where(wh => wh.Status == 3).Count();
            model.POReceivedWIP = source.Where(wh => wh.Status == 5).Count();
            model.TotalActiveCalls = source.Where(wh => wh.Status != 4 && wh.Status != 6).Count();
            model.TotalCallsMade = source.Count();
            if (source.Where(wh => wh.Status == 4).Count() > 0)
            {
                model.CancelledRate = source.Where(wh => wh.Status == 4).Count() * 100 / source.Count();
            }
            else
            {
                model.CancelledRate = 0;
            }
            model.MonthlySalesReport = monthlySalesReport;
            return (ActionResult)this.View((object)model);
        }

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager" })]
        public ActionResult ManagerDashboard()
        {
            UserDetails currentUser = (UserDetails)this.Session["UserDetails"];
            IQueryable<SaleActivity> source;
            if (!currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin"))
            {
                if (!currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager"))
                    source = this.db.SaleActivities.Where(wh => wh.CreatedBy == currentUser.Id);
                else
                    source = this.db.SaleActivities.Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy));
            }
            else
                source = this.db.SaleActivities;

           
            Dashboard model = new Dashboard();

            if (source.Where(wh => wh.Status == 4).Count() > 0)
                model.CancelledRate = source.Where(wh => wh.Status == 4).Count();
            else
                model.CancelledRate = 0;
                        
            //var source1 = source.Where(wh => wh.SaleDate.Month == DateTime.Now.Month && wh.SaleDate.Year == DateTime.Now.Year);
            model.Closed = source.Where(wh => wh.Status == 6).Count();
            model.InDiscussion = source.Where(wh => wh.Status == 2).Count();
            model.InitialCall = source.Where(wh => wh.Status == 1).Count();
            model.PendingfromCustomer = source.Where(wh => wh.Status == 3).Count();
            model.POReceivedWIP = source.Where(wh => wh.Status == 5).Count();           
            model.TotalCallsMade = source.Count();
            model.TotalOrders = 0;
            model.TotalActiveCalls = source.Where(wh => wh.Status != 4 && wh.Status != 6).Count();

            //Report
            //TODO
            List<GetMonthlySalesReport_Result> list = new List<GetMonthlySalesReport_Result>(); // this.db.GetMonthlySalesReport("Super Admin", new int?(1), new int?(1)).ToList<GetMonthlySalesReport_Result>();
            MonthlySalesReport monthlySalesReport = new MonthlySalesReport();
            monthlySalesReport.Mname = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.mname.Value)).ToArray<int>();
            monthlySalesReport.Calls = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.calls.Value)).ToArray<int>();
            monthlySalesReport.Orders = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.orders.Value)).ToArray<int>();
            monthlySalesReport.Cancels = list.Select<GetMonthlySalesReport_Result, int>((Func<GetMonthlySalesReport_Result, int>)(sel => sel.cancels.Value)).ToArray<int>();

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
            if(userId == 0)
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                userId = userDetails.Id;
            }
            //var result = from sale in this.db.SaleActivities
            //             join usr in this.db.Users on sale.CreatedBy equals usr.Id
            //             group sale by new { sale.InvoiceAmount, sale.CreatedBy, usr.FullName, usr.SaleTarget } into g
            //             where g.Key.CreatedBy == userId
            //             select new {                            
            //                 Name = g.Key.FullName,
            //                 SaleTarget = g.Key.SaleTarget,
            //                 Amount = g.Sum(s => s.InvoiceAmount)
            //             };

            //var finalResult = new { target = result.Select(sel=> sel.SaleTarget).ToList(), achived = result.Select(sel => sel.Amount).ToList() };
            //TODO
            List<GetEmployeePerformanceReport_Result> EmpPerformanceReport = new List<GetEmployeePerformanceReport_Result>(); // this.db.GetEmployeePerformanceReport(userId).ToList<GetEmployeePerformanceReport_Result>();

            return (ActionResult)this.Json((object)EmpPerformanceReport, JsonRequestBehavior.AllowGet);
        }
    }
}