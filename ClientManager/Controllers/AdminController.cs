using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class AdminController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: Admin
        [CustomAuthorize("Admin","Manager")]
        public ActionResult UserList()
        {
            var user = db.Users;
            return View(user.ToList());
        }

        [CustomAuthorize("Admin")]
        public ActionResult ActivateUser(int? id)
        {
            UserDetails currentUser = (UserDetails)Session["UserDetails"];
            JsonReponse jsonRes = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                    
                }

                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.IsActive = true;
                    user.ModifiedBy = currentUser.Id;
                    user.ModifiedOn = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "User Activated successfully!", status = "Success", redirectURL = "/Admin/UserList?" + DateTime.Now.Ticks };
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize("Admin")]
        public ActionResult DeActivateUser(int? id)
        {
            UserDetails currentUser = (UserDetails)Session["UserDetails"];
            JsonReponse jsonRes = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                User user = db.Users.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    user.IsActive = false;
                    user.ModifiedBy = currentUser.Id;
                    user.ModifiedOn = DateTime.Now;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "User De-Activated successfully!", status = "Success", redirectURL = "/Admin/UserList?" + DateTime.Now.Ticks };
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize("Admin")]
        // GET: Admin/User/Edit/5
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

            return View(saleActivity);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize("Admin", "Manager")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,SaleDate,Status,ClientName,ClientEmail,ClientPhoneNo,ProductId,Capacity,Unit,RecentCallDate,AnticipatedClosingDate,NoOfFollowUps,Remarks,SalesRepresentativeId,InvoiceNo,InvoiceAmount,DateOfClosing,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] SaleActivity saleActivity)
        public ActionResult Edit(UserDetails userData)
        {
            JsonReponse jsonRes = null;            
            
            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                var user = db.Users.FirstOrDefault(wh => wh.Id == userData.Id);
                int lastSavedId = 0;
                if (user == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    if (!true)
                    {
                        jsonRes = new JsonReponse { message = "Enter all required fields.", status = "Failed", redirectURL = "" };
                    }
                    else
                    {
                        db.Users.Add(new User ());
                        db.Entry(user).State = EntityState.Modified;
                        lastSavedId = db.SaveChanges();
                    }


                    if (lastSavedId > 0)
                    {
                        jsonRes = new JsonReponse { message = "User updated successfully!", status = "Success", redirectURL = "/SaleActivities/Edit/" + user.Id };
                    }
                    else
                    {
                        jsonRes = new JsonReponse { message = "User update not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/User/Delete/5
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int? id)
        {
            JsonReponse jsonRes = null;
            User user = new User();
            try
            {
                var currentUser = (UserDetails)Session["UserDetails"];

                user = db.Users.FirstOrDefault(wh => wh.Id == id);

                if (user == null)
                {
                    jsonRes = new JsonReponse { message = "There is no record for given Id", status = "Failed", redirectURL = "" };
                }
                else
                {
                    db.Users.Remove(user);
                    db.SaveChanges();

                    jsonRes = new JsonReponse { message = "user deleted successfully!", status = "Success", redirectURL = "/Admin/User/List/" };

                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
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