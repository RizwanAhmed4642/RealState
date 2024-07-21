using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Admin.Controllers
{
    public class ApplicationsController : Controller
    {
        QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs AAL = new AdminActivity_Logs();
        protected EncryptDecrypt encry = new EncryptDecrypt();
        public ActionResult ApprovedInvApplications()
        {
            var products = db.Products.ToList();

            var investorapp = db.InvestorApplications.ToList().Where(Ia => Ia.ApprovalStatus == true && Ia.RejectedStatus == false).ToList();
            var users = db.AspNetUsers.ToList();

            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var items in users.ToList().Where(u => u.Id == products[i].UserID).ToList())
                {
                    products[i].AspNetUsers = new List<AspNetUser>();
                    products[i].AspNetUsers.Add(items);
                }

                products[i].InvestorApplications = new List<InvestorApplication>();
                foreach (var item in investorapp.ToList().Where(ia => ia.ProductID == products[i].ProductID).ToList())
                {

                    products[i].InvestorApplications.Add(item);
                }
            }


            return View(products);
        }

        public ActionResult RejectedInvApplications()
        {
            var products = db.Products.ToList();

            var investorapp = db.InvestorApplications.ToList().Where(Ia => Ia.ApprovalStatus == false && Ia.RejectedStatus == true).ToList();
            var users = db.AspNetUsers.ToList();

            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var items in users.ToList().Where(u => u.Id == products[i].UserID).ToList())
                {
                    products[i].AspNetUsers = new List<AspNetUser>();
                    products[i].AspNetUsers.Add(items);
                }

                products[i].InvestorApplications = new List<InvestorApplication>();
                foreach (var item in investorapp.ToList().Where(ia => ia.ProductID == products[i].ProductID).ToList())
                {

                    products[i].InvestorApplications.Add(item);
                }
            }

            return View(products);
        }

        public ActionResult InvestorApplications()
        {
            var products = db.Products.ToList();

            var investorapp = db.InvestorApplications.ToList().Where(x=>x.ApprovalStatus == null && x.RejectedStatus == null).ToList();
            var users = db.AspNetUsers.ToList();

            for (int i = 0; i < products.Count(); i++)
            {
                foreach (var items in users.ToList().Where(u => u.Id == products[i].UserID).ToList())
                {
                    products[i].AspNetUsers = new List<AspNetUser>();
                    products[i].AspNetUsers.Add(items);
                }

                products[i].InvestorApplications = new List<InvestorApplication>();
                foreach (var item in investorapp.ToList().Where(ia => ia.ProductID == products[i].ProductID).ToList())
                {

                    products[i].InvestorApplications.Add(item);
                }
            }

            return View(products);
        }

        public ActionResult InvApplicationDetails(int id, int appID, string appnumber)
        {
            var products = db.Products.Find(id);

            var users = db.AspNetUsers.ToList();
            var investorapp = db.InvestorApplications.ToList().Where(x => x.ProductID == id && x.InvestorApplicationID == appID && x.ApplicationNumber == Convert.ToString(appnumber)).ToList();

            foreach (var items in users.ToList().Where(u => u.Id == products.UserID).ToList())
            {
                products.AspNetUsers = new List<AspNetUser>();
                products.AspNetUsers.Add(items);
            }

            products.InvestorApplications = new List<InvestorApplication>();
            foreach (var items in investorapp.Where(ia => ia.ProductID == products.ProductID && ia.InvestorApplicationID == appID))
            {
                products.InvestorApplications.Add(items);
            }

            return View(products);
        }

        public ActionResult ApproveInvapp(int id)
        {
            var invapp = db.InvestorApplications.Find(id);
            invapp.ApprovalStatus = true;
            invapp.RejectedStatus = false;
            db.Entry(invapp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("InvestorApplications");
        }

        public ActionResult RejectInvapp(int id)
        {
            var invapp = db.InvestorApplications.Find(id);
            invapp.RejectedStatus = true;
            invapp.ApprovalStatus = false;
            db.Entry(invapp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("InvestorApplications");
        }


        public ActionResult BorrowerApplication()
        {
            var products = db.Products.ToList();
            var borrowerapp = db.BorrowerApplications.ToList();

            for (int i = 0; i < products.Count(); i++)
            {
                products[i].BorrowerApplications = new List<BorrowerApplication>();
                foreach (var item in borrowerapp.ToList().Where(ba => ba.ProductID == products[i].ProductID).ToList())
                {
                    products[i].BorrowerApplications.Add(item);
                }
            }

            return View(products);
        }

        public ActionResult BoApplicationDetails(int id, int bappID, string bAppNum)
        {
            var products = db.Products.Find(id);

            var users = db.AspNetUsers.ToList();

            var borapp = db.BorrowerApplications.ToList().Where(x => x.ProductID == id && x.BorrowerApplicationID == bappID && x.ApplicationNumber == Convert.ToString(bAppNum)).ToList();

            foreach (var items in users.ToList().Where(u => u.Id == products.UserID).ToList())
            {
                products.AspNetUsers = new List<AspNetUser>();
                products.AspNetUsers.Add(items);
            }

            products.BorrowerApplications = new List<BorrowerApplication>();
            foreach (var items in borapp.Where(ia => ia.ProductID == products.ProductID && ia.BorrowerApplicationID == bappID))
            {
                products.BorrowerApplications.Add(items);
            }

            return View(products);
        }


        public ActionResult ApproveBapp(int id)
        {
            var borapp = db.BorrowerApplications.Find(id);
            borapp.ApprovalStatus = true;
            borapp.RejectedStatus = false;
            db.Entry(borapp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("BorrowerApplication", "Configuration");
        }

        public ActionResult RejectBapp(int id)
        {
            var borapp = db.BorrowerApplications.Find(id);
            borapp.RejectedStatus = true;
            borapp.ApprovalStatus = false;
            db.Entry(borapp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("BorrowerApplication", "Configuration");
        }


    }
}