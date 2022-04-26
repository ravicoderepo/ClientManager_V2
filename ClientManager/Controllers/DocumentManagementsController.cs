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
    public class DocumentManagementsController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: DocumentManagements
        public ActionResult Index()
        {
            var documentManagements = db.DocumentManagements.Include(d => d.User).Include(d => d.User1);
            return View(documentManagements.ToList());
        }

        // GET: DocumentManagements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentManagement documentManagement = db.DocumentManagements.Find(id);
            if (documentManagement == null)
            {
                return HttpNotFound();
            }
            return View(documentManagement);
        }

        // GET: DocumentManagements/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: DocumentManagements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DocumentSource,ReferenceRecId,DocumentName,Description,FileName,FileExtension,FileData,URL,CreatedDate,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] DocumentManagement documentManagement)
        {
            if (ModelState.IsValid)
            {
                db.DocumentManagements.Add(documentManagement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", documentManagement.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", documentManagement.ModifiedBy);
            return View(documentManagement);
        }

        // GET: DocumentManagements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentManagement documentManagement = db.DocumentManagements.Find(id);
            if (documentManagement == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", documentManagement.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", documentManagement.ModifiedBy);
            return View(documentManagement);
        }

        // POST: DocumentManagements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DocumentSource,ReferenceRecId,DocumentName,Description,FileName,FileExtension,FileData,URL,CreatedDate,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] DocumentManagement documentManagement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentManagement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "Password", documentManagement.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Password", documentManagement.ModifiedBy);
            return View(documentManagement);
        }

        // GET: DocumentManagements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentManagement documentManagement = db.DocumentManagements.Find(id);
            if (documentManagement == null)
            {
                return HttpNotFound();
            }
            return View(documentManagement);
        }

        // POST: DocumentManagements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentManagement documentManagement = db.DocumentManagements.Find(id);
            db.DocumentManagements.Remove(documentManagement);
            db.SaveChanges();
            return RedirectToAction("Index");
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
