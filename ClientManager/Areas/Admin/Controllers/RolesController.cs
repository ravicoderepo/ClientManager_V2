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

namespace ClientManager.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: Admin/Roles
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult List()
        {
            var roles = db.Roles.Include(r => r.User).Include(r => r.User1);
            return View(roles.ToList());
        }

        // GET: Admin/Roles/Details/5
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Admin/Roles/Create
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Create()
        {
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ReportingManager = new SelectList(db.Users, "Id", "FullName");
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "FullName");
            ViewBag.Status = new SelectList(Utility.DefaultList.GetStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Create(Role RoleData)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
          
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;
                if (string.IsNullOrEmpty(RoleData.RoleName))
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
                    this.db.Roles.Add(new Role()
                    {
                        RoleName = RoleData.RoleName,
                        IsActive = RoleData.IsActive,
                        CreatedBy = userDetails.Id,
                        CreatedOn = DateTime.Now
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Role created successfully!",
                        status = "Success",
                        redirectURL = "/Admin/Roles/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "User creation not completed, try again after sometime.",
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

        // GET: Admin/Roles/Edit/5
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Status = new SelectList(Utility.DefaultList.GetStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Edit(Role RoleData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                Role entity = this.db.Roles.FirstOrDefault(wh => wh.Id == RoleData.Id);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (string.IsNullOrEmpty(RoleData.RoleName))
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
                    this.db.Entry<Role>(entity).State = EntityState.Modified;
                    string str;

                    entity.RoleName = RoleData.RoleName;
                    entity.IsActive = RoleData.IsActive;    
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);
                    str = "Role Updated";

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = str + " successfully!",
                            status = "Success",
                            redirectURL = "/Admin/Roles/List"
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

        // GET: Admin/Roles/Delete/5
        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult Activate(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Role entity = this.db.Roles.Find(id);
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(true);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<Role>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/Roles/List?" + DateTime.Now.Ticks.ToString()
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

        [CustomAuthorize(new string[] { "Super Admin" })]
        public ActionResult DeActivate(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Role entity = this.db.Roles.Find(id);
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(false);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<Role>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User De-Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/Roles/List?" + DateTime.Now.Ticks.ToString()
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
