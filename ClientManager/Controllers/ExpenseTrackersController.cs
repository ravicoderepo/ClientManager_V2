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
        [CustomAuthorize(new string[] { "Admin", "Finance" })]
        public ActionResult List()
        {
            var expenceTracker = db.ExpenseTrackers.Include(p => p.User).Include(p => p.User1);
            return View(expenceTracker.ToList());
        }



        // GET: PettyCashes/Create
        [CustomAuthorize(new string[] { "Admin", "Finance" })]
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Utility.DefaultList.GetPaymentStatusList(), "Value", "Text", "Pending").ToList<SelectListItem>();
            ViewBag.ExpenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", 1);
            return View();
        }


        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin", "Finance" })]
        public ActionResult Create(Models.ExpenceTrackerData expenceTrackerData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;
                if (expenceTrackerData.ExpenseDate == null || expenceTrackerData.ExpenseAmount <= 0 || string.IsNullOrEmpty(expenceTrackerData.Description))
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
                    this.db.ExpenseTrackers.Add(new DBOperation.ExpenseTracker()
                    {
                        ExpenseDate = DateTime.ParseExact(expenceTrackerData.ExpenseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        ExpenseCategoryId = expenceTrackerData.ExpenseCategoryId,
                        ExpenseAmount = expenceTrackerData.ExpenseAmount,
                        Description = expenceTrackerData.Description,
                        Status = expenceTrackerData.Status,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    }); ;
                    num = this.db.SaveChanges();

                    Utility.Emails.SendEmail(Utility.c.ReadSetting("FinanceEmailId"), "Petty Cash Added", Utility.Emails.GetEmailTemplate("PettyCashAdded"));
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Expence entry created successfully!",
                        status = "Success",
                        redirectURL = "/ExpenseTrackers/List"
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
        [CustomAuthorize(new string[] { "Admin", "Finance" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.ExpenseTracker expenceTracker = db.ExpenseTrackers.Find(id);
            if (expenceTracker == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.Status = new SelectList(Utility.DefaultList.GetPaymentStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.ExpenseCategory = new SelectList(db.ExpenceCategories, "Id", "CategoryName", expenceTracker.ExpenseCategoryId);
            return View(expenceTracker);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin", "Finance" })]
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
                    entity.ExpenseDate = DateTime.ParseExact(expenceTrackerData.ExpenseDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
