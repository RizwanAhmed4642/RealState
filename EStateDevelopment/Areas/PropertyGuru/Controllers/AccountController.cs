using EStateDevelopment.Areas.Admin;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.PropertyGuru.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        QIGIEntities db = new QIGIEntities();
        protected EncryptDecrypt encry = new EncryptDecrypt();
        AdminActivity_Logs log = new AdminActivity_Logs();
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(PropertyGuruUser data)
        {
            try
            {
                AspNetUser obj = new AspNetUser();
                obj.UserName = data.firstname + data.lastname;
                obj.firstname = data.firstname;
                obj.lastname = data.lastname;
                obj.Email = data.Email;
                obj.PasswordHash = encry.Encrypt(data.PasswordHash);
                obj.NationalID = data.NationalID;
                obj.phone = data.phone;
                obj.address = data.address;
                obj.state = data.state;
                obj.city = data.city;
                obj.AdminApproval = false;
                obj.AdminRejected = false;
                var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Property Guru").FirstOrDefault();
                if (roles != null)
                {
                    obj.role_id = roles.Id;
                }
                db.AspNetUsers.Add(obj);
                db.SaveChanges();
                log.AddLog("Signup Request", "Property Guru Registered in Application");
                db.SaveChanges();
                ///Sending Email to user
                var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your account has registered successfully, You will be notified after approval of registeration process.</p></div>";


                EmailSend _emailsend = new EmailSend();

                _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                var _dispName = "QIGI Administration";
                MailMessage mymessage = new MailMessage();
                mymessage.To.Add(obj.Email);
                mymessage.From = new MailAddress(_email, _dispName);
                mymessage.Subject = "Thank You For Registration";
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
                TempData["response"] = "Thank You For Sign up, You will received an email shortly.";
                return RedirectToAction("Index", "Home", new { area = "Public" });
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Public" });
            }
        }
    }
}