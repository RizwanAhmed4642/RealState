using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.PropertyGuru.Controllers
{
    [Authorize(Roles = "Property Guru")]
    public class ProductConfigurationController : Controller
    {
        QIGIEntities _db = new QIGIEntities();

        // GET: PropertyGuru/ProductConfiguration
        public ActionResult CollateralType()
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult CollateralType(CollateralType collateralType)
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");


            var pro = _db.Products.Find(collateralType.ProductID);
            collateralType.ProductName = pro.Name;

            if (ModelState.IsValid)
            {
                _db.CollateralTypes.Add(collateralType);
                _db.SaveChanges();
                ModelState.Clear();
            }

            return View();
        }

        public ActionResult ProductChargesType()
        {
            var data = _db.ChargesSlabs.ToList();
            ViewBag.ChargesSlabList = new SelectList(data.ToList(), "ChargesSlabID", "SlabName");

            return View();
        }

        [HttpPost]
        public ActionResult ProductChargesType(ProductChargesType productChargesType)
        {

            var data = _db.ChargesSlabs.ToList();
            ViewBag.ChargesSlabList = new SelectList(data.ToList(), "ChargesSlabID", "SlabName");

            if (ModelState.IsValid)
            {
                _db.ProductChargesTypes.Add(productChargesType);
                _db.SaveChanges();
                ModelState.Clear();
            }

            return View();
        }

        public ActionResult ChargesSlab()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChargesSlab(ChargesSlab chargesSlab)
        {
            if(ModelState.IsValid)
            {
                _db.ChargesSlabs.Add(chargesSlab);
                _db.SaveChanges();
                ModelState.Clear();
            }
           return View();
        }

        public ActionResult ProductCharges()
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            var data_2 = _db.ProductChargesTypes.ToList();
            ViewBag.ProductChargesTypeList = new SelectList(data_2.ToList(), "PChargesTypeID", "ChargesName");

            return View();
        }

        [HttpPost]
        public ActionResult ProductCharges(ProductCharge productCharges)
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            var pro = _db.Products.Find(productCharges.ProductID);
            productCharges.ProductName = pro.Name;

            var data_2 = _db.ProductChargesTypes.ToList();
            ViewBag.ProductChargesTypeList = new SelectList(data_2.ToList(), "PChargesTypeID", "ChargesName");

            if (ModelState.IsValid)
            {
                _db.ProductCharges.Add(productCharges);
                _db.SaveChanges();
                ModelState.Clear();
            }

            return View();
        }

        public ActionResult ProductInvestRateSlab()
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            var data_2 = _db.ProductCharges.ToList();
            var chargestypes = _db.ProductChargesTypes.ToList();
            for (int i = 0; i < data_2.Count(); i++)
            {
                foreach (var item in chargestypes.ToList().Where(x=>x.PChargesTypeID == data_2[i].PChargesTypeID).ToList())
                {
                    data_2[i].ProductChargesType = item;
                }
            }
            ViewBag.ProductChargesList = new SelectList(data_2.ToList(), "PChargesID", "ProductChargesType.ChargesName");

            return View();
        }

        [HttpPost]
        public ActionResult ProductInvestRateSlab(ProductInteresRateSlab productInteresRateSlab)
        {
            var data = _db.Products.ToList();
            ViewBag.ProductList = new SelectList(data.ToList(), "ProductID", "Name");

            var pro = _db.Products.Find(productInteresRateSlab.ProductID);
            productInteresRateSlab.ProductName = pro.Name;


            var data_2 = _db.ProductCharges.ToList();
            var chargestypes = _db.ProductChargesTypes.ToList();
            for (int i = 0; i < data_2.Count(); i++)
            {
                foreach (var item in chargestypes.ToList().Where(x => x.PChargesTypeID == data_2[i].PChargesTypeID).ToList())
                {
                    data_2[i].ProductChargesType = item;
                }
            }
            ViewBag.ProductChargesList = new SelectList(data_2.ToList(), "PChargesID", "ProductChargesType.ChargesName");


            if (ModelState.IsValid)
            {
                _db.ProductInteresRateSlabs.Add(productInteresRateSlab);
                _db.SaveChanges();
                ModelState.Clear();
            }

            return View();
        }
    }
}