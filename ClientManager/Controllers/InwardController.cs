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
    public class InwardController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Inward
        [HttpGet]
        public ActionResult List()
        {
            var inwardList = (from iTrans in db.VRM_InwardStockTransaction
                              join iStock in db.VRM_InwardStock on iTrans.StockId equals iStock.StockId
                              orderby iTrans.ReceivedDate descending
                              select new InwardData 
                              { 
                                  StockId=iStock.StockId,
                                  InwardStockTransactionId=iTrans.InwardStockTransactionId,
                                  MaterialId = iStock.MaterialId, 
                                  MaterialName = iStock.Material.MaterialName,
                                  TypeId = iStock.TypeId, 
                                  TypeName = iStock.Type.TypeName,
                                  ItemId = iStock.TypeId, 
                                  ItemName = iStock.Item.ItemName,
                                  AvailableQuantity = iStock.AvailableQuantity, 
                                  Quantity = iStock.Quantity, 
                                  PONumber = iTrans.PONumber, 
                                  GRNnumber = iTrans.GRNnumber, 
                                  ReceivedBy = iTrans.ReceivedBy, 
                                  ReceivedDate = iTrans.ReceivedDate, 
                                  Description = iTrans.Description 
                              }).ToList();

            return View(inwardList);
        }
        [HttpGet]
        public ActionResult ListView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            ViewBag.MaterialId = new SelectList(db.Materials, "MaterialId", "MaterialName", 1).ToList<SelectListItem>();
            ViewBag.TypeId = new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == 1), "TypeId", "TypeName", 1).ToList<SelectListItem>();
            ViewBag.ItemId = new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == 1), "ItemId", "ItemName", 1).ToList<SelectListItem>();
            ViewBag.CompanyId = new SelectList(db.Companies.Where(wh => wh.IsActive == true && wh.IsActive == true), "CompanyId", "Name", 1).ToList<SelectListItem>();
            return View();
        }
        [HttpPost]
        public ActionResult Create(InwardData inwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (inwardData.MaterialId <= 0 || inwardData.TypeId <= 0 || inwardData.ItemId <= 0 || inwardData.Quantity <= 0 || inwardData.ReceivedFrom <= 0 || string.IsNullOrEmpty(inwardData.PONumber) || string.IsNullOrEmpty(inwardData.GRNnumber) || inwardData.ReceivedDate == null)
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
                        //Stock
                        var stock = new DBOperation.VRM_InwardStock()
                        {
                            MaterialId = inwardData.MaterialId,
                            TypeId = inwardData.TypeId,
                            ItemId = inwardData.ItemId,
                            Quantity = inwardData.Quantity,
                            AvailableQuantity = Utility.CommonFunctions.GetAvailableQuantity(inwardData.ItemId) + inwardData.Quantity,
                            IsActive = true,

                        };

                        this.db.VRM_InwardStock.Add(stock);

                        var stockId = this.db.SaveChanges();
                        //Inward Transaction
                        if (stockId > 0)
                        {
                            //var rcvdDate = DateTime.ParseExact(inwardData.ReceivedDate.ToString().Substring(0, 10).Replace('-', '/'), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            this.db.VRM_InwardStockTransaction.Add(new DBOperation.VRM_InwardStockTransaction()
                            {
                                PONumber = inwardData.PONumber,
                                GRNnumber = inwardData.GRNnumber,
                                ReceivedFrom = inwardData.ReceivedFrom,
                                ReceivedBy = userData.FullName,
                                ReceivedDate = DateTime.Now,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                Description = inwardData.Description,
                                StockId = stock.StockId,
                            });
                        }
                        var iTransId = this.db.SaveChanges();

                        ////Available Quantity Update
                        //var tempStock = db.VRM_InwardStock.Where(wh => wh.ItemId == inwardData.ItemId);
                        //var currAvlQty = Utility.CommonFunctions.GetAvailableQuantity(inwardData.ItemId);

                        //var iStockUpdate = tempStock.Where(wh=> wh.StockId == stockId).FirstOrDefault();
                        //iStockUpdate.AvailableQuantity = currAvlQty + inwardData.Quantity;

                        //this.db.VRM_InwardStock.Attach(iStockUpdate);
                        //this.db.Entry(iStockUpdate).State = EntityState.Modified;
                        //this.db.SaveChanges();

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
        public ActionResult Edit(int Id)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];

            var inward= (from iTrans in db.VRM_InwardStockTransaction
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
    }
}