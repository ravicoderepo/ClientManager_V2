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
            ViewBag.MaterialId = new SelectList(db.Materials, "MaterialId", "MaterialName", 1).ToList<SelectListItem>();
            ViewBag.TypeId = new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == 1), "TypeId", "TypeName", 1).ToList<SelectListItem>();
            ViewBag.ItemId = new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == 1), "ItemId", "ItemName", 1).ToList<SelectListItem>();
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

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Edit(int Id)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            var inward = (from iTrans in db.VRM_InwardStockTransaction
                          join iStock in db.VRM_InwardStock on iTrans.StockId equals iStock.StockId
                          where iTrans.InwardStockTransactionId == Id
                          orderby iTrans.ReceivedDate descending
                          select new InwardData
                          {
                              StockId = iStock.StockId,
                              MaterialId = iStock.MaterialId,
                              MaterialName = iStock.Material.MaterialName,
                              TypeId = iStock.TypeId,
                              TypeName = iStock.Type.TypeName,
                              ItemId = iStock.ItemId,
                              ItemName = iStock.Item.ItemName,
                              AvailableQuantity = iStock.AvailableQuantity,
                              Quantity = iStock.Quantity,
                              InwardStockTransactionId = iTrans.InwardStockTransactionId,
                              PONumber = iTrans.PONumber,
                              GRNnumber = iTrans.GRNnumber,
                              ReceivedBy = iTrans.ReceivedBy,
                              ReceivedDate = iTrans.ReceivedDate,
                              ReceivedFrom = iTrans.ReceivedFrom,
                              Description = iTrans.Description
                          }).FirstOrDefault();


            ViewBag.MaterialId = new SelectList(db.Materials.Where(wh => wh.IsActive == true), "MaterialId", "MaterialName", inward.MaterialId).ToList<SelectListItem>();
            ViewBag.TypeId = new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == inward.MaterialId), "TypeId", "TypeName", inward.TypeId).ToList<SelectListItem>();
            ViewBag.ItemId = new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == inward.TypeId), "ItemId", "ItemName", inward.ItemId).ToList<SelectListItem>();
            ViewBag.CompanyId = new SelectList(db.Companies.Where(wh => wh.IsActive == true), "CompanyId", "Name", inward.ReceivedFrom).ToList<SelectListItem>();

            return View(inward);
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Edit(InwardData inwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    if (inwardData.MaterialId > 0 || inwardData.TypeId > 0 || inwardData.ItemId > 0 || inwardData.Quantity > 0 || inwardData.ReceivedFrom > 0 || string.IsNullOrEmpty(inwardData.PONumber) || string.IsNullOrEmpty(inwardData.GRNnumber) || inwardData.ReceivedDate != null)
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


                        this.db.VRM_InwardStock.Add(new DBOperation.VRM_InwardStock()
                        {
                            MaterialId = inwardData.MaterialId,
                            TypeId = inwardData.TypeId,
                            ItemId = inwardData.ItemId,
                            Quantity = inwardData.Quantity,
                            AvailableQuantity = 0,
                            IsActive = inwardData.IsActive,

                        });
                        var stockId = this.db.SaveChanges();

                        this.db.VRM_InwardStockTransaction.Add(new DBOperation.VRM_InwardStockTransaction()
                        {
                            PONumber = inwardData.PONumber,
                            GRNnumber = inwardData.GRNnumber,
                            ReceivedFrom = inwardData.ReceivedFrom,
                            ReceivedBy = userData.FullName,
                            ReceivedDate = inwardData.ReceivedDate,
                            Description = inwardData.Description,
                            StockId = stockId,
                        });
                        var iTransId = this.db.SaveChanges();
                        transaction.Commit();
                        if (iTransId > 0)
                            data = new JsonReponse()
                            {
                                message = "Inward & Stock saved successfully!",
                                status = "Success",
                                redirectURL = "/Inward/List"
                            };
                        else
                            data = new JsonReponse()
                            {
                                message = "Inward entry not completed, try again after sometime.",
                                status = "Failed",
                                redirectURL = ""
                            };
                    }
                }
                catch (Exception ex)
                {
                    transaction.Commit();
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