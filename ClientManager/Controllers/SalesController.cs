using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBOperation;

namespace ClientManager.Views.Sales
{
    public class SalesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: Sales
        public ActionResult List()
        {
            var sales = db.Sales.Include(s => s.SalesStatu).Include(s => s.User);
            return View(sales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status");
            ViewBag.RepresentativeId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SaleDate,Status,AnticipatedClosing,NoOfFollowUps,NextFollowUpDate,RepresntativeId,Remarks,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", sale.Status);
            ViewBag.RepresntativeId = new SelectList(db.Users, "Id", "Password", sale.RepresentativeId);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", sale.Status);
            ViewBag.RepresentativeId = new SelectList(db.Users, "Id", "Password", sale.RepresentativeId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SaleDate,Status,AnticipatedClosing,NoOfFollowUps,NextFollowUpDate,RepresntativeId,Remarks,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(db.SalesStatus, "Id", "Status", sale.Status);
            ViewBag.RepresntativeId = new SelectList(db.Users, "Id", "Password", sale.RepresentativeId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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
