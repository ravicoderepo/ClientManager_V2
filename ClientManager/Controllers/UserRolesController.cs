using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBOperation;

namespace ClientManager.Controllers
{
    public class UserRolesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: UserRoles
        public ActionResult Index()
        {
            var userRoles = db.UserRoles.Include(u => u.Role).Include(u => u.User).Include(u => u.User1).Include(u => u.User2).Include(u => u.User3);
            return View(userRoles.ToList());
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UserRole userRole = db.UserRoles.Find(id);
            return userRole == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)userRole);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,RoleId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", userRole.RoleId);
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", userRole.CreatedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", userRole.ModifiedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userRole = db.UserRoles.Find(id);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", userRole.RoleId);
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", userRole.CreatedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", userRole.ModifiedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            return View(userRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,RoleId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", userRole.RoleId);
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", userRole.CreatedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", userRole.ModifiedBy);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UserRole model = this.db.UserRoles.Find(id);
            return model == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)model);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.db.UserRoles.Remove(this.db.UserRoles.Find(id));
            this.db.SaveChanges();
            return (ActionResult)this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}
