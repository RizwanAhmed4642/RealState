using System.Web.Mvc;

namespace EStateDevelopment.Areas.PropertyGuru
{
    public class PropertyGuruAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PropertyGuru";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PropertyGuru_default",
                "PropertyGuru/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}