using System.Web.Mvc;

namespace EStateDevelopment.Areas.CreditCommittee
{
    public class CreditCommitteeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CreditCommittee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CreditCommittee_default",
                "CreditCommittee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}