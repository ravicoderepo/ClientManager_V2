using ClientManager.Models;
using DBOperation;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClientManager.Infrastructure
{

    //https://www.c-sharpcorner.com/article/custom-authorization-filter-in-mvc-with-an-example/
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        private ClientManagerEntities db = new ClientManagerEntities();
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            UserDetails userDetails = (UserDetails)httpContext.Session["UserDetails"];

            if (userDetails != null)
            {
                if (userDetails.Id > 0)
                {
                    var userRole = (from u in db.Users
                                    join r in db.UserRoles on u.Id equals r.UserId
                                    where u.Id == userDetails.Id
                                    select new
                                    {
                                        r.Role.RoleName
                                    }).FirstOrDefault();

                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.RoleName) return true;
                    }
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "NotAuthorized" }
               });
        }
    }
}
