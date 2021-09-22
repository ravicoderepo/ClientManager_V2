﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        // GET: SaleActivities
        public ActionResult ListView(string callDateFrom ="", string callDateTo="", int status=0, string productName = "", string phoneNo = "")
        {
            DateTime dtcallDateFrom = new DateTime();
            DateTime dtcallDateTo = new DateTime();
            var currentUser = (UserDetails)Session["UserDetails"];
            var saleActivities = (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin")) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

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

            if (!string.IsNullOrEmpty(phoneNo))
            {
                saleActivities = saleActivities.Where(wh => wh.ClientPhoneNo.Contains(phoneNo.Trim()));
            }

            if (status > 0)
                saleActivities = saleActivities.Where(wh => wh.Status == status);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            statusList.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            ViewBag.Status = statusList;

            return PartialView(saleActivities.ToList());
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        // GET: SaleActivities
        public ActionResult List()
        {
            var currentUser = (UserDetails)Session["UserDetails"];
            var saleActivities = (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin")) ? db.SaleActivities.Include(s => s.SalesStatu) : (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "manager")) ? db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id || currentUser.ReportingToMe.Contains(wh.CreatedBy)) : db.SaleActivities.Include(s => s.SalesStatu).Where(wh => wh.CreatedBy == currentUser.Id);

            List<SelectListItem> statusList = new SelectList(db.SalesStatus, "Id", "Status").ToList();
            statusList.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            ViewBag.Status = statusList;

            return View(saleActivities);
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
        // GET: SaleActivities
        public ActionResult Index()
        {
            var saleActivities = db.SaleActivities.Include(s => s.Product).Include(s => s.SalesStatu).Include(s => s.User);
            return View(saleActivities.ToList());
        }

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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
        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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
                var saleDetails = new SaleActivity { SaleDate = saleData.SaleDate, Status = saleData.Status, ClientPhoneNo = saleData.ClientPhoneNo, ClientEmail = saleData.ClientEmail, ClientName = saleData.ClientName, ProductName = saleData.ProductName, RecentCallDate = saleData.RecentCallDate, Capacity = saleData.Capacity, Unit = saleData.Unit, Remarks = saleData.Remarks, CreatedBy = currentUser.Id, CreatedOn = DateTime.Now, AnticipatedClosingDate = saleData.AnticipatedClosingDate, SalesRepresentativeId = saleData.SalesRepresentativeId, NoOfFollowUps = saleData.NoOfFollowUps, InvoiceAmount = saleData.InvoiceAmount, InvoiceNo = saleData.InvoiceNo };

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
                        jsonRes = new JsonReponse { message = "Sale Activity saved successfully!", status = "Success", redirectURL = "/SaleActivities/Edit/" + saleDetails.Id };
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

        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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
            ViewBag.AccessLevel = (saleActivity.CreatedBy == currentUser.Id || currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin")) ? "Full" : "View";
            return View(saleActivity);
        }

        // POST: SaleActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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
                        if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientEmail) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || saleData.RecentCallDate == null || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.InvoiceNo) || saleData.InvoiceAmount < 0 || string.IsNullOrEmpty(saleData.Remarks))
                        {
                            saleData.DateOfClosing = DateTime.Now;
                            jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                            isMandatoryError = true;
                        }
                        else
                        {
                            lastSavedId = UpdateData(saleData, saleActivity, currentUser);
                        }
                    }
                    else if (saleData.SaleDate == null || saleData.SalesRepresentativeId <= 0 || saleData.Status <= 0 || string.IsNullOrEmpty(saleData.ClientPhoneNo) || string.IsNullOrEmpty(saleData.ClientEmail) || string.IsNullOrEmpty(saleData.ClientName) || string.IsNullOrEmpty(saleData.ProductName) || saleData.RecentCallDate == null || string.IsNullOrEmpty(saleData.Capacity) || string.IsNullOrEmpty(saleData.Unit) || string.IsNullOrEmpty(saleData.Remarks))
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
                            jsonRes = new JsonReponse { message = "Sale Activity updated successfully!", status = "Success", redirectURL = "/SaleActivities/Edit/" + saleActivity.Id };
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
            db.Entry(saleActivity).State = EntityState.Modified;

            saleActivity.SaleDate = saleData.SaleDate;
            saleActivity.Status = saleData.Status;
            saleActivity.ClientPhoneNo = saleData.ClientPhoneNo;
            saleActivity.ClientEmail = saleData.ClientEmail;
            saleActivity.ClientName = saleData.ClientName;
            saleActivity.ProductName = saleData.ProductName;
            saleActivity.RecentCallDate = saleData.RecentCallDate;
            saleActivity.Capacity = saleData.Capacity;
            saleActivity.Unit = saleData.Unit;
            saleActivity.Remarks = saleActivity.Remarks + ((!string.IsNullOrEmpty(saleData.Remarks)) ? "<br/>" + saleData.Remarks : "");
            saleActivity.AnticipatedClosingDate = saleData.AnticipatedClosingDate;
            saleActivity.SalesRepresentativeId = saleData.SalesRepresentativeId;
            saleActivity.NoOfFollowUps = (!string.IsNullOrEmpty(saleData.Remarks)) ? saleActivity.NoOfFollowUps + 1 : saleActivity.NoOfFollowUps;
            saleActivity.InvoiceAmount = saleData.InvoiceAmount;
            saleActivity.InvoiceNo = saleData.InvoiceNo;
            saleActivity.DateOfClosing = saleData.DateOfClosing;
            saleActivity.ModifiedOn = DateTime.Now;
            saleActivity.ModifiedBy = currentUser.Id;

            var lastSavedId = db.SaveChanges();
            return lastSavedId;
        }

        // GET: SaleActivities/Delete/5
        [CustomAuthorize("Admin", "Manager", "SalesRep")]
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
        //[CustomAuthorize("Admin", "Manager", "SalesRep")]
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
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
