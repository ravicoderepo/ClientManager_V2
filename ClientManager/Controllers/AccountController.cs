using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBOperation;
using ClientManager.Models;

namespace ClientManager.Controllers
{
    public class AccountController : Controller
    {
        private ClientManagerEntities db = new ClientManagerEntities();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserDetails"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Login userLogin)
        {
            JsonReponse jsonRes = null;

            try
            {
                if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
                {
                    jsonRes = new JsonReponse { message = "User name and Password is required.", status = "Failed", redirectURL = "" };
                }
                else if (!string.IsNullOrEmpty(userLogin.Email) && !string.IsNullOrEmpty(userLogin.Password))
                {
                    User userData = db.Users.FirstOrDefault(wh => wh.Email == userLogin.Email & wh.Password == userLogin.Password);

                    if (userData == null)
                    {
                        jsonRes = new JsonReponse { message = "Invalid Credential", status = "Failed", redirectURL = "" };
                    }
                    else
                    {
                        UserDetails userDetails = new UserDetails
                        {
                            Id = userData.Id,
                            Email = userData.Email,
                            FullName = userData.FullName,
                            IsActive = userData.IsActive,
                            CreatedBy = userData.CreatedBy,
                            CreatedOn = userData.CreatedOn,
                            ModifiedOn = userData.ModifiedOn,
                            ModifiedBy = userData.ModifiedBy,
                            ReportingManager = userData.ReportingManager,
                            UserRoles = userData.UserRoles1.Where(wh => wh.UserId == userData.Id).Select(sel => new ClientManager.Models.UserRole { Id = sel.Id, RoleId = sel.RoleId, RoleName = sel.Role.RoleName }).ToList(),
                            ReportingToMe = db.Users.Where(wh => wh.ReportingManager == userData.Id).Select(sel=> sel.Id).ToList()
                        };

                        Session["UserDetails"] = userDetails;

                        jsonRes = new JsonReponse { message = "Valid Credentials", status = "Success", redirectURL = "/Home/MyDashboard" };
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegister(Register userRegister)
        {
            //var currentUser = (UserDetails)Session["UserDetails"];
            JsonReponse jsonRes = null;

            try
            {
                if (string.IsNullOrEmpty(userRegister.Email) || string.IsNullOrEmpty(userRegister.FullName) || string.IsNullOrEmpty(userRegister.Password))
                {
                    jsonRes = new JsonReponse { message = "User name and Password is required.", status = "Failed", redirectURL = "" };
                }
                else if (!string.IsNullOrEmpty(userRegister.Email) && !string.IsNullOrEmpty(userRegister.FullName) && !string.IsNullOrEmpty(userRegister.Password))
                {
                    if (db.Users.Any(wh => wh.Email == userRegister.Email))
                    {
                        jsonRes = new JsonReponse { message = "This Email Id already already registered.", status = "Failed", redirectURL = "" };
                    }
                    else
                    {
                        db.Users.Add(new User { FullName = userRegister.FullName, Email = userRegister.Email, Password = userRegister.Password, IsActive = true, CreatedBy = 1, CreatedOn = DateTime.Now });

                        if (db.SaveChanges() > 0)
                        {
                            jsonRes = new JsonReponse { message = "User registration completed successfully!", status = "Success", redirectURL = "/Account/Login" };
                        }
                        else
                        {
                            jsonRes = new JsonReponse { message = "User registration not completed, try again after sometime.", status = "Failed", redirectURL = "" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                jsonRes = new JsonReponse { message = ex.Message, status = "Error", redirectURL = "" };
            }

            return Json(jsonRes, JsonRequestBehavior.AllowGet);
        }
    }

}