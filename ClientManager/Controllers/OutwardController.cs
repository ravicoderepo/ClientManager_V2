using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class OutwardController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Inward
        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult List()
        {
            var outwardList = (from outward in db.Outwards
                               orderby outward.InvoiceDate descending
                               select new OutwardData
                               {
                                   Id = outward.Id,
                                   InvoiceDate = outward.InvoiceDate,
                                   InvoiceNumber = outward.InvoiceNumber,
                                   CustomerName = outward.CustomerName,
                                   CreatedBy = outward.CreatedBy,
                                   CreatedOn = outward.CreatedOn,
                                   ModifiedBy = outward.ModifiedBy,
                                   ModifiedOn = outward.ModifiedOn,
                                   Comments = outward.Comments,
                                   Status = outward.Status,
                               }).ToList();

            return View(outwardList);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult ListView()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Create()
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            ViewBag.Status = Utility.DefaultList.GetInvoiceStatusList();

            ViewBag.MaterialId = Utility.DefaultList.BindList(new SelectList(db.Materials.Where(wh => wh.IsActive == true), "MaterialId", "MaterialName", 1).ToList<SelectListItem>(), true);
            ViewBag.TypeId = Utility.DefaultList.BindList(new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == 0), "TypeId", "TypeName", 1).ToList<SelectListItem>(), true);
            ViewBag.ItemId = Utility.DefaultList.BindList(new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == 0), "ItemId", "ItemName", 1).ToList<SelectListItem>(), true);
            ViewBag.PaymentStatus = Utility.DefaultList.GetPaymentStatusList("INVOICE");
            var outwardData = new OutwardData();
            outwardData.DespatchData = new List<DespatchData>();

            return View(outwardData);
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Create(OutwardData outwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data = null;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(outwardData.CustomerName) || string.IsNullOrEmpty(outwardData.InvoiceNumber) || string.IsNullOrEmpty(outwardData.InvoiceNumber) || outwardData.InvoiceDate == null || string.IsNullOrEmpty(outwardData.Comments) || string.IsNullOrEmpty(outwardData.Status))
                    {
                        data = new JsonReponse()
                        {
                            message = "Enter all required fields of Invoice.",
                            status = "Failed",
                            redirectURL = ""
                        };
                        transaction.Rollback();
                    }
                    else
                    {
                        //Outward/Invoice
                        var outward = new DBOperation.Outward()
                        {
                            Status = outwardData.Status,
                            Comments = outwardData.Comments,
                            InvoiceDate = outwardData.InvoiceDate,
                            LRNumber = "",
                            InvoiceNumber = outwardData.InvoiceNumber,
                            CustomerName = outwardData.CustomerName,
                            CreatedOn = DateTime.Now,
                            CreatedBy = userData.Id,
                        };

                        this.db.Outwards.Add(outward);
                        var outwardId = this.db.SaveChanges();

                        if (outwardId > 0)
                        {
                            if (string.IsNullOrEmpty(outwardData.DespatchData.FirstOrDefault().LRNumber) || string.IsNullOrEmpty(outwardData.DespatchData.FirstOrDefault().TransportBy) || string.IsNullOrEmpty(outwardData.DespatchData.FirstOrDefault().ShipToCity) || outwardData.DespatchData.FirstOrDefault().DespatchDate == null || string.IsNullOrEmpty(outwardData.DespatchData.FirstOrDefault().PaymentStatus))
                            {
                                data = new JsonReponse()
                                {
                                    message = "Enter all required fields of Dispatch.",
                                    status = "Failed",
                                    redirectURL = ""
                                };
                                transaction.Rollback();
                                return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                var despatch = new DBOperation.Despatch();

                                var dispatchData = outwardData.DespatchData.FirstOrDefault();

                                despatch.OutwardId = outward.Id;

                                despatch.DespatchNo = dispatchData.DespatchNo;
                                despatch.DespatchDate = dispatchData.DespatchDate;
                                despatch.LRNumber = dispatchData.LRNumber;
                                despatch.TransportBy = dispatchData.TransportBy;
                                despatch.ShipToCity = dispatchData.ShipToCity;
                                despatch.PaymentStatus = dispatchData.PaymentStatus;

                                despatch.CreatedOn = DateTime.Now;
                                despatch.CreatedBy = userData.Id;

                                this.db.Despatches.Add(despatch);
                                var dispachId = this.db.SaveChanges();


                                if (dispachId > 0)
                                {
                                    if (dispatchData.DespatchItems != null && dispatchData.DespatchItems.Count() > 0)
                                    {
                                        var despatchItem = new DBOperation.DespatchItem();
                                        foreach (var item in dispatchData.DespatchItems)
                                        {
                                            var despatchItemData = new DespatchItem
                                            {
                                                ItemId = item.ItemId,
                                                DespatchId = despatch.Id,
                                                Quantity = item.Quantity,
                                            };

                                            this.db.DespatchItems.Add(despatchItemData);
                                        }
                                    }
                                    else
                                    {
                                        data = new JsonReponse()
                                        {
                                            message = "Add atlease one product details.",
                                            status = "Failed",
                                            redirectURL = ""
                                        };
                                        transaction.Rollback();
                                        return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
                                    }
                                }

                            }
                        }

                        var iTransId = this.db.SaveChanges();

                        if (iTransId > 0)
                        {
                            data = new JsonReponse()
                            {
                                message = "Outward & Despatch saved successfully!",
                                status = "Success",
                                redirectURL = "/Outward/List"
                            };
                            transaction.Commit();
                        }
                        else
                        {
                            data = new JsonReponse()
                            {
                                message = "Outward & Despatch entry not completed, try again after sometime.",
                                status = "Failed",
                                redirectURL = ""
                            };
                            transaction.Rollback();
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    data = new JsonReponse()
                    {
                        message = ex.Message,
                        status = "Error",
                        redirectURL = ""
                    };
                }
            }
            return (ActionResult)this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Edit(OutwardData outwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data = null;

            try
            {
                if (string.IsNullOrEmpty(outwardData.CustomerName) || string.IsNullOrEmpty(outwardData.InvoiceNumber) || string.IsNullOrEmpty(outwardData.InvoiceNumber) || outwardData.InvoiceDate == null || string.IsNullOrEmpty(outwardData.Comments) || string.IsNullOrEmpty(outwardData.Status))
                {
                    data = new JsonReponse()
                    {
                        message = "Enter all required fields of Invoice.",
                        status = "Failed",
                        redirectURL = ""
                    };
                }
                else
                {
                    DBOperation.Outward entity = db.Outwards.Where(wh => wh.Id == outwardData.Id).FirstOrDefault();

                    if (entity == null)
                    {
                        data = new JsonReponse()
                        {
                            message = "There is no record for given Id",
                            status = "Failed",
                            redirectURL = ""
                        };
                    }
                    else
                    {
                        //Outward/Invoice

                        entity.Status = outwardData.Status;
                        entity.Comments = outwardData.Comments;
                        entity.InvoiceDate = outwardData.InvoiceDate;
                        entity.LRNumber = "";
                        entity.InvoiceNumber = outwardData.InvoiceNumber;
                        entity.CustomerName = outwardData.CustomerName;

                        entity.ModifiedBy = new int?(userData.Id);
                        entity.ModifiedOn = new DateTime?(DateTime.Now);
                        this.db.Entry<DBOperation.Outward>(entity).State = EntityState.Modified;

                        if (this.db.SaveChanges() > 0)
                        {
                            data = new JsonReponse()
                            {
                                message = "Outward/Invoice details Updated successfully!",
                                status = "Success",
                                redirectURL = "/Outward/List"
                            };
                        }
                        else
                        {
                            data = new JsonReponse()
                            {
                                message = "Not completed, try again after sometime.",
                                status = "Failed",
                                redirectURL = ""
                            };
                        }
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

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Edit(int Id)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            var outward = (from oTrans in db.Outwards
                           orderby oTrans.InvoiceDate descending
                           where oTrans.Id == Id
                           select new OutwardData
                           {
                               Id = Id,
                               InvoiceNumber = oTrans.InvoiceNumber,
                               InvoiceDate = oTrans.InvoiceDate,
                               CustomerName = oTrans.CustomerName,
                               Status = oTrans.Status,
                               Comments = oTrans.Comments,
                               CreatedOn = oTrans.CreatedOn,
                               CreatedBy = oTrans.CreatedBy,
                               ModifiedOn = oTrans.ModifiedOn,
                               ModifiedBy = oTrans.ModifiedBy,
                               DespatchData = oTrans.Despatches.Select(sel => new DespatchData
                               {
                                   DespatchDate = sel.DespatchDate,
                                   DespatchNo = sel.DespatchNo,
                                   Id = sel.Id,
                                   LRNumber = sel.LRNumber,
                                   PaymentStatus = sel.PaymentStatus,
                                   TransportBy = sel.TransportBy,
                                   ShipToCity = sel.ShipToCity,
                                   CreatedBy = sel.CreatedBy,
                                   CreatedOn = sel.CreatedOn,
                                   ModifiedBy = sel.ModifiedBy,
                                   ModifiedOn = sel.ModifiedOn
                               }).ToList(),
                           }).FirstOrDefault();

            //ViewBag.PaymentStatus = Utility.DefaultList.GetPaymentStatusList("INVOICE");
            ViewBag.Status = Utility.DefaultList.GetInvoiceStatusList(outward.Status);
            return View(outward);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult EditDespatch(int Id)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            var outward = (from oTrans in db.Outwards
                           orderby oTrans.InvoiceDate descending
                           where oTrans.Id == Id
                           select new OutwardData
                           {
                               InvoiceNumber = oTrans.InvoiceNumber,
                               InvoiceDate = oTrans.InvoiceDate,
                               CustomerName = oTrans.CustomerName,
                               Status = oTrans.Status,
                               Comments = oTrans.Comments,
                               CreatedOn = oTrans.CreatedOn,
                               CreatedBy = oTrans.CreatedBy,
                               ModifiedOn = oTrans.ModifiedOn,
                               ModifiedBy = oTrans.ModifiedBy,
                               DespatchData = oTrans.Despatches.Select(sel => new DespatchData
                               {
                                   DespatchDate = sel.DespatchDate,
                                   DespatchNo = sel.DespatchNo,
                                   Id = sel.Id,
                                   LRNumber = sel.LRNumber,
                                   PaymentStatus = sel.PaymentStatus,
                                   TransportBy = sel.TransportBy,
                                   ShipToCity = sel.ShipToCity,
                                   CreatedBy = sel.CreatedBy,
                                   CreatedOn = sel.CreatedOn,
                                   ModifiedBy = sel.ModifiedBy,
                                   ModifiedOn = sel.ModifiedOn,
                                   DespatchItems = sel.DespatchItems.Select(desItems => new DespatchItemData
                                   {
                                       Id = desItems.Id,
                                       DespatchId = desItems.DespatchId,
                                       ItemId = desItems.ItemId,
                                       ItemName = desItems.Item.ItemName,
                                       MaterialId = desItems.Item.MaterialId,
                                       MaterialName = desItems.Item.Material.MaterialName,
                                       TypeId = desItems.Item.TypeId,
                                       TypeName = desItems.Item.Type.TypeName,
                                       Quantity = desItems.Quantity
                                   }).ToList(),
                               }).ToList(),
                           }).FirstOrDefault();

            ViewBag.MaterialId = new SelectList(db.Materials.Where(wh => wh.IsActive == true), "MaterialId", "MaterialName", 1).ToList<SelectListItem>();
            ViewBag.TypeId = new SelectList(db.Types.Where(wh => wh.IsActive == true), "TypeId", "TypeName", 1).ToList<SelectListItem>();
            ViewBag.ItemId = new SelectList(db.Items.Where(wh => wh.IsActive == true), "ItemId", "ItemName", 1).ToList<SelectListItem>();
            ViewBag.CompanyId = new SelectList(db.Companies.Where(wh => wh.IsActive == true), "CompanyId", "Name", 1).ToList<SelectListItem>();
            ViewBag.PaymentStatus = Utility.DefaultList.GetPaymentStatusList("INVOICE");
            ViewBag.Status = Utility.DefaultList.GetInvoiceStatusList(outward.Status);
            return PartialView(outward);
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult UpdateInvoice(OutwardData outwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;

            try
            {
                if (string.IsNullOrEmpty(outwardData.InvoiceNumber) || string.IsNullOrEmpty(outwardData.CustomerName) || outwardData.InvoiceDate == null || string.IsNullOrEmpty(outwardData.Comments) || string.IsNullOrEmpty(outwardData.Status))
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
                    DBOperation.Outward entity = db.Outwards.Where(wh => wh.Id == outwardData.Id).FirstOrDefault();
                    if (entity == null)
                    {
                        data = new JsonReponse()
                        {
                            message = "There is no record for given Id",
                            status = "Failed",
                            redirectURL = ""
                        };
                    }
                    else
                    {
                        entity.Status = outwardData.Status;
                        entity.Comments = outwardData.Comments;
                        entity.InvoiceDate = outwardData.InvoiceDate;
                        entity.InvoiceNumber = outwardData.InvoiceNumber;
                        entity.CustomerName = outwardData.CustomerName;
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedBy = userData.Id;

                        this.db.Entry<DBOperation.Outward>(entity).State = EntityState.Modified;

                        if (this.db.SaveChanges() > 0)
                        {
                            data = new JsonReponse()
                            {
                                message = "Outward details Updated successfully!",
                                status = "Success",
                                redirectURL = "/Outward/List"
                            };
                        }
                        else
                        {
                            data = new JsonReponse()
                            {
                                message = "Not completed, try again after sometime.",
                                status = "Failed",
                                redirectURL = ""
                            };
                        }
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

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult EditDespatch(DespatchData despatchData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;

            try
            {
                if (string.IsNullOrEmpty(despatchData.DespatchNo) || string.IsNullOrEmpty(despatchData.LRNumber) || despatchData.DespatchDate == null || string.IsNullOrEmpty(despatchData.PaymentStatus) || string.IsNullOrEmpty(despatchData.TransportBy) || string.IsNullOrEmpty(despatchData.ShipToCity))
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
                    DBOperation.Despatch entity = db.Despatches.Where(wh => wh.Id == despatchData.Id).FirstOrDefault();
                    if (entity == null)
                    {
                        data = new JsonReponse()
                        {
                            message = "There is no record for given Id",
                            status = "Failed",
                            redirectURL = ""
                        };
                    }
                    else
                    {
                        entity.PaymentStatus = despatchData.PaymentStatus;
                        entity.LRNumber = despatchData.LRNumber;
                        entity.DespatchDate = despatchData.DespatchDate;
                        entity.DespatchNo = despatchData.DespatchNo;
                        entity.TransportBy = despatchData.TransportBy;
                        entity.ShipToCity = despatchData.ShipToCity;
                        entity.ModifiedOn = DateTime.Now;
                        entity.ModifiedBy = userData.Id;

                        this.db.Entry<DBOperation.Despatch>(entity).State = EntityState.Modified;

                        if (this.db.SaveChanges() > 0)
                        {
                            data = new JsonReponse()
                            {
                                message = "Despatch details Updated successfully!",
                                status = "Success",
                                redirectURL = "/Outward/List"
                            };
                        }
                        else
                        {
                            data = new JsonReponse()
                            {
                                message = "Not completed, try again after sometime.",
                                status = "Failed",
                                redirectURL = ""
                            };
                        }
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

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult OutwardItemPartial()
        {
            ViewBag.Status = Utility.DefaultList.GetInvoiceStatusList();
            ViewBag.MaterialId = new SelectList(db.Materials, "MaterialId", "MaterialName", 1).ToList<SelectListItem>();
            ViewBag.TypeId = new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == 1), "TypeId", "TypeName", 1).ToList<SelectListItem>();
            ViewBag.ItemId = new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == 1), "ItemId", "ItemName", 1).ToList<SelectListItem>();
            return PartialView(new List<ClientManager.Models.DespatchItemData>());
        }
    }

}