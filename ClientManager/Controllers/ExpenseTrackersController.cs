using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;

namespace ClientManager.Controllers
{
    public class ExpenseTrackersController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: PettyCashes
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin", "Accounts Manager" })]
        public ActionResult List(string dashboardFilter="")
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            var expenceTracker = db.ExpenseTrackers.Include(p => p.User).Include(p => p.User1);
            //var TotalPettyCashAmount = db.PettyCashes.Where(wh => wh.AmountRecivedDate.Month == DateTime.Now.Month && wh.AmountRecivedDate.Year == DateTime.Now.Year).ToList();
            //var TotalApprovedExpenceAmount = db.ExpenseTrackers.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Verified").ToList();
            //var TotalUnApprovedExpenceAmount = db.ExpenseTrackers.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Pending").ToList();
            //var TotalUnVerifiedExpenceAmount = db.ExpenseTrackers.Where(wh => wh.ExpenseDate.Month == DateTime.Now.Month && wh.ExpenseDate.Year == DateTime.Now.Year && wh.Status == "Approved").ToList();
            //decimal? TotalPettyCash = (TotalPettyCashAmount != null && TotalPettyCashAmount.Count > 0) ? TotalPettyCashAmount.Sum(S=> S.AmountReceived) : 0;
            //decimal? TotalApprovedExpence = (TotalApprovedExpenceAmount != null && TotalApprovedExpenceAmount.Count > 0) ? TotalApprovedExpenceAmount.Sum(s=> s.ExpenseAmount) : 0;
            //decimal? TotalUnApprovedExpence = (TotalUnApprovedExpenceAmount!=null && TotalUnApprovedExpenceAmount.Count > 0) ? TotalUnApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            //decimal? TotalUnVerifiedExpence = (TotalUnVerifiedExpenceAmount != null && TotalUnVerifiedExpenceAmount.Count > 0) ? TotalUnVerifiedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;

            //ViewBag.TotalPettyCash = TotalPettyCash;
            //ViewBag.TotalApprovedExpence = TotalApprovedExpence;
            //ViewBag.TotalUnApprovedExpence = TotalUnApprovedExpence;
            //ViewBag.TotalUnVerifiedExpence = TotalUnVerifiedExpence;
            //ViewBag.AvailablePettyCash = (TotalPettyCash - TotalApprovedExpence);
            //ViewBag.CurrentMonthAndYear = Utility.ConstantData.ToMonthName(DateTime.Now) + "/" + DateTime.Now.Year;
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList  = new SelectList(Utility.DefaultList.GetPaymentStatusList(), "Value", "Text", "").ToList();

            //if (userData.UserRoles.Any(a => a.RoleName.ToLower() == "store admin"))
            //{
            //    statusList = new SelectList(Utility.DefaultList.GetPaymentStatusList().Where(w=> w.Text=="Pending").ToList(), "Value", "Text", "").ToList();
            //}
            statusList.Insert(0, (new SelectListItem { Text = "", Value = "" }));
            ViewBag.Status = statusList;
            ViewBag.Years = new SelectList(Utility.DefaultList.GetYearList(), "Value", "Text", "").ToList();
            ViewBag.Months = new SelectList(Utility.DefaultList.GetMonthList(), "Value", "Text", "").ToList();
            List<SelectListItem> expenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", "").ToList();
            expenseCategory.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            ViewBag.ExpenseCategory = expenseCategory;
            ViewBag.DashboardFilter = string.IsNullOrEmpty(dashboardFilter) ? "" : dashboardFilter;
            return View(expenceTracker.ToList());
        }
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin", "Accounts Manager" })]
        public ActionResult ListView(string expenseDateFrom = "", string expenseDateTo = "", string status = "", int month = 0,int year = 0, int expenseCat = 0)
        {
            DateTime dtExpenseDateFrom = new DateTime();
            DateTime dtExpenseDateTo = new DateTime();
            
            month = (month == 0) ? DateTime.Now.Month : month;
            year = (year == 0) ? DateTime.Now.Year : year;

            var expenceTracker = db.ExpenseTrackers.Include(p => p.User).Include(p => p.User1);
            if (!string.IsNullOrEmpty(expenseDateFrom))
            {
                dtExpenseDateFrom = DateTime.Parse(expenseDateFrom);
                expenceTracker = expenceTracker.Where(wh => wh.ExpenseDate >= dtExpenseDateFrom);
            }

            if (!string.IsNullOrEmpty(expenseDateTo))
            {
                dtExpenseDateTo = DateTime.Parse(expenseDateTo);
                expenceTracker = expenceTracker.Where(wh => wh.ExpenseDate <= dtExpenseDateTo);
            }

            if (!string.IsNullOrEmpty(status))
                expenceTracker = expenceTracker.Where(wh => wh.Status == status);

            if (year > 0)
                expenceTracker = expenceTracker.Where(wh => wh.ExpenseDate.Year == year);          

            if (month > 0)
                expenceTracker = expenceTracker.Where(wh => wh.ExpenseDate.Month == month);

            if (expenseCat > 0)
                expenceTracker = expenceTracker.Where(wh => wh.ExpenseCategoryId == expenseCat);

            var TotalPettyCashAmount = db.PettyCashes.Where(wh => wh.AmountRecivedDate.Month == month && wh.AmountRecivedDate.Year == year).ToList();
            var TotalApprovedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == month && wh.ExpenseDate.Year == year && wh.Status == "Verified").ToList();
            var TotalUnApprovedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == month && wh.ExpenseDate.Year == year && wh.Status == "Pending").ToList();
            var TotalUnVerifiedExpenceAmount = expenceTracker.Where(wh => wh.ExpenseDate.Month == month && wh.ExpenseDate.Year == year && wh.Status == "Approved").ToList();
            decimal? TotalPettyCash = (TotalPettyCashAmount != null && TotalPettyCashAmount.Count > 0) ? TotalPettyCashAmount.Sum(S => S.AmountReceived) : 0;
            decimal? TotalApprovedExpence = (TotalApprovedExpenceAmount != null && TotalApprovedExpenceAmount.Count > 0) ? TotalApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? TotalUnApprovedExpence = (TotalUnApprovedExpenceAmount != null && TotalUnApprovedExpenceAmount.Count > 0) ? TotalUnApprovedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;
            decimal? TotalUnVerifiedExpence = (TotalUnVerifiedExpenceAmount != null && TotalUnVerifiedExpenceAmount.Count > 0) ? TotalUnVerifiedExpenceAmount.Sum(s => s.ExpenseAmount) : 0;

            ViewBag.TotalPettyCash = TotalPettyCash.Value.ToString("#,##0.00");
            ViewBag.TotalApprovedExpence = TotalApprovedExpence.Value.ToString("#,##0.00");
            ViewBag.TotalUnApprovedExpence = TotalUnApprovedExpence.Value.ToString("#,##0.00");
            ViewBag.TotalUnVerifiedExpence = TotalUnVerifiedExpence.Value.ToString("#,##0.00");
            ViewBag.PendingPettyCash = (TotalPettyCash.Value - TotalApprovedExpence.Value).ToString("#,##0.00");
            ViewBag.AvailablePettyCash = (TotalPettyCash.Value - (TotalUnApprovedExpence.Value + TotalUnVerifiedExpence.Value)).ToString("#,##0.00");
            ViewBag.CurrentMonthAndYear = month + "/" + year;

            //List<SelectListItem> statusList = new SelectList(Utility.DefaultList.GetPaymentStatusList(), "Value", "Text", "").ToList();
            //statusList.Insert(0, (new SelectListItem { Text = "", Value = "" }));
            //ViewBag.Status = statusList;
            //ViewBag.Years = new SelectList(Utility.DefaultList.GetYearList(), "Value", "Text", "").ToList();
            //ViewBag.Months = new SelectList(Utility.DefaultList.GetMonthList(), "Value", "Text", "").ToList();
            //List<SelectListItem> expenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", "").ToList();
            //expenseCategory.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            //ViewBag.ExpenseCategory = expenseCategory;
            return PartialView(expenceTracker.ToList().OrderByDescending(ord => ord.ExpenseDate).ToList());
        }

        // GET: PettyCashes/Create
        [CustomAuthorize(new string[] { "Super Admin", "Store Admin" })]
        public ActionResult Create()
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList = new SelectList(Utility.DefaultList.GetPaymentStatusList(), "Value", "Text", "").ToList();

            if (userData.UserRoles.Any(a => a.RoleName.ToLower() == "store admin"))
            {
                statusList = new SelectList(Utility.DefaultList.GetPaymentStatusList().Where(w => w.Text == "Pending").ToList(), "Value", "Text", "").ToList();
            }
            statusList.Insert(0, new SelectListItem()
            {
                Text = "",
                Value = ""
            });
            ViewBag.Status = new SelectList(statusList, "Value", "Text", "Pending").ToList<SelectListItem>();
            ViewBag.ExpenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", 1);
            return View();
        }


        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Store Admin" })]
        public ActionResult Create(Models.ExpenceTrackerData expenceTrackerData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse jsonReponse = (JsonReponse)null;
            var recId = 0;
            JsonReponse data;
            try
            {
                int num = 0;
                if (expenceTrackerData.Status == null||expenceTrackerData.ExpenseDate == null || expenceTrackerData.ExpenseAmount <= 0 || string.IsNullOrEmpty(expenceTrackerData.Description))
                {
                    jsonReponse = new JsonReponse()
                    {
                        message = "Enter all required fields.",
                        status = "Failed",
                        redirectURL = ""
                    };
                }
                else
                {
                    var expnseTracker = new DBOperation.ExpenseTracker()
                    {
                        ExpenseDate = DateTime.ParseExact(expenceTrackerData.ExpenseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ExpenseCategoryId = expenceTrackerData.ExpenseCategoryId,
                        ExpenseAmount = expenceTrackerData.ExpenseAmount,
                        Description = expenceTrackerData.Description,
                        Status = expenceTrackerData.Status,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    };
                    this.db.ExpenseTrackers.Add(expnseTracker);
                    num = this.db.SaveChanges();
                    recId = expnseTracker.Id;

                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Expence entry created successfully!",
                        status = "Success",
                        redirectURL = "/ExpenseTrackers/Edit/" + recId
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "Expence entry not completed, try again after sometime.",
                        status = "Failed",
                        redirectURL = ""
                    };
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = ""
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        // GET: ExpenceCategories/Edit/5
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin", "Accounts Manager" })]
        public ActionResult Edit(int? id)
        {
            UserDetails currentUser = (UserDetails)this.Session["UserDetails"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.ExpenseTracker expenceTracker = db.ExpenseTrackers.Find(id);
            if (expenceTracker == null)
            {
                return HttpNotFound();
            }

            var statusList = Utility.DefaultList.GetPaymentStatusList();

            if (currentUser.UserRoles.Any(a => a.RoleName == "Accounts Manager"))
            {
                statusList = statusList.Where(wh => wh.Value == "Verified").ToList();
            }
            else if (currentUser.UserRoles.Any(a => a.RoleName == "Super User"))
            {
                statusList = statusList.Where(wh => wh.Value == "Approved").ToList();
            }
            else
            {
                statusList = statusList.Where(wh => wh.Value == "Pending").ToList();
            }
            statusList.Insert(0, new SelectListItem()
            {
                Text = "",
                Value = ""
            });

            ViewBag.Status = new SelectList(statusList, "Value", "Text", expenceTracker.Status).ToList<SelectListItem>();
            ViewBag.ExpenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", expenceTracker.ExpenseCategoryId);
            return View(expenceTracker);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin", "Accounts Manager" })]
        public ActionResult Edit(Models.ExpenceTrackerData expenceTrackerData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                DBOperation.ExpenseTracker entity = this.db.ExpenseTrackers.FirstOrDefault(wh => wh.Id == expenceTrackerData.Id);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                if (expenceTrackerData.ExpenseDate == null || expenceTrackerData.ExpenseAmount <= 0 || string.IsNullOrEmpty(expenceTrackerData.Description))
                {
                    data = new JsonReponse()
                    {
                        message = "Enter all required fields.",
                        status = "Failed",
                        redirectURL = ""
                    };
                }
                else
                {
                    this.db.Entry<DBOperation.ExpenseTracker>(entity).State = EntityState.Modified;

                    entity.ExpenseAmount = expenceTrackerData.ExpenseAmount;
                    entity.ExpenseDate = DateTime.ParseExact(expenceTrackerData.ExpenseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    entity.ExpenseCategoryId = expenceTrackerData.ExpenseCategoryId;
                    entity.Description = expenceTrackerData.Description;
                    entity.Status = expenceTrackerData.Status;
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = "Expense updated successfully!",
                            status = "Success",
                            redirectURL = "/ExpenseTrackers/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = "Expense update not completed, try again after sometime.",
                            status = "Failed",
                            redirectURL = ""
                        };
                }
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = ""
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [CustomAuthorize(new string[] { "Super User" })]
        public ActionResult ExpenceEntryApproval(int id, string status)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (id <= 0)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ExpenseTracker entity = this.db.ExpenseTrackers.Find(id);

                if (entity == null)
                    return (ActionResult)this.HttpNotFound();

                entity.Status = status;
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<ExpenseTracker>(entity).State = EntityState.Modified;
                this.db.SaveChanges();

                data = new JsonReponse()
                {
                    message = "Expence Entry Approved.",
                    status = "Success",
                    redirectURL = "/ExpenseTrackers/List?" + DateTime.Now.Ticks.ToString()
                };
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = ""
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Accounts Manager" })]
        public ActionResult ExpenceEntryVerify(int id, string status)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];

            JsonReponse data;
            try
            {
                if (id <= 0)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ExpenseTracker entity = this.db.ExpenseTrackers.Find(id);

                if (entity == null)
                    return (ActionResult)this.HttpNotFound();

                entity.Status = status;
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<ExpenseTracker>(entity).State = EntityState.Modified;
                this.db.SaveChanges();

                data = new JsonReponse()
                {
                    message = "Expence Entry Verified.",
                    status = "Success",
                    redirectURL = "/ExpenseTrackers/List?" + DateTime.Now.Ticks.ToString()
                };
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = ""
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Store Admin" })]
        public ActionResult Delete(int id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];

            JsonReponse data;
            try
            {
                if (id <= 0)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                ExpenseTracker entity = this.db.ExpenseTrackers.Find(id);

                if (entity == null)
                    return (ActionResult)this.HttpNotFound();

                this.db.ExpenseTrackers.Remove(entity);
                this.db.SaveChanges();

                data = new JsonReponse()
                {
                    message = "Expence Entry Deleted.",
                    status = "Success",
                    redirectURL = "/ExpenceTrackers/List?" + DateTime.Now.Ticks.ToString()
                };
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = ""
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
