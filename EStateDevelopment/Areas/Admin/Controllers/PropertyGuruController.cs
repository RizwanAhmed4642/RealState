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
    public class PropertyGuruController : Controller
    {
        protected QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs log = new AdminActivity_Logs();
        public ActionResult PendingGuru()
        {
            try
            {
                var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Property Guru").FirstOrDefault();
                if (roles != null)
                {
                    var list = db.AspNetUsers.ToList().Where(x => x.AdminApproval == false && x.AdminRejected == false && x.role_id == roles.Id).ToList();
                    return View(list);
                }
                List<AspNetUser> obj = new List<AspNetUser>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }


        public ActionResult ApproveGuru(int id)
        {
            try
            {
                var user = db.AspNetUsers.Find(id);
                if (user != null)
                {
                    user.AdminApproval = true;
                    user.AdminRejected = false;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    log.AddLog("Approved Guru", "Approved Guru in Application");
                    db.SaveChanges();

                    var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your account has approved, you can login now with your email and password.</p></div>";
                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "QIGI Administration";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(user.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "Registered Account Approved";
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

                }
                return RedirectToAction("ApprovedGuru", "PropertyGuru", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult RejectGuru(int id)
        {
            try
            {
                var user = db.AspNetUsers.Find(id);
                if (user != null)
                {
                    user.AdminApproval = false;
                    user.AdminRejected = true;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    log.AddLog("Reject Guru", "Rejected Guru in Application");
                    db.SaveChanges();

                    var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your account has rejected by administrator, Please contact administration.</p></div>";
                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "QIGI Administration";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(user.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "Registered Account Rejected";
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

                }
                return RedirectToAction("RejecteddGuru", "PropertyGuru", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult ApprovedGuru()
        {
            try
            {
                var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Property Guru").FirstOrDefault();
                if (roles != null)
                {
                    var list = db.AspNetUsers.ToList().Where(x => x.AdminApproval == true && x.AdminRejected == false && x.role_id == roles.Id).ToList();
                    return View(list);
                }
                List<AspNetUser> obj = new List<AspNetUser>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }

        public ActionResult RejectedGuru()
        {
            try
            {
                var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Property Guru").FirstOrDefault();
                if (roles != null)
                {
                    var list = db.AspNetUsers.ToList().Where(x => x.AdminApproval == false && x.AdminRejected == true && x.role_id == roles.Id).ToList();
                    return View(list);
                }
                List<AspNetUser> obj = new List<AspNetUser>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
        }
    }
}