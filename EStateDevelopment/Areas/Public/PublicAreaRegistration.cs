using System.Web.Mvc;

namespace EStateDevelopment.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Public";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //name: "Public_Default",
            //url: "Public/{controller}/{action}/{id}",
            //defaults: new
            //{
            //    area = "Public",
            //    controller = "Home",
            //    action = "Index",
            //    id = UrlParameter.Optional
            //});
            context.MapRoute(
                "Public_default",
                "Public/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}