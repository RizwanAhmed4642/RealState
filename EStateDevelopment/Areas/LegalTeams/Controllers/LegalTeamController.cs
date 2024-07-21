using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.LegalTeams.Controllers
{
    [Authorize(Roles = "Legal Team")]
    public class LegalTeamController : Controller
    {
        // GET: LegalTeams/LegalTeam
        public ActionResult Dashboard()
        {
            return View();
        }  
        public ActionResult Correspondance()
        {
            return View();
        }
    }
}