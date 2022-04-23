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
        public ActionResult List()
        {
            var roles = db.Roles.Include(r => r.User).Include(r => r.User1);
            return View(roles.ToList());
        }

        // GET: Admin/Roles/Details/5
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
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoleName,IsActive,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", role.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", role.ModifiedBy);
            return View(role);
        }

        // GET: Admin/Roles/Edit/5
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
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", role.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", role.ModifiedBy);
            return View(role);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleName,IsActive,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", role.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", role.ModifiedBy);
            return View(role);
        }

        // GET: Admin/Roles/Delete/5
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

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Activate(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User entity = this.db.Users.Find(new object[1]
                {
          (object) id
                });
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(true);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<User>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks.ToString()
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

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult DeActivate(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User entity = this.db.Users.Find(new object[1]
                {
          (object) id
                });
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(false);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<User>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User De-Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks.ToString()
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
