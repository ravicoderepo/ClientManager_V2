using System;
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
    public class ItemsController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: ExpenceCategories
        [CustomAuthorize(new string[] { "Super Admin","Super User" })]
        public ActionResult List()
        {
            var items = db.Items;
            return View(items.ToList());
        }

        // GET: ExpenceCategories/Create
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(Utility.DefaultList.GetStatusList(), "Value", "Text", 1).ToList<SelectListItem>();
            return View();
        }

        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult Create(Models.ItemData itemData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;
                if (string.IsNullOrEmpty(itemData.ItemName) || string.IsNullOrEmpty(itemData.Description))
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
                    this.db.Items.Add(new DBOperation.Item()
                    {
                        ItemName = itemData.ItemName,
                        Description = itemData.Description,
                        IsActive = itemData.IsActive,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Item created successfully!",
                        status = "Success",
                        redirectURL = "/items/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "Item creation not completed, try again after sometime.",
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
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.Item expenceCategory = db.Items.Find(id);
            if (expenceCategory == null)
            {
                return HttpNotFound();
            }

           
            ViewBag.Status = new SelectList(Utility.DefaultList.GetStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(expenceCategory);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult Edit(Models.ItemData itemData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                DBOperation.Item entity = this.db.Items.FirstOrDefault(wh => wh.ItemId == itemData.ItemId);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (string.IsNullOrEmpty(itemData.ItemName) || string.IsNullOrEmpty(itemData.Description))
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
                    this.db.Entry<DBOperation.Item>(entity).State = EntityState.Modified;
                    string str;
                    if (userDetails.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>)(wh => wh.RoleName.ToLower() == "super admin")))
                    {
                        entity.ItemName = itemData.ItemName;
                        entity.Description = itemData.Description;
                        entity.IsActive = itemData.IsActive;                       
                        str = "Expence Category Updated";
                    }

                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = "Updated successfully!",
                            status = "Success",
                            redirectURL = "/items/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = "Not completed, try again after sometime.",
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
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult Activate(int? id)
        {
            JsonReponse data;
            try
            {

                DBOperation.Item entity = this.db.Items.FirstOrDefault(wh => wh.ItemId == id);


                if (entity == null)
                {
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = "/Items/List"
                    };
                }
                else
                {
                    this.db.Entry<DBOperation.Item>(entity).State = EntityState.Modified;
                    entity.IsActive = true;

                    if (this.db.SaveChanges() > 0)
                    {
                        data = new JsonReponse()
                        {
                            message = "Activated Successfully!",
                            status = "Success",
                            redirectURL = "/Items/List"
                        };
                    }
                    else
                    {
                        data = new JsonReponse()
                        {
                            message = "Failed to update!",
                            status = "Error",
                            redirectURL = "/Items/List"
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                data = new JsonReponse()
                {
                    message = ex.Message,
                    status = "Error",
                    redirectURL = "/Items/List"
                };
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User" })]
        public ActionResult DeActivate(int? id)
        {
            JsonReponse data;
            try
            {

                DBOperation.Item entity = this.db.Items.FirstOrDefault(wh => wh.ItemId == id);


                if (entity == null)
                {
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = "/Items/List"
                    };
                }
                else
                {
                    this.db.Entry<DBOperation.Item>(entity).State = EntityState.Modified;
                    entity.IsActive = false;

                    if (this.db.SaveChanges() > 0)
                    {
                        data = new JsonReponse()
                        {
                            message = "De-Activated Successfully!",
                            status = "Success",
                            redirectURL = "/Items/List"
                        };
                    }
                    else
                    {
                        data = new JsonReponse()
                        {
                            message = "Failed to update!",
                            status = "Error",
                            redirectURL = "/Items/List"
                        };
                    }

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
