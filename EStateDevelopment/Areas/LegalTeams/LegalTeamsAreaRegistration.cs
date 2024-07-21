using System.Web.Mvc;

namespace EStateDevelopment.Areas.LegalTeams
{
    public class LegalTeamsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LegalTeams";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LegalTeams_default",
                "LegalTeams/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}