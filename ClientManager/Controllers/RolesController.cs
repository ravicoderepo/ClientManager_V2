using DBOperation;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;


namespace ClientManager.Controllers
{
    public class RolesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: Roles
        public ActionResult Index()
        {
            var roles = db.Roles.Include(r => r.User).Include(r => r.User1);
            return View(roles.ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Role role = db.Roles.Find(id);
            return role == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)role);            
        }

        // GET: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Roles/Create
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
                return (ActionResult)this.RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", role.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", role.ModifiedBy);
            return View(role);
        }

        // GET: Roles/Edit/5
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

        // POST: Roles/Edit/5
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

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Role role = db.Roles.Find(id);
            return role == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)role);
        }     

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            this.db.Roles.Remove(this.db.Roles.Find(id));
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
