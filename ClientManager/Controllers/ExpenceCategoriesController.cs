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
    public class ExpenceCategoriesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: ExpenceCategories
        [CustomAuthorize(new string[] { "Admin", "OfficeAdmin" })]
        public ActionResult List()
        {
            var expenceCategories = db.ExpenceCategories.Include(e => e.User).Include(e => e.User1);
            return View(expenceCategories.ToList());
        }

        // GET: ExpenceCategories/Create
        [CustomAuthorize(new string[] { "Admin", "OfficeAdmin" })]
        public ActionResult Create()
        {
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ReportingManager = new SelectList(db.Users, "Id", "FullName");
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "FullName");
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            ViewBag.Status = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View();

        }

        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin", "OfficeAdmin" })]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.ExpenceCategory ExpenceCategoryData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            System.Collections.Generic.List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;
                if (string.IsNullOrEmpty(ExpenceCategoryData.CategoryName) || string.IsNullOrEmpty(ExpenceCategoryData.Description))
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
                    this.db.ExpenceCategories.Add(new DBOperation.ExpenceCategory()
                    {
                        CategoryName = ExpenceCategoryData.CategoryName,
                        Description = ExpenceCategoryData.Description,
                        IsActive = ExpenceCategoryData.IsActive,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Expence category created successfully!",
                        status = "Success",
                        redirectURL = "/ExpenceCategories/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "Expence category creation not completed, try again after sometime.",
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
        [CustomAuthorize(new string[] { "Admin", "OfficeAdmin" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.ExpenceCategory expenceCategory = db.ExpenceCategories.Find(id);
            if (expenceCategory == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            ViewBag.Status = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(expenceCategory);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin", "OfficeAdmin" })]
        public ActionResult Edit(Models.ExpenceCategory ExpenceCategoryData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                DBOperation.ExpenceCategory entity = this.db.ExpenceCategories.FirstOrDefault(wh => wh.Id == ExpenceCategoryData.Id);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (string.IsNullOrEmpty(ExpenceCategoryData.CategoryName) || string.IsNullOrEmpty(ExpenceCategoryData.Description))
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
                    this.db.Entry<DBOperation.ExpenceCategory>(entity).State = EntityState.Modified;
                    string str;
                    if (userDetails.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>)(wh => wh.RoleName.ToLower() == "admin")))
                    {
                        entity.CategoryName = ExpenceCategoryData.CategoryName;
                        entity.Description = ExpenceCategoryData.Description;
                        entity.IsActive = ExpenceCategoryData.IsActive;                       
                        str = "Expence Category Updated";
                    }
                    else
                        str = "User Sale Target";
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = str + " successfully!",
                            status = "Success",
                            redirectURL = "/ExpenceCategories/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = str + " Not completed, try again after sometime.",
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