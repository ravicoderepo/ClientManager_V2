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
    public class DocumentsController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: PettyCashes        
        public ActionResult List()
        {
            var documents = db.Documents.Include(p => p.User).Include(p => p.User1);
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            return View(documents.ToList());
        }

        // GET: PettyCashes/Create        
        public ActionResult Create()
        {
            ViewBag.DocumentType = new SelectList(Utility.DefaultList.GetDocumentTypesList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.Status = new SelectList(Utility.DefaultList.GetDocumentStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.DocumentSource = new SelectList(Utility.DefaultList.GetDocumentTypesList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.ReferenceRecId = new SelectList(Utility.DefaultList.GetDocumentTypesList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            return View();
        }

        // POST: ExpenceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Models.DocumentData DocumentData)
        {
            UserDetails userData = (UserDetails)this.Session["UserDetails"];
            JsonReponse jsonReponse = (JsonReponse)null;

            JsonReponse data;
            try
            {
                int num = 0;

                if (DocumentData.FileName != null || DocumentData.DocumentType != null || DocumentData.DocumentSource != null || DocumentData.ReferenceRecId > 0 || DocumentData.Status != null || string.IsNullOrEmpty(DocumentData.Description))
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
                    this.db.Documents.Add(new DBOperation.Document()
                    {
                        FileName = DocumentData.FileName,
                        DocumentType = DocumentData.DocumentType,
                        DocumentSource = DocumentData.DocumentSource,
                        ReferenceRecId = DocumentData.ReferenceRecId,
                        Status = DocumentData.Status,
                        CreatedBy = userData.Id,
                        CreatedOn = DateTime.Now
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "Document datails created successfully!",
                        status = "Success",
                        redirectURL = "/Documents/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "Document datails not completed, try again after sometime.",
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBOperation.Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentType = new SelectList(Utility.DefaultList.GetPaymentModeList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.Status = new SelectList(Utility.DefaultList.GetStatusList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.DocumentSource = new SelectList(Utility.DefaultList.GetPaymentModeList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            ViewBag.ReferenceRecId = new SelectList(Utility.DefaultList.GetPaymentModeList(), "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(document);
        }

        // POST: ExpenceCategories/Edit/5
        [HttpPost]
        public ActionResult Edit(Models.DocumentData DocumentData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                DBOperation.Document entity = this.db.Documents.FirstOrDefault(wh => wh.Id == DocumentData.Id);
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (DocumentData.FileName != null || DocumentData.DocumentType != null || DocumentData.DocumentSource != null || DocumentData.ReferenceRecId > 0 || DocumentData.Status != null || string.IsNullOrEmpty(DocumentData.Description))
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
                    this.db.Entry<DBOperation.Document>(entity).State = EntityState.Modified;

                    entity.FileName = DocumentData.FileName;
                    entity.DocumentType = DocumentData.DocumentType;
                    entity.DocumentSource = DocumentData.DocumentSource;
                    entity.ReferenceRecId = DocumentData.ReferenceRecId;
                    entity.Status = DocumentData.Status;
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = "Document datails updated successfully!",
                            status = "Success",
                            redirectURL = "/Documents/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = "Document datails update not completed, try again after sometime.",
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

        [HttpGet]
        public ActionResult DocumentStatusUpdate(int id, string status)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (id <= 0)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                Document entity = this.db.Documents.Find(id);

                if (entity == null)
                    return (ActionResult)this.HttpNotFound();

                entity.Status = status;
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<Document>(entity).State = EntityState.Modified;
                this.db.SaveChanges();

                data = new JsonReponse()
                {
                    message = "Document status updated.",
                    status = "Success",
                    redirectURL = "/Documents/List?" + DateTime.Now.Ticks.ToString()
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
