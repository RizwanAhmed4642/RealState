using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.PropertyGuru.Controllers
{
    [Authorize(Roles = "Property Guru")]
    public class SecurityTypeController : Controller
    {

        QIGIEntities _db = new QIGIEntities();

        // GET: PropertyGuru/SecurityType
        public ActionResult security()
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult security(SecurityType securityType)
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            var pro = _db.Products.Find(securityType.ProductID);
            securityType.ProductName = pro.Name;

            if (ModelState.IsValid)
            {
                _db.SecurityTypes.Add(securityType);
                _db.SaveChanges();
                ModelState.Clear();
            }

            return View();
        }
    }
}