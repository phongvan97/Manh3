using System.Web.Mvc;

namespace LamDep.Areas.Identity
{
    public class IdentityAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Identity";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Identity_default",
                "Identity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "LamDep.Areas.Identity.Controllers" }
            );
        }
    }
}