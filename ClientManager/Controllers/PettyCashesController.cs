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
    public class PettyCashesController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: PettyCashes
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult List()
        {
            var pettyCashes = db.PettyCashes.Include(p => p.User).Include(p => p.User1);
            return View(pettyCashes.ToList());
        }



        // GET: PettyCashes/Create
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Create()
        {
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "ATM",
                Value = "ATM"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Account Transfer",
                Value = "Account Transfer"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Cash",
                Value = "Cash"
            });
            items.Insert(3, new SelectListItem()
            {
                Text = "Other Receivables",
                Value = "Other Receivables"
            });
            ViewBag.ModeOfPayment = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View();
        }


        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Create(Models.PettyCashData PettyCashData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;
                if (PettyCashData.AmountReceived <= 0 || PettyCashData.AmountRecivedDate == null || string.IsNullOrEmpty(PettyCashData.Description))
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
                    this.db.PettyCashes.Add(new DBOperation.PettyCash()
                    {
                        AmountReceived = PettyCashData.AmountReceived,
                        AmountRecivedDate = PettyCashData.AmountRecivedDate,
                        ModeOfPayment = PettyCashData.ModeOfPayment,
                        Description = PettyCashData.Description,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Petty Cash entry created successfully!",
                        status = "Success",
                        redirectURL = "/PettyCashes/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "Petty Cash entry not completed, try again after sometime.",
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
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.PettyCash pettyCash = db.PettyCashes.Find(id);
            if (pettyCash == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "ATM",
                Value = "ATM"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Account Transfer",
                Value = "Account Transfer"
            });
            items.Insert(2, new SelectListItem()
            {
                Text = "Cash",
                Value = "Cash"
            });
            items.Insert(3, new SelectListItem()
            {
                Text = "Other Receivables",
                Value = "Other Receivables"
            });
            ViewBag.ModeOfPayment = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(pettyCash);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Edit(Models.PettyCashData PettyCashData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                DBOperation.PettyCash entity = this.db.PettyCashes.FirstOrDefault(wh => wh.Id == PettyCashData.Id);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (PettyCashData.AmountReceived <= 0 || PettyCashData.AmountRecivedDate == null || string.IsNullOrEmpty(PettyCashData.Description))
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
                    this.db.Entry<DBOperation.PettyCash>(entity).State = EntityState.Modified;

                    entity.AmountReceived = PettyCashData.AmountReceived;
                    entity.AmountRecivedDate = PettyCashData.AmountRecivedDate;
                    entity.ModeOfPayment = PettyCashData.ModeOfPayment;
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = "Petty Cash updated successfully!",
                            status = "Success",
                            redirectURL = "/ExpenceCategories/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = "Petty Cash update not completed, try again after sometime.",
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
