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
            var outwardList = (from outwards in db.Outwards
                               join outwarditems in db.OutwardItems on outwards.Id equals outwarditems.OutwardId
                               orderby outwards.InvoiceDate descending
                               select new OutwardData
                               {
                                   Id = outwards.Id,
                                   InvoiceNumber = outwards.InvoiceNumber,
                                   InvoiceDate = outwards.InvoiceDate,
                                   LRNumber = outwards.LRNumber,
                                   CustomerName = outwards.CustomerName,
                                   CreatedBy = outwards.CreatedBy,
                                   CreatedOn = outwards.CreatedOn,
                                   ModifiedBy = outwards.ModifiedBy,
                                   ModifiedOn = outwards.ModifiedOn,
                                   Comments = outwards.Comments
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
            var outwardData = new OutwardData() { OutwardItemData = new OutwardItemData() };

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

            return View(new OutwardItemData());
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult OutwardItemListPartial()
        {
            List<OutwardItemData> lstOutwardItems = new List<OutwardItemData>();
            return View(lstOutwardItems);
        }
    }

}