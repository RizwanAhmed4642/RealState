using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace EStateDevelopment.Areas.PropertyGuru.Controllers
{
    [Authorize(Roles = "Property Guru")]
    public class DashboardController : Controller
    {
        // GET: Property_Guru/Dashboard
        public ActionResult Home()
        {
            return View();
        }

        
    }
}