using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Admin.Controllers
{
    public class InvestorController : Controller
    {
        protected QIGIEntities db = new QIGIEntities();

        public ActionResult AllInvestor()
        {
            try
            {
                var roles = db.AspNetRoles.ToList().Where(r => r.Name == "Investor").FirstOrDefault();
                if (roles != null)
                {
                    var Inv = db.AspNetUsers.ToList().Where(i=> i.role_id == roles.Id); 
                    //.Where(i => i.AdminApproval == false && i.AdminRejected == false &&
                  
                    return View(Inv);
                }
                else
                {
                    var InvList = new List<AspNetUser>()
                    {
                        // Investors 
                    };

                    return View(InvList);
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        //public ActionResult Approved()
        //{

        //    try
        //    {
        //        var roles = db.AspNetRoles.ToList().Where(r => r.Name == "Investor").FirstOrDefault();
        //        if (roles != null)
        //        {
        //            var Inv = db.AspNetUsers.ToList().Where(i => i.AdminApproval == true && i.AdminRejected == false &&
        //            i.role_id == roles.Id);
        //            return View(Inv);
        //        }
        //        else
        //        {
        //            var InvList = new List<AspNetUser>()
        //                {
        //                     Investors
        //                };

        //            return View(InvList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["response"] = "Oops Error " + ex.Message;
        //        return RedirectToAction("Index", "Admin", new { area = "Admin" });
        //    }
        //}

        //public ActionResult Rejected()
        //{
        //    try
        //    {
        //        var roles = db.AspNetRoles.ToList().Where(r => r.Name == "Investor").FirstOrDefault();
        //        if (roles != null)
        //        {
        //            var Inv = db.AspNetUsers.ToList().Where(i => i.AdminApproval == false && i.AdminRejected == true &&
        //            i.role_id == roles.Id);
        //            return View(Inv);
        //        }
        //        else
        //        {
        //            var InvList = new List<AspNetUser>()
        //                {
        //                     Investors
        //                };

        //            return View(InvList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["response"] = "Oops Error " + ex.Message;
        //        return RedirectToAction("Index", "Admin", new { area = "Admin" });
        //    }
        //}

        public ActionResult Applications(int? id)
        {
            try
            {
                var user = db.AspNetUsers.Find(id);
                var products = db.Products.ToList();
                var investorapp = db.InvestorApplications.ToList().Where(ia => ia.Email == user.Email);

                for (int i = 0; i < products.Count(); i++)
                {

                    products[i].AspNetUsers = new List<AspNetUser>();
                    products[i].AspNetUsers.Add(user);


                    products[i].InvestorApplications = new List<InvestorApplication>();
                    foreach (var item in investorapp.ToList().Where(ia => ia.ProductID == products[i].ProductID).ToList())
                    {

                        products[i].InvestorApplications.Add(item);
                    }
                }

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult Qualification(int? id)
        {
            try
            {
                var user = db.AspNetUsers.Find(id);
                var invqualification = db.InvestorQualifications.Where(q => q.InvestorID == user.Id).FirstOrDefault();
                if(invqualification == null)
                {
                    return HttpNotFound();
                }
                return View(invqualification);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult InvUserDetail(int? id)
        {
            try
            {
                var roles = db.AspNetRoles.ToList().Where(r => r.Name == "Investor").FirstOrDefault();
                var user = db.AspNetUsers.Where(u => u.Id==id  || u.role_id == roles.Id ).FirstOrDefault();

                Session["InvRole"] = roles.Name;

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }


    }
}