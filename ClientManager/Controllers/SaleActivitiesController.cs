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
    [CustomAuthenticationFilter]
    public class SaleActivitiesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager", "Sales Engineer" })]
        // GET: SaleActivities
        public ActionResult ListView(string callDateFrom ="", string callDateTo="", int status=0, string productName = "", int salesPerson = 0)
        {
            DateTime dtcallDateFrom = new DateTime();
            DateTime dtcallDateTo = new DateTime();
            var currentUser = (UserDetails)Session["UserDetails"];
            var saleActivities = (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "Super Admin")) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "Sales Manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            if (!string.IsNullOrEmpty(callDateFrom))
            {
                dtcallDateFrom = DateTime.Parse(callDateFrom);
                saleActivities = saleActivities.Where(wh => wh.SaleDate >= dtcallDateFrom);
            }
            
            if (!string.IsNullOrEmpty(callDateTo))
            {
                dtcallDateTo = DateTime.Parse(callDateTo);
                saleActivities = saleActivities.Where(wh => wh.SaleDate <= dtcallDateTo);
            }


            if (!string.IsNullOrEmpty(productName))
            {
                saleActivities = saleActivities.Where(wh => wh.ProductName.Contains(productName.Trim()));
            }

            if (salesPerson > 0)
            {
                saleActivities = saleActivities.Where(wh => wh.CreatedBy == salesPerson);
            }

            if (status > 0)
                saleActivities = saleActivities.Where(wh => wh.Status == status);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            statusList.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            ViewBag.Status = statusList;

            return PartialView(saleActivities.OrderByDescending(ord=> ord.SaleDate).ToList());
        }

        [CustomAuthorize(new string[] { "Super Admin", "Sales Manager", "Sales Engineer" })]
        // GET: SaleActivities
        public ActionResult List()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            string[] superroles = { "Super Admin", "Super User" };
            var saleActivities = (currentUser.UserRoles.Any(wh => superroles.Contains(wh.RoleName))) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "sales manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            var salesPersons = db.Users.Where(wh=> wh.IsActive == true);
            List<SelectListItem> selesPersonList = new List<SelectListItem>();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "super admin" || wh.RoleName.ToLower() == "super user"))
            {
                string[] roleNames = { "Sales Manager", "Sales Engineer" };
                selesPersonList  = new SelectList(db.UserRoles.Where(rl => roleNames.Contains(rl.Role.RoleName)).Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName }), "Id", "FullName").ToList();               
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

            //db.UserRoles.Where(rl => rl.Role.RoleName.ToLower() == "Sales Engineer").Select(sel => new { Id = sel.UserId, FullName = sel.User1.FullName });            


            statusList.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            selesPersonList.Insert(0, (new SelectListItem { Text = "", Value = "0" }));

            ViewBag.SalesPerson = selesPersonList;
            ViewBag.Status = statusList;
           

            return View(saleActivities.OrderByDescending(ord => ord.SaleDate).ToList());
        }

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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
        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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
                    SaleDate = DateTime.ParseExact(saleData.SaleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture), 
                    Status = saleData.Status, 
                    ClientPhoneNo = saleData.ClientPhoneNo, 
                    ClientEmail = saleData.ClientEmail, 
                    ClientName = saleData.ClientName, 
                    ProductName = saleData.ProductName, 
                    RecentCallDate = DateTime.ParseExact(saleData.RecentCallDate, "MM/dd/yyyy", CultureInfo.InvariantCulture), 
                    Capacity = saleData.Capacity, 
                    Unit = saleData.Unit, 
                    Remarks = saleData.Remarks, 
                    CreatedBy = currentUser.Id, 
                    CreatedOn = DateTime.Now, 
                    AnticipatedClosingDate = DateTime.ParseExact(saleData.AnticipatedClosingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture), 
                    SalesRepresentativeId = saleData.SalesRepresentativeId, 
                    NoOfFollowUps = saleData.NoOfFollowUps, 
                    InvoiceAmount = saleData.InvoiceAmount, 
                    InvoiceNo = saleData.InvoiceNo 
                };

                if (saleData.Status == 6)
                {
                    if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName)  || saleData.RecentCallDate == null || string.IsNullOrEmpty(saleData.Remarks) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.InvoiceNo) || saleData.InvoiceAmount < 0)
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

        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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
        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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
                        if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.InvoiceNo) || saleData.InvoiceAmount < 0 || string.IsNullOrEmpty(saleData.Remarks))
                        {
                            saleData.DateOfClosing = DateTime.ParseExact(DateTime.Now.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
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
            saleActivity.SaleDate = DateTime.ParseExact(saleData.SaleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            saleActivity.Status = saleData.Status;
            saleActivity.ClientPhoneNo = saleData.ClientPhoneNo;
            saleActivity.ClientEmail = saleData.ClientEmail;
            saleActivity.ClientName = saleData.ClientName;
            saleActivity.ProductName = saleData.ProductName;
            saleActivity.RecentCallDate = DateTime.ParseExact(saleData.RecentCallDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            saleActivity.Capacity = saleData.Capacity;
            saleActivity.Unit = saleData.Unit;
            saleActivity.Remarks += !string.IsNullOrEmpty(saleData.Remarks) ? "<br/>" + saleData.Remarks + "-" + DateTime.ParseExact(saleData.RecentCallDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString() : "";
            saleActivity.AnticipatedClosingDate = DateTime.ParseExact(saleData.AnticipatedClosingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
            saleActivity.DateOfClosing = DateTime.ParseExact(saleData.RecentCallDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            saleActivity.ModifiedOn = new DateTime?(DateTime.Now);
            saleActivity.ModifiedBy = new int?(currentUser.Id);
            return this.db.SaveChanges();
        }


        // GET: SaleActivities/Delete/5
        [CustomAuthorize("Super Admin", "Sales Manager", "Sales Engineer")]
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
