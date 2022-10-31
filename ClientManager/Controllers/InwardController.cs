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
    public class InwardController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();
        // GET: Inward
        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult List()
        {
            var inwardList = (from iTrans in db.VRM_InwardStockTransaction
                              join iStock in db.VRM_InwardStock on iTrans.StockId equals iStock.StockId
                              orderby iTrans.CreatedDate descending
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
                                  //AvailableQuantity = iStock.AvailableQuantity, 
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
            ViewBag.MaterialId = Utility.DefaultList.BindList(new SelectList(db.Materials.Where(wh => wh.IsActive == true), "MaterialId", "MaterialName", 0).ToList<SelectListItem>(), true);
            ViewBag.TypeId = Utility.DefaultList.BindList(new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == 0), "TypeId", "TypeName", 0).ToList<SelectListItem>(), true);
            ViewBag.ItemId = Utility.DefaultList.BindList(new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == 0 && wh.ParentId == 0), "ItemId", "ItemName", 0).ToList<SelectListItem>(), true);
            ViewBag.CompanyId = Utility.DefaultList.BindList(new SelectList(db.Companies.Where(wh => wh.IsActive == true && wh.IsActive == true), "CompanyId", "Name", 0).ToList<SelectListItem>(),true);
            return View();
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult Create(InwardData inwardData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (inwardData.MaterialId <= 0 || inwardData.TypeId <= 0 || inwardData.ItemId <= 0 || inwardData.Quantity <= 0 || inwardData.ReceivedFrom <= 0 || string.IsNullOrEmpty(inwardData.PONumber) || string.IsNullOrEmpty(inwardData.GRNnumber))
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
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
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
                                  //AvailableQuantity = 0,//Utility.CommonFunctions.GetAvailableQuantity(iStock.ItemId),
                                  Quantity = iStock.Quantity,
                                  InwardStockTransactionId = iTrans.InwardStockTransactionId,
                                  PONumber = iTrans.PONumber,
                                  GRNnumber = iTrans.GRNnumber,
                                  ReceivedBy = iTrans.ReceivedBy,
                                  ReceivedDate = iTrans.ReceivedDate,
                                  ReceivedFrom = iTrans.ReceivedFrom,
                                  Description = iTrans.Description
                              }).FirstOrDefault();

            ViewBag.MaterialId = Utility.DefaultList.BindList(new SelectList(db.Materials.Where(wh => wh.IsActive == true), "MaterialId", "MaterialName", inward.MaterialId).ToList<SelectListItem>(), true);
            ViewBag.TypeId = Utility.DefaultList.BindList(new SelectList(db.Types.Where(wh => wh.IsActive == true && wh.MaterialId == inward.MaterialId), "TypeId", "TypeName", inward.TypeId).ToList<SelectListItem>(), true);
            ViewBag.ItemId = Utility.DefaultList.BindList(new SelectList(db.Items.Where(wh => wh.IsActive == true && wh.TypeId == inward.TypeId && wh.ParentId == 0), "ItemId", "ItemName", inward.ItemId).ToList<SelectListItem>(), true);
            ViewBag.CompanyId = Utility.DefaultList.BindList(new SelectList(db.Companies.Where(wh => wh.IsActive == true), "CompanyId", "Name", inward.ReceivedFrom).ToList<SelectListItem>(), true);
            if (userData.UserRoles.Any(wh => wh.RoleName != "Store Admin"))
            {
                ViewBag.AccessLevel = "View";
            }
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
                    if (inwardData.MaterialId <= 0 || inwardData.TypeId <= 0 || inwardData.ItemId <= 0 || inwardData.Quantity <= 0|| inwardData.ReceivedFrom <= 0|| string.IsNullOrEmpty(inwardData.PONumber) || string.IsNullOrEmpty(inwardData.GRNnumber))
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
                        DBOperation.VRM_InwardStock entityStock = db.VRM_InwardStock.Where(wh => wh.StockId == inwardData.StockId).FirstOrDefault();
                        DBOperation.VRM_InwardStockTransaction entityTrans = db.VRM_InwardStockTransaction.Where(wh => wh.InwardStockTransactionId == inwardData.InwardStockTransactionId).FirstOrDefault();
                        if (entityStock == null)
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
                            //Stock
                            entityStock.MaterialId = inwardData.MaterialId;
                            entityStock.TypeId = inwardData.TypeId;
                            entityStock.ItemId = inwardData.ItemId;
                            entityStock.Quantity = inwardData.Quantity;
                            entityStock.IsActive = inwardData.IsActive;

                            //Transaction
                            entityTrans.PONumber = inwardData.PONumber;
                            entityTrans.GRNnumber = inwardData.GRNnumber;
                            entityTrans.ReceivedFrom = inwardData.ReceivedFrom;
                            entityTrans.ReceivedBy = userData.FullName;
                            entityTrans.ReceivedDate = inwardData.ReceivedDate;
                            entityTrans.Description = inwardData.Description;

                            this.db.Entry<DBOperation.VRM_InwardStock>(entityStock).State = EntityState.Modified;
                            this.db.Entry<DBOperation.VRM_InwardStockTransaction>(entityTrans).State = EntityState.Modified;

                            if (this.db.SaveChanges() > 0)
                            {
                                transaction.Commit();
                                data = new JsonReponse()
                                {
                                    message = "Inward details Updated successfully!",
                                    status = "Success",
                                    redirectURL = "/Inward/List"
                                };
                            }
                            else
                            {
                                transaction.Rollback();
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
        [CustomAuthorize(new string[] { "Super User", "Super Admin", "Sales Manager", "Sales Engineer", "Store Admin" })]
        public ActionResult GetStocks()
        {
            var stocks = (from stock in db.VRM_InwardStock.AsEnumerable()
                          group stock by stock.ItemId into itemGroup
                          select new ItemSummaryData
                          {
                              ItemId = itemGroup.FirstOrDefault().ItemId,
                              ItemName = itemGroup.FirstOrDefault().Item.ItemName,
                              MaterialName = itemGroup.FirstOrDefault().Material.MaterialName,
                              TypeName = itemGroup.FirstOrDefault().Type.TypeName,
                              TotalQuantity = itemGroup.Sum(s => s.Quantity),
                              AvailableQuantity = Utility.CommonFunctions.GetAvailableQuantity(itemGroup.FirstOrDefault().ItemId),
                          }).ToList();


            return View(stocks);
        }

        [HttpGet]
        [CustomAuthorize(new string[] { "Super Admin", "Super User", "Store Admin" })]
        public ActionResult GetStockDetails(int Id)
        {
            var inwardList = (from iTrans in db.VRM_InwardStockTransaction
                              join iStock in db.VRM_InwardStock on iTrans.StockId equals iStock.StockId
                              where iStock.ItemId == Id
                              orderby iTrans.ReceivedDate descending
                              select new InwardData
                              {
                                  StockId = iStock.StockId,
                                  InwardStockTransactionId = iTrans.InwardStockTransactionId,
                                  MaterialId = iStock.MaterialId,
                                  MaterialName = iStock.Material.MaterialName,
                                  TypeId = iStock.TypeId,
                                  TypeName = iStock.Type.TypeName,
                                  ItemId = iStock.TypeId,
                                  ItemName = iStock.Item.ItemName,
                                  Quantity = iStock.Quantity,
                                  PONumber = iTrans.PONumber,
                                  GRNnumber = iTrans.GRNnumber,
                                  ReceivedBy = iTrans.ReceivedBy,
                                  ReceivedDate = iTrans.ReceivedDate,
                                  Description = iTrans.Description
                              }).ToList();

            return PartialView(inwardList);
        }
    }
}