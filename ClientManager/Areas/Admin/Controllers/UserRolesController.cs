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
  public class UserRolesController : Controller
  {
    private ClientManagerEntities db = new ClientManagerEntities();

    [CustomAuthorize(new string[] {"Admin"})]
    public ActionResult Index() => (ActionResult) this.View((object) this.db.UserRoles.Include<DBOperation.UserRole, Role>((Expression<Func<DBOperation.UserRole, Role>>) (u => u.Role)).Include<DBOperation.UserRole, User>((Expression<Func<DBOperation.UserRole, User>>) (u => u.User)).Include<DBOperation.UserRole, User>((Expression<Func<DBOperation.UserRole, User>>) (u => u.User1)).Include<DBOperation.UserRole, User>((Expression<Func<DBOperation.UserRole, User>>) (u => u.User2)).Include<DBOperation.UserRole, User>((Expression<Func<DBOperation.UserRole, User>>) (u => u.User3)).ToList<DBOperation.UserRole>());

    public ActionResult Details(int? id)
    {
      if (!id.HasValue)
        return (ActionResult) new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      DBOperation.UserRole model = this.db.UserRoles.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [CustomAuthorize("Admin")]
    public ActionResult Create(int userId = 1)
    {
      var list = this.db.UserRoles.Where<DBOperation.UserRole>((Expression<Func<DBOperation.UserRole, bool>>) (wh => wh.UserId == userId)).Select(sel => new
      {
        RoleId = sel.RoleId,
        RoleName = sel.Role.RoleName
      }).ToList();

      IEnumerable<string> assignedRoleNames = list.Select(sel => sel.RoleName);
     
      ViewBag.AvailableRoles = new SelectList((IEnumerable) list, "RoleId", "RoleName");
     
      ViewBag.AssignedRoles = new SelectList(this.db.Roles.Where<Role>((Expression<Func<Role, bool>>) (wh => !assignedRoleNames.Contains<string>(wh.RoleName))), "Id", "RoleName");
     
      ViewBag.Users = new SelectList(this.db.Users, "Id", "FullName", (object) userId);
      return (ActionResult) this.View();
    }

    [HttpPost]
    [CustomAuthorize("Admin")]
    public ActionResult Create(UserRoleData userRoleData)
    {
      JsonReponse jsonReponse = (JsonReponse) null;
      JsonReponse data;
      try
      {
        UserDetails userDetails = (UserDetails) this.Session["UserDetails"];
        string str = "";
        int num = 0;
        if (userRoleData.UserId <= 0 || userRoleData.SelectedRoles == null)
          jsonReponse = new JsonReponse()
          {
            message = "Enter all required fields.",
            status = "Failed",
            redirectURL = ""
          };
        else if (userDetails.UserRoles.Any<ClientManager.Models.UserRole>((Func<ClientManager.Models.UserRole, bool>) (wh => wh.RoleName.ToLower() == "admin")))
        {
          this.db.UserRoles.RemoveRange((IEnumerable<DBOperation.UserRole>) this.db.UserRoles.Where<DBOperation.UserRole>((Expression<Func<DBOperation.UserRole, bool>>) (wh => wh.UserId == userRoleData.UserId)));
          int index = 0;
          foreach (int selectedRole in userRoleData.SelectedRoles)
          {
            this.db.UserRoles.Add(new DBOperation.UserRole()
            {
              UserId = userRoleData.UserId,
              RoleId = userRoleData.SelectedRoles[index],
              CreatedOn = DateTime.Now,
              CreatedBy = new int?(userDetails.Id)
            });
            ++index;
          }
          str = "User Roles updated ";
          num = this.db.SaveChanges();
        }
        if (num > 0)
          data = new JsonReponse()
          {
            message = str + " successfully!",
            status = "Success",
            redirectURL = "/Admin/UserRoles/Create"
          };
        else
          data = new JsonReponse()
          {
            message = str + " not completed, try again after sometime.",
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
      return (ActionResult) this.Json((object) data, JsonRequestBehavior.AllowGet);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
        this.db.Dispose();
      base.Dispose(disposing);
    }
  }
}
