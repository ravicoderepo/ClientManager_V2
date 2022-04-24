using ClientManager.Infrastructure;
using ClientManager.Models;
using DBOperation;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ClientManager.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        [CustomAuthorize(new string[] { "Admin", "Manager" })]
        public ActionResult List()
        {
            UserDetails currentUser = (UserDetails)this.Session["UserDetails"];
            DbSet<User> users = this.db.Users;
            List<int> restrictedUSers = users.Where(wh => wh.UserRoles.Any(rol => rol.Role.RoleName == "Admin")).Select(sel => sel.Id).ToList();
            if (currentUser.UserRoles.Any(wh => wh.RoleName.ToLower() == "admin"))
                return (ActionResult)this.View((object)users);
            return (ActionResult)this.View(users.Where(wh => !restrictedUSers.Contains(wh.Id) & wh.ReportingManager == currentUser.Id).ToList());
        }

        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Activate(int? id)
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
                    redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks.ToString()
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
        public ActionResult DeActivate(int? id)
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
                    redirectURL = "/Admin/User/List?" + DateTime.Now.Ticks.ToString()
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
        public ActionResult Create()
        {
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ReportingManager = new SelectList(db.Users, "Id", "FullName");
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "FullName");
            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            ViewBag.Status = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View();

        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Admin" })]
        public ActionResult Create(UserData userData)
        {
            UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
            System.Collections.Generic.List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            JsonReponse jsonReponse = (JsonReponse)null;
            
            JsonReponse data;
            try
            {
                this.db.Users.FirstOrDefault<User>((Expression<Func<User, bool>>)(wh => wh.Id == userData.Id));
                int num = 0;
                if (string.IsNullOrEmpty(userData.UserId) || string.IsNullOrEmpty(userData.Password) || string.IsNullOrEmpty(userData.FullName))
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
                    this.db.Users.Add(new User()
                    {
                        FullName = userData.FullName,
                        Email = userData.UserId,
                        Password = userData.Password,
                        AddressLine1 = userData.AddressLine1,
                        AddressLine2 = userData.AddressLine2,
                        City = userData.City,
                        State = userData.State,
                        Pincode = userData.PinCode,
                        IsActive = new bool?(userData.IsActive),
                        DateOfBirth = userData.DateOfBirth,
                        DateOfJoining = userData.DateofJoining,
                        EmployeeId = userData.EmpId,
                        ReportingManager = userData.ReportingManager,
                        SaleTarget = userData.SaleTarget,
                        CreatedBy = new int?(userDetails.Id),
                        CreatedOn = new DateTime?(DateTime.Now)
                    });
                    num = this.db.SaveChanges();
                }
                if (num > 0)
                    data = new JsonReponse()
                    {
                        message = "User created successfully!",
                        status = "Success",
                        redirectURL = "/Admin/User/List"
                    };
                else
                    data = new JsonReponse()
                    {
                        message = "User creation not completed, try again after sometime.",
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

        [CustomAuthorize(new string[] { "Admin", "Manager" })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "FullName", user.ModifiedBy);
            ViewBag.ReportingManager = new SelectList(db.Users, "Id", "FullName", user.ReportingManager);
            ViewBag.CreatedBy = new SelectList(db.Users, "Id", "FullName", user.CreatedBy);

            List<SelectListItem> items = new System.Collections.Generic.List<SelectListItem>();
            items.Insert(0, new SelectListItem()
            {
                Text = "Active",
                Value = "1"
            });
            items.Insert(1, new SelectListItem()
            {
                Text = "De-Active",
                Value = "0"
            });
            ViewBag.Status = new SelectList(items, "Value", "Text", (object)1).ToList<SelectListItem>();
            return View(user);
        }

        [HttpPost]
        [CustomAuthorize(new string[] { "Admin", "Manager" })]
        public ActionResult Edit(UserData userData)
        {
            JsonReponse data;
            try
            {
                UserDetails userDetails = (UserDetails)this.Session["UserDetails"];
                User entity = this.db.Users.FirstOrDefault<User>((Expression<Func<User, bool>>)(wh => wh.Id == userData.Id));
                if (entity == null)
                    data = new JsonReponse()
                    {
                        message = "There is no record for given Id",
                        status = "Failed",
                        redirectURL = ""
                    };
                else if (string.IsNullOrEmpty(userData.UserId) || string.IsNullOrEmpty(userData.Password) || string.IsNullOrEmpty(userData.FullName))
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
                    this.db.Entry<User>(entity).State = EntityState.Modified;
                    string str;
                    if (userDetails.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>)(wh => wh.RoleName.ToLower() == "admin")))
                    {
                        entity.FullName = userData.FullName;
                        entity.Email = userData.UserId;
                        entity.Password = userData.Password;
                        entity.EmployeeId = userData.EmpId;
                        entity.AddressLine1 = userData.AddressLine1;
                        entity.AddressLine2 = userData.AddressLine2;
                        entity.City = userData.City;
                        entity.State = userData.State;
                        entity.Pincode = userData.PinCode;
                        entity.IsActive = new bool?(userData.IsActive);
                        entity.DateOfBirth = userData.DateOfBirth;
                        entity.DateOfJoining = userData.DateofJoining;
                        entity.SaleTarget = userData.SaleTarget;
                        entity.ReportingManager = userData.ReportingManager;
                        str = "User Updated";
                    }
                    else
                        str = "User Sale Target";
                    entity.ModifiedBy = new int?(userDetails.Id);
                    entity.ModifiedOn = new DateTime?(DateTime.Now);

                    if (this.db.SaveChanges() > 0)
                        data = new JsonReponse()
                        {
                            message = str + " successfully!",
                            status = "Success",
                            redirectURL = "/Admin/User/List"
                        };
                    else
                        data = new JsonReponse()
                        {
                            message = str + " Not completed, try again after sometime.",
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
