using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;

namespace ClientManager.Controllers
{
    public class AdminController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize(new string[] { "Admin", "Manager" })]
        public ActionResult UserList() => (ActionResult)this.View((object)this.db.Users.ToList<User>());

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult ActivateUser(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User entity = this.db.Users.Find(new object[1]
                {
          (object) id
                });
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(true);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<User>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/UserList?" + DateTime.Now.Ticks.ToString()
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

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult DeActivateUser(int? id)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            JsonReponse data;
            try
            {
                if (!id.HasValue)
                    return (ActionResult)new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User entity = this.db.Users.Find(new object[1]
                {
          (object) id
                });
                if (entity == null)
                    return (ActionResult)this.HttpNotFound();
                entity.IsActive = new bool?(false);
                entity.ModifiedBy = new int?(userDetails.Id);
                entity.ModifiedOn = new DateTime?(DateTime.Now);
                this.db.Entry<User>(entity).State = EntityState.Modified;
                this.db.SaveChanges();
                data = new JsonReponse()
                {
                    message = "User De-Activated successfully!",
                    status = "Success",
                    redirectURL = "/Admin/UserList?" + DateTime.Now.Ticks.ToString()
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

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Delete(int? id)
        {
            User user = new User();
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                User entity = this.db.Users.FirstOrDefault<User>((Expression<Func<User, bool>>)(wh => (int?)wh.Id == id));
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
                    this.db.Users.Remove(entity);
                    this.db.SaveChanges();
                    data = new JsonReponse()
                    {
                        message = "user deleted successfully!",
                        status = "Success",
                        redirectURL = "/Admin/User/List/"
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
                this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}