using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Admin.Controllers
{
    [Authorize(Roles = "QIGIAdmin, Admin,Administrator")]
    public class ProductsController : Controller
    {
        protected QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs log = new AdminActivity_Logs();
        public ActionResult PendingProducts()
        {
            try
            {
                var products = db.Products.ToList().Where(x => x.ApprovalStatus == false && x.RejectedStatus == false).ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult ApprovedProducts()
        {
            try
            {
                var products = db.Products.ToList().Where(x => x.ApprovalStatus == true && x.RejectedStatus == false && x.Status == true).ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult RejectedProducts()
        {
            try
            {
                var products = db.Products.ToList().Where(x => x.ApprovalStatus == false && x.RejectedStatus == true && x.Status == false).ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult Approved(int id)
        {
            try
            {
                var products = db.Products.Find(id);
                products.ApprovalStatus = true;
                products.Status = true;
                products.RejectedBy = User.Identity.Name.ToString();
                db.Entry(products).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                log.AddLog("Approved", "Approved Product in Application");
                db.SaveChanges();



                var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your Product" + products.Name + "has Rejected by Administrator.</p></div>";
                EmailSend _emailsend = new EmailSend();

                _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                var _dispName = "QIGI Administration";
                MailMessage mymessage = new MailMessage();
                mymessage.To.Add(products.UserEmail);
                mymessage.From = new MailAddress(_email, _dispName);
                mymessage.Subject = "Product Rejected";
                mymessage.Body = _msgForAdmin;
                mymessage.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = ConfigurationManager.AppSettings["host"];
                    smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_email, _pass);

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(mymessage);

                }
                return RedirectToAction("ApprovedProducts", "Products");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult Reject(int id)
        {
            try
            {
                var products = db.Products.Find(id);
                products.RejectedStatus = true;
                products.Status = false;
                products.ImagePath = null;
                products.ApprovalStatus = false;
                products.RejectedBy = User.Identity.Name.ToString();
                db.Entry(products).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                log.AddLog("Rejected", "Rejected Product in Application");
                db.SaveChanges();
                ///Sending Email to user
           

                var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your Product" + products.Name + "has Rejected by Administrator.</p></div>";
                EmailSend _emailsend = new EmailSend();

                _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                var _dispName = "QIGI Administration";
                MailMessage mymessage = new MailMessage();
                mymessage.To.Add(products.UserEmail);
                mymessage.From = new MailAddress(_email, _dispName);
                mymessage.Subject = "Product Rejected";
                mymessage.Body = _msgForAdmin;
                mymessage.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = ConfigurationManager.AppSettings["host"];
                    smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTP_Port"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(_email, _pass);

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.Send(mymessage);

                }
                return RedirectToAction("RejectedProducts", "Configuration");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                List<ApplicationModel> applications = new List<ApplicationModel>();
                var users = db.AspNetUsers.ToList();
                var charges = db.ChargesSlabs.ToList();
                var chargesType = db.ProductChargesTypes.ToList();
                var Pcharges = db.ProductCharges.ToList();


                var products = db.Products.Find(id);

                for (int i = 0; i < chargesType.Count(); i++)
                {
                    foreach (var item in charges.ToList().Where(x => x.ChargesSlabID == chargesType[i].ChargesSlabID).ToList())
                    {
                        chargesType[i].ChargesSlabs = new List<ChargesSlab>();
                        chargesType[i].ChargesSlabs.Add(item);
                    }
                }

                for (int i = 0; i < Pcharges.Count(); i++)
                {
                    foreach (var items in chargesType.ToList().Where(c => c.PChargesTypeID == Pcharges[i].PChargesID).ToList())
                    {
                        Pcharges[i].ProductChargesType = items;
                    }
                }



                foreach (var items in Pcharges.ToList().Where(p => p.ProductID == products.ProductID).ToList())
                {
                    products.ProductCharges = new List<ProductCharge>();
                    products.ProductCharges.Add(items);
                }

                foreach (var items in users.ToList().Where(u => u.Id == products.UserID).ToList())
                {
                    products.AspNetUsers = new List<AspNetUser>();
                    products.AspNetUsers.Add(items);
                }


                var borrowerapp = db.BorrowerApplications.ToList();
                var investorapp = db.InvestorApplications.ToList();
                for (int i = 0; i < borrowerapp.Count(); i++)
                {
                    var product = db.Products.Find(borrowerapp[i].ProductID);
                    if (product != null)
                    {
                        ApplicationModel obj = new ApplicationModel();
                        obj.Name = borrowerapp[i].FullName;
                        obj.ApplicationID = borrowerapp[i].BorrowerApplicationID;
                        obj.ProductName = product.Name;
                        obj.RateofReturn = product.RateofReturn;
                        obj.Phone = borrowerapp[i].Phone;
                        obj.Email = borrowerapp[i].Email;
                        obj.ProductID = (int)borrowerapp[i].ProductID;
                        obj.personType = "Borrower";
                        applications.Add(obj);
                    }
                }

                foreach (InvestorApplication i in investorapp)
                {
                    var product = db.Products.Find(i.ProductID);
                    if (product != null)
                    {
                        applications.Add(new ApplicationModel()
                        {
                            Name = i.FullName,
                            ApplicationID = i.InvestorApplicationID,
                            ProductName = product.Name,
                            RateofReturn = product.RateofReturn,
                            Phone = i.Phone,
                            Email = i.Email,
                            ProductID = (int)i.ProductID,
                            personType = "Investor",
                        });
                    }

                }

                products.ApplicationModels = new List<ApplicationModel>();
                foreach (var items in applications.Where(a => a.ProductID == products.ProductID))
                {

                    products.ApplicationModels.Add(items);
                }


                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }
    }
}