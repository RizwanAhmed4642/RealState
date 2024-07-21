using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.AccountManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: AccountManager/Home
        public ActionResult LendingDashboard()
        {
            return View();
        }

        public ActionResult Correspondance()
        {
            return View();
        }

        public ActionResult Verification()
        {
            return View();
        }

        public ActionResult ContractDrafting()
        {
            return View();
        }

        public ActionResult FundsAvailibilityRequest()
        {
            return View();
        }

        public ActionResult CustomerIDCreation()
        {
            return View();
        }
    }
}