using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;

namespace ClientManager.Controllers
{
    [CustomAuthenticationFilter]
    public class SaleActivitiesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize(new string[] { "Super User", "Super Admin", "Sales Manager", "Sales Engineer" })]
        // GET: SaleActivities
        public ActionResult ListView(int status = 0, int month = 0, int year = 0, string productName = "", string customerName = "", int salesPerson = 0, string searchFrom="")
        {
            month = (month > 0) ? month : 0;
            year = (year > 0) ? year : 0;

            var currentUser = (UserDetails)Session["UserDetails"];
            string[] superroles = { "Super Admin", "Super User" };
            if (salesPerson == 0)
            {
                salesPerson = currentUser.Id;
                ViewBag.SelectedSalesPerson = currentUser.FullName;
            }
            else if (salesPerson == -1)
            {
                ViewBag.SelectedSalesPerson = " all sales user";
            }
            else
            {
                ViewBag.SelectedSalesPerson = db.Users.FirstOrDefault(n => n.Id == salesPerson).FullName;
            }

            var saleActivities = (currentUser.UserRoles.Any(wh => superroles.Contains(wh.RoleName))) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            var salesPersons = db.Users.Where(wh => wh.IsActive == true);
            List<SelectListItem> selesPersonList = new List<SelectListItem>();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin" || wh.RoleName.ToLower() == "super user"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName)).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }
            else if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && currentUser.ReportingToMe.Contains(rl.UserId) || rl.UserId == currentUser.Id).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }
            else if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales engineer"))
            {
                string[] roleNames = { "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && rl.UserId == currentUser.Id).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();
            }

            if (!string.IsNullOrEmpty(productName))
            {
                saleActivities = saleActivities.Where(wh => wh.ProductName.Contains(productName.Trim()));
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                saleActivities = saleActivities.Where(wh => wh.ClientName.Contains(customerName.Trim()));
            }

            if (year > 0)
                saleActivities = saleActivities.Where(wh => wh.SaleDate.Year == year);
            else
            {
                //if (string.IsNullOrEmpty(searchFrom) || searchFrom == "Dashboard" || searchFrom == "DashboardTeam")
                //{
                //    saleActivities = saleActivities.Where(wh => wh.SaleDate.Year == DateTime.Now.Year);
                //}

            }

            if (month > 0)
                saleActivities = saleActivities.Where(wh => wh.SaleDate.Month == month);
            else
            {
                //if (string.IsNullOrEmpty(searchFrom) || searchFrom != "Dashboard" || searchFrom == "DashboardTeam")
                //{
                //    saleActivities = saleActivities.Where(wh => wh.SaleDate.Month == DateTime.Now.Month);
                //}
            }


            if (salesPerson > 0)
            {
                if(searchFrom != "DashboardTeam" )
                    //&& !currentUser.UserRoles.Any(a=> a.RoleName == "Sales Manager" || a.RoleName =="Super User" || a.RoleName =="Super Admin"))
                {
                    saleActivities = saleActivities.Where(wh => wh.CreatedBy == salesPerson);
                }
            }

            //1   Initial Call
            //2   In Discussion
            //3   Pending from Customer
            //4   Cancelled
            //5   PO Received – WIP
            //6   Closed
            if (status > 0 && status !=1235)
            {
                saleActivities = saleActivities.Where(wh => wh.Status == status);
            }
            else if (status == 1235)
            {
                saleActivities = saleActivities.Where(wh => wh.Status != 4 && wh.Status != 6);
            }
            var output = saleActivities.Where(wh => wh.Status == 6 && wh.InvoiceAmount != null).Select(sel => (sel.InvoiceAmount.HasValue) ? sel.InvoiceAmount.Value : 0).ToList();
            ViewBag.TotalSalesBySalesPerson = " Rs." + output.Sum(s => s).ToString("#,##,##0.00");

            if (searchFrom == "Link")
            {
                //4-Cancelled, 6-Closed
                saleActivities = saleActivities.Where(wh => wh.Status != 6 && wh.Status !=4);
            }

            var result = saleActivities.OrderBy(wh => wh.Status).ToList();

            statusList.Insert(0, (new SelectListItem { Text = "All", Value = "0" }));
            selesPersonList.Insert(0, (new SelectListItem { Text = "All", Value = "0" }));

            ViewBag.SalesPerson = selesPersonList;
            ViewBag.Status = statusList;

            return PartialView(result.ToList());
        }

        [CustomAuthorize(new string[] { "Super User", "Super Admin", "Sales Manager", "Sales Engineer" })]
        // GET: SaleActivities
        public ActionResult List()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            string[] superroles = { "Super Admin", "Super User" };

            //var saleActivities = db.SaleActivities.Include(s => s.SalesStatu);
            var saleActivities = (currentUser.UserRoles.Any(wh => superroles.Contains(wh.RoleName))) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            var salesPersons = db.Users.Where(wh => wh.IsActive == true);
            List<SelectListItem> selesPersonList = new List<SelectListItem>();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin" || wh.RoleName.ToLower() == "super user"))
            {
                string[] roleNames = { "Super User", "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && rl.User1.IsActive == true).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName", currentUser.Id).ToList();
            }
            else if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && currentUser.ReportingToMe.Contains(rl.UserId) && rl.User1.IsActive == true || rl.UserId == currentUser.Id).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName", currentUser.Id).ToList();
            }
            else if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales engineer"))
            {
                string[] roleNames = { "Sales Engineer" };
                selesPersonList = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName) && rl.UserId == currentUser.Id && rl.User1.IsActive == true).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName", currentUser.Id).ToList();
            }

            //ViewBag.SelectedSalesPerson = " of " + currentUser.FullName;
            //db.UserRoles.Where(rl => rl.Role.RoleName.ToLower() == "Sales Engineer").Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName });            


            var output = saleActivities.Where(wh => wh.Status == 6 && wh.InvoiceAmount != null).Select(sel => (sel.InvoiceAmount.HasValue) ? sel.InvoiceAmount.Value : 0).ToList();
            ViewBag.TotalSalesBySalesPerson = "Rs." + output.Sum(s => s).ToString("#,##,##0.00");


            statusList.Insert(0, (new SelectListItem { Text = "All", Value = "0" }));
            selesPersonList.Insert(0, (new SelectListItem { Text = "All", Value = "0" }));

            ViewBag.SalesPerson = selesPersonList;
            ViewBag.Status = statusList;
            ViewBag.Years = new SelectList(Utility.DefaultList.GetYearList(), "Value", "Text", "").ToList();
            ViewBag.Months = new SelectList(Utility.DefaultList.GetMonthList(), "Value", "Text", "").ToList();

            var result = from sale in saleActivities orderby sale.Status ascending select sale;

            return PartialView(result.ToList());
        }

        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        // GET: SaleActivities
        public ActionResult Index()
        {
            var saleActivities = db.SaleActivities.Include(s => s.Product).Include(s => s.SalesStatu).Include(s => s.User);
            return View(saleActivities.ToList());
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
        // GET: SaleActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleActivity saleActivity = db.SaleActivities.Find(id);
            if (saleActivity == null)
            {
                return HttpNotFound();
            }
            return View(saleActivity);
        }

        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        // GET: SaleActivities/Create
        public ActionResult Create()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", 1);
            ViewBag.Representative = new SelectList(db.Users, "Id", "FullName", currentUser.Id);
            //ViewBag.ProductName = new SelectList(db.Products, "Id", "ProductName", 1);
            ViewBag.Unit = new SelectList(Utility.DefaultList.GetUnitList(), "Text", "Value", 1);

            return View();
        }

        //POST: SaleActivities/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,SaleDate,Status,ClientName,ClientEmail,ClientPhoneNo,ProductId,Capacity,Unit,RecentCallDate,AnticipatedClosingDate,NoOfFollowUps,Remarks,SalesRepresentativeId,InvoiceNo,InvoiceAmount,DateOfClosing,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] SaleActivity saleActivity)
        public ActionResult Create(SaleData saleData)
        {
            JsonReponse jsonRes = null;
            SaleActivity saleActivity = new SaleActivity();
            var currentUser = (UserDetails)Session["UserDetails"];
            var lastSavedId = 0;
            try
            {
                var saleDetails = new SaleActivity
                {
                    SaleDate = DateTime.ParseExact(saleData.SaleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Status = saleData.Status,
                    ClientPhoneNo = saleData.ClientPhoneNo,
                    ClientEmail = saleData.ClientEmail,
                    ClientName = saleData.ClientName,
                    ProductName = saleData.ProductName,
                    RecentCallDate = DateTime.ParseExact(saleData.RecentCallDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Capacity = saleData.Capacity,
                    Unit = saleData.Unit,
                    Remarks = saleData.Remarks,
                    CreatedBy = currentUser.Id,
                    CreatedOn = DateTime.Now,
                    AnticipatedClosingDate = (!string.IsNullOrEmpty(saleData.AnticipatedClosingDate)) ? DateTime.ParseExact(saleData.AnticipatedClosingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : new Nullable<DateTime>(),
                    SalesRepresentativeId = saleData.SalesRepresentativeId,
                    NoOfFollowUps = saleData.NoOfFollowUps,
                    InvoiceAmount = saleData.InvoiceAmount,
                    InvoiceNo = saleData.InvoiceNo
                };

                if (saleData.Status == 6)
                {
                    if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || saleData.RecentCallDate == null || string.IsNullOrEmpty(saleData.Remarks) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.InvoiceNo) || saleData.InvoiceAmount < 0)
                    {
                        jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                    }
                    else
                    {
                        saleDetails.DateOfClosing = DateTime.Now;
                        db.SaleActivities.Add(saleDetails);
                        lastSavedId = db.SaveChanges();

                        if (lastSavedId > 0)
                        {
                            jsonRes = new JsonReponse { message = "Sale Activity saved successfully!", status = "Success", redirectURL = "/SaleActivities/Edit/" + saleDetails.Id };
                        }
                        else
                        {
                            jsonRes = new JsonReponse { message = "Sale Activity not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                        }
                    }
                }
                else if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || saleData.RecentCallDate == null || string.IsNullOrEmpty(saleData.Remarks) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit))
                {
                    jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                }
                else
                {
                    db.SaleActivities.Add(saleDetails);
                    lastSavedId = db.SaveChanges();

                    if (lastSavedId > 0)
                    {
                        jsonRes = new JsonReponse { message = "Sale Activity saved successfully!", status = "Success", redirectURL = "/SaleActivities/List" };
                    }
                    else
                    {
                        jsonRes = new JsonReponse { message = "Sale Activity not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);

            //ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductCode", saleActivity.ProductId);
            //ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", saleActivity.Status);
            //ViewBag.SalesRepresentativeId = new SelectList(db.Users, "Id", "Password", saleActivity.SalesRepresentativeId);
            //return View(saleActivity);
        }

        [CustomAuthorize("Super User", "Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        // GET: SaleActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentUser = (UserDetails)Session["UserDetails"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleActivity saleActivity = db.SaleActivities.Find(id);
            if (saleActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", saleActivity.SalesStatu.Id);
            ViewBag.Representative = new SelectList(db.Users, "Id", "FullName", currentUser.Id);
            //ViewBag.ProductName = new SelectList(db.Products, "Id", "ProductName", saleActivity.ProductId);
            ViewBag.Unit = new SelectList(Utility.DefaultList.GetUnitList(), "Text", "Value", saleActivity.Unit);
            ViewBag.AccessLevel = (saleActivity.CreatedBy == currentUser.Id || currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "Super Admin")) ? "Full" : "View";
            return View(saleActivity);
        }

        // POST: SaleActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,SaleDate,Status,ClientName,ClientEmail,ClientPhoneNo,ProductId,Capacity,Unit,RecentCallDate,AnticipatedClosingDate,NoOfFollowUps,Remarks,SalesRepresentativeId,InvoiceNo,InvoiceAmount,DateOfClosing,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] SaleActivity saleActivity)
        public ActionResult Edit(SaleData saleData)
        {
            JsonReponse jsonRes = null;
            SaleActivity saleActivity = new SaleActivity();
            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                saleActivity = db.SaleActivities.FirstOrDefault(wh => wh.Id == saleData.Id);
                int lastSavedId = 0;
                bool isMandatoryError = false;
                if (saleActivity == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    if (saleData.Status == 6)
                    {
                        if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.InvoiceNo) || saleData.InvoiceAmount < 0 || string.IsNullOrEmpty(saleData.Remarks) || string.IsNullOrEmpty(saleData.DateOfClosing))
                        {
                            //saleData.DateOfClosing = DateTime.Now.ToString("dd/MM/yyyy"); //DateTime.ParseExact(DateTime.Now.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
                            jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                            isMandatoryError = true;
                        }
                        else
                        {
                            lastSavedId = UpdateData(saleData, saleActivity, currentUser);
                        }
                    }
                    else if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.Remarks))
                    {
                        jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                        isMandatoryError = true;
                    }
                    else
                    {
                        lastSavedId = UpdateData(saleData, saleActivity, currentUser);
                    }

                    if (isMandatoryError == false)
                    {
                        if (lastSavedId > 0)
                        {
                            jsonRes = new JsonReponse { message = "Sale Activity updated successfully!", status = "Success", redirectURL = "/SaleActivities/List" };
                        }
                        else
                        {
                            jsonRes = new JsonReponse { message = "Sale Activity update not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);

            //    db.Entry(saleActivity).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("List");

            //ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductCode", saleActivity.ProductId);
            //ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", saleActivity.Status);
            //ViewBag.SalesRepresentativeId = new SelectList(db.Users, "Id", "Password", saleActivity.SalesRepresentativeId);
            //return View(saleActivity);
        }

        private int UpdateData(SaleData saleData, SaleActivity saleActivity, UserDetails currentUser)
        {
            this.db.Entry<SaleActivity>(saleActivity).State = EntityState.Modified;
            saleActivity.SaleDate = DateTime.ParseExact(saleData.SaleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            saleActivity.Status = saleData.Status;
            saleActivity.ClientPhoneNo = saleData.ClientPhoneNo;
            saleActivity.ClientEmail = saleData.ClientEmail;
            saleActivity.ClientName = saleData.ClientName;
            saleActivity.ProductName = saleData.ProductName;
            saleActivity.RecentCallDate = DateTime.ParseExact(saleData.RecentCallDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            saleActivity.Capacity = saleData.Capacity;
            saleActivity.Unit = saleData.Unit;
            saleActivity.Remarks += !string.IsNullOrEmpty(saleData.Remarks) ? "<br/>" + saleData.Remarks + "-" + DateTime.ParseExact(saleData.RecentCallDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString().Substring(0, 10) : "";
            saleActivity.AnticipatedClosingDate = (!string.IsNullOrEmpty(saleData.AnticipatedClosingDate)) ? DateTime.ParseExact(saleData.AnticipatedClosingDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : new Nullable<DateTime>();
            saleActivity.SalesRepresentativeId = saleData.SalesRepresentativeId;
            SaleActivity saleActivity1 = saleActivity;
            int? nullable;
            if (string.IsNullOrEmpty(saleData.Remarks))
            {
                nullable = saleActivity.NoOfFollowUps;
            }
            else
            {
                int? noOfFollowUps = saleActivity.NoOfFollowUps;
                nullable = noOfFollowUps.HasValue ? new int?(noOfFollowUps.GetValueOrDefault() + 1) : new int?();
            }
            saleActivity1.NoOfFollowUps = nullable;
            saleActivity.InvoiceAmount = new Decimal?(saleData.InvoiceAmount);
            saleActivity.InvoiceNo = saleData.InvoiceNo;
            saleActivity.DateOfClosing = DateTime.ParseExact(saleData.RecentCallDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            saleActivity.ModifiedOn = new DateTime?(DateTime.Now);
            saleActivity.ModifiedBy = new int?(currentUser.Id);
            return this.db.SaveChanges();
        }


        // GET: SaleActivities/Delete/5
        [CustomAuthorize("Super User", "Super Admin", "Sales Manager", "Sales Engineer")]
        public ActionResult Delete(int? id)
        {
            JsonReponse jsonRes = null;
            SaleActivity saleActivity = new SaleActivity();
            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                saleActivity = db.SaleActivities.FirstOrDefault(wh => wh.Id == id);

                if (saleActivity == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    db.SaleActivities.Remove(saleActivity);
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "Sale Activity deleted successfully!", status = "Success", redirectURL = "/SaleActivities/List/" };

                    //jsonRes = new JsonReponse { message = "Sale Activity deleted not completed, try again after sometime.", status = "Failed", redirectURL = "" };

                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //SaleActivity saleActivity = db.SaleActivities.Find(id);
            //if (saleActivity == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(saleActivity);
        }

        // POST: SaleActivities/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SaleActivity saleActivity = db.SaleActivities.Find(id);
        //    db.SaleActivities.Remove(saleActivity);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public JsonResult GetItemCode(string term)
        {
            // var codes = db.w_Items.Where(i => i.ItemCode.StartsWith(term)).ToList();
            var result = new Dictionary<string, string>();
            var namecodes = new List<SelectListItem>();
            namecodes = (from u in db.SaleActivities.Where(wh=> wh.ClientName.Contains(term)) select new SelectListItem { Text = u.ClientName, Value = u.Id.ToString() }).Distinct().ToList();

            foreach (var item in namecodes)
            {
                if (!result.ContainsKey(item.Text.ToString()))
                {
                    result.Add(item.Text, item.Value.ToString());
                }
            }

            var namecodes1 = result.Where(s => s.Key.ToLower().Contains
                        (term.ToLower())).Select(w => w).ToList();
           return Json(namecodes1, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetItemDetails(int id)
        {
            var codeList = db.SaleActivities.Where(i => i.Id == id).ToList();

            var viewmodel = codeList.Select(x => new
            {
                Name = x.ClientName,
                Email = x.ClientEmail,
                PhoneNo = x.ClientPhoneNo
            }).Distinct();

            return Json(viewmodel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
