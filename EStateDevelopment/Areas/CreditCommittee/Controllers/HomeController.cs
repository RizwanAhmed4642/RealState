using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.CreditCommittee.Controllers
{
    [Authorize(Roles = "Credit Committee")]
    public class HomeController : Controller
    {
        // GET: CreditCommittee/Home
        public ActionResult Home()
        {
            return View();
        }
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