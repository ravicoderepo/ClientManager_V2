using System.Web.Mvc;

namespace ClientManager.Areas.Admin
{
  public class AdminAreaRegistration : AreaRegistration
  {
    public override string AreaName => "Admin";

    public override void RegisterArea(AreaRegistrationContext context) => context.MapRoute("Admin_default", "Admin/{controller}/{action}/{id}", (object) new
    {
      action = "Index",
      id = UrlParameter.Optional
    });
  }
}
