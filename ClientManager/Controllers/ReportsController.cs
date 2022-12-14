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
    public class ReportsController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Accounts Manager" })]
        public ActionResult ExpenseTrackerReport()
        {
            var expenseReport = db.ExpenseTrackers.Select(sel => new ExpenseTrackerReportData
            {
                Description = sel.Description,
                ExpenseAmount = sel.ExpenseAmount,
                ExpenseCategoryName = sel.ExpenceCategory.CategoryName,
                ExpenseDate = sel.ExpenseDate,
                Id = sel.Id,
                Status = sel.Status
            }).AsEnumerable();

            return View(expenseReport.OrderByDescending(ord => ord.ExpenseDate).ToList());
        }
    }
}