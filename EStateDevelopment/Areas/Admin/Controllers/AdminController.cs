using EStateDevelopment.Areas.Admin.ViewModel;
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
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs AAL = new AdminActivity_Logs();
        protected EncryptDecrypt encry = new EncryptDecrypt();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ExistRole = db.AspNetRoles.ToList().Exists(x => x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase));

                    if (ExistRole == false)
                    {

                        AspNetRole _entity = new AspNetRole();
                        _entity.Name = model.Name;
                        _entity.Status = true;
                        db.AspNetRoles.Add(_entity);
                        db.SaveChanges();
                        AAL.AddLog("Add", "Add Role in Application");
                        TempData["response"] = "Role Add Successfully.";
                        return RedirectToAction("ListRole", "Admin");
                    }
                    else
                    {
                        TempData["response"] = "Same Record Already Exists !";
                        return RedirectToAction("AddRole", "Admin");
                    }


                }
                catch (Exception ex)
                {

                    var massage = ex.Message;
                }
            }



            return View();
        }
        public ActionResult ListRole()
        {
            var RoleList = db.AspNetRoles.ToList();

            return View(RoleList);
        }
        public ActionResult DeleteRoles(int id)
        {
            try
            {
                var Role = db.AspNetRoles.Find(id);
                if (Role != null)
                {
                    db.AspNetRoles.Remove(Role);

                    db.SaveChanges();
                    AAL.AddLog("Delete", "Delete Role in Application");
                    TempData["response"] = "Role Deleted Successfully !";
                    return RedirectToAction("ListRole", "Admin");
                }
                else
                {
                    TempData["response"] = "Role Delete Failed !";
                    return RedirectToAction("ListRoles", "Admin");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListRole", "Admin");
            }
        }
        public ActionResult UpdateRoles(int id)
        {
            try
            {
                var Role = db.AspNetRoles.Find(id);
                RoleViewModel model = new RoleViewModel();
                if (Role != null)
                {
                    model.Id = Role.Id;
                    model.Name = Role.Name;
                    model.Status = Role.Status;
                    return View(model);
                }
                else
                {
                    TempData["response"] = "Role Not Found !";
                    return RedirectToAction("ListRole", "Admin");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListRole", "Admin");
            }
        }

        [HttpPost]
        public ActionResult UpdateRoles(RoleViewModel model, int id)
        {
            try
            {
                var ExistRole = db.AspNetRoles.ToList().Exists(x => x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != id);
                if (ExistRole == false)
                {
                    var _entity = db.AspNetRoles.Find(id);
                    if (_entity != null)
                    {
                        _entity.Name = model.Name;
                        _entity.Status = model.Status;
                        db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                        AAL.AddLog("Update", "Update Role in Application");
                        TempData["response"] = "Role Updated Successfully !";
                        return RedirectToAction("ListRole", "Admin");
                    }
                    else
                    {
                        TempData["response"] = "Role Update Failed !";
                        return RedirectToAction("ListRole", "Admin");
                    }
                }
                else
                {
                    TempData["response"] = "Same Record Already Exists !";
                    return RedirectToAction("UpdateRoles", "Admin");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListRole", "Admin");
            }
        }
        public ActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();
            var _Rolelist = db.AspNetRoles.ToList();
            ViewBag.RoleId = new SelectList(_Rolelist.ToList(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUser(UserViewModel model)
        {

            try
            {
                AspNetUser user = new AspNetUser();
                var isExsist = db.AspNetUsers.ToList().Where(x => x.Email == model.Email).ToList();
                if (isExsist.Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        user.is_active = true;
                        user.created_date = DateTime.Now;
                        user.UserName = model.firstname + " " + model.lastname;
                        user.PasswordHash = encry.Encrypt(model.PasswordHash);
                        user.firstname = model.firstname;
                        user.lastname = model.lastname;
                        user.Email = model.Email;
                        user.state = model.state;
                        user.address = model.address;
                        user.city = model.city;
                        user.phone = model.mobile;
                        user.NationalID = model.NationalID;
                        user.role_id = model.role_id;
                        user.AdminApproval = true;
                        user.AdminRejected = false;
                        var currentuser = db.AspNetUsers.Add(user);
                        db.SaveChanges();
                        AspNetUserRole userroles = new AspNetUserRole();
                        userroles.Roles_id = currentuser.role_id;
                        var userRoles = db.AspNetRoles.Where(x => x.Id == currentuser.role_id).FirstOrDefault();
                        userroles.RoleName = userRoles.Name;
                        var users = db.AspNetUsers.Find(currentuser.Id);
                        userroles.User_id = currentuser.Id;
                        userroles.UserName = users.UserName;
                        db.AspNetUserRoles.Add(userroles);
                        AAL.AddLog("Save", "Save User in Application");
                        db.SaveChanges();
                        TempData["response"] = "User Registered Successfully.";
                        return RedirectToAction("ListUser", "Admin");
                    }
                    else
                    {
                        TempData["response"] = "Invalid Model " + ModelState.Values;
                        return RedirectToAction("AddUser", "Admin");
                    }
                }
                else
                {
                    TempData["response"] = "Email Already Exists !";
                    return RedirectToAction("AddUser", "Admin");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("AddUser", "Admin");

            }
        }
        /// <summary>
        /// This function show the list of user with role name.
        ///  Auther Rizwan Ahmed.
        /// </summary>
        /// <returns></returns>
        public ActionResult ListUser()
        {
            var _resultModel = new List<UserViewModel>();

            var _allusers = db.AspNetUsers.ToList();
            foreach (var item in _allusers)
            {
                var _model = new UserViewModel();

                _model.Id = item.Id;
                _model.firstname = item.firstname;
                _model.lastname = item.lastname;
                _model.Email = item.Email;
                _model.state = item.state;
                _model.address = item.address;
                _model.city = item.city;
                _model.phone = item.mobile;
                _model.NationalID = item.NationalID;
                _model.role_id = item.role_id;
                _resultModel.Add(_model);


            }
            var _allrole = db.AspNetRoles.ToList();
            if (_resultModel.Count > 0)
            {
                for (int i = 0; i < _resultModel.Count(); i++)
                {
                    foreach (var item in _allrole.ToList().Where(x => x.Id == _resultModel[i].role_id))
                    {
                        _resultModel[i].RoleName = item.Name;
                    }
                }
            }
            return View(_resultModel);

        }
        /// <summary>
        /// This function Delete user .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var user = db.AspNetUsers.Find(id);
                if (user != null)
                {
                    db.AspNetUsers.Remove(user);
                    db.SaveChanges();
                    AAL.AddLog("Delete", "Delete User from Application");
                    TempData["response"] = "User Deleted Successfully !";
                    return RedirectToAction("ListUser", "Admin");
                }
                else
                {
                    TempData["response"] = "User Delete Failed !";
                    return RedirectToAction("ListUser", "Admin");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListUser", "Admin");
            }
        }

        public ActionResult BlockUserList()
        {
            var _resultModel = new List<UserViewModel>();

            var _allusers = db.AspNetUsers.ToList();
            foreach (var item in _allusers)
            {
                var _model = new UserViewModel();

                _model.Id = item.Id;
                _model.is_active = item.is_active;

                _model.firstname = item.firstname;
                _model.lastname = item.lastname;
                _model.Email = item.Email;
                _model.state = item.state;
                _model.address = item.address;
                _model.city = item.city;
                _model.phone = item.mobile;
                _model.NationalID = item.NationalID;
                _model.role_id = item.role_id;
                _resultModel.Add(_model);


            }
            var _allrole = db.AspNetRoles.ToList();
            if (_resultModel.Count > 0)
            {
                for (int i = 0; i < _resultModel.Count(); i++)
                {
                    foreach (var item in _allrole.ToList().Where(x => x.Id == _resultModel[i].role_id))
                    {
                        _resultModel[i].RoleName = item.Name;
                    }
                }
            }
            return View(_resultModel);

        }

        public ActionResult UnBlockUser(int id)
        {
            try
            {

                var _entity = db.AspNetUsers.Find(id);
                if (_entity != null)
                {
                    _entity.is_active = true;

                    db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    AAL.AddLog("UnBlock", "User UnBlock in Application");

                    var _msgForAdmin = "<div><h3>Dear User,</h3><br/><p> Your account has UnBlocked successfully.</p></div>";


                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "QIGI Administration";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(_entity.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "User UnBlock in Application";
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

                    TempData["response"] = "User UnBlock  Successfully !";
                    return RedirectToAction("BlockUserList");
                }
                else
                {
                    TempData["response"] = "User Block Failed !";
                    return RedirectToAction("BlockUserList");
                }


            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("BlockUserList");
            }
        }
        public ActionResult BlockUser(int id)
        {
            try
            {

                var _entity = db.AspNetUsers.Find(id);
                if (_entity != null)
                {
                    _entity.is_active = false;

                    db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    AAL.AddLog("Block", "User Block in Application");
                    var _msgForAdmin = "<div><h3>Dear User,</h3><br/><p> Your account has Blocked In Application.</p></div>";


                    EmailSend _emailsend = new EmailSend();

                    _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                    var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                    var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                    var _dispName = "QIGI Administration";
                    MailMessage mymessage = new MailMessage();
                    mymessage.To.Add(_entity.Email);
                    mymessage.From = new MailAddress(_email, _dispName);
                    mymessage.Subject = "User Block in Application";
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


                    TempData["response"] = "User Block  Successfully !";
                    return RedirectToAction("BlockUserList");
                }
                else
                {
                    TempData["response"] = "User Block Failed !";
                    return RedirectToAction("BlockUserList");
                }


            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("BlockUserList");
            }
        }


        public ActionResult UserProfile()
        {
            return View();
        }
        public ActionResult Approved(int id)
        {
            var products = db.Products.Find(id);
            products.ApprovalStatus = true;
            db.Entry(products).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var user = db.AspNetUsers.Where(x => x.Email == products.UserEmail).FirstOrDefault();
            var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your Product"+ products.Name + "has Approved by Administrator.</p></div>";
            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "QIGI Administration";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(user.Email);
            mymessage.From = new MailAddress(_email, _dispName);
            mymessage.Subject = "Product Approved";
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



            return RedirectToAction("ListProducts", "Admin");
        }

        public ActionResult Reject(int id)
        {
            var products = db.Products.Find(id);
            products.RejectedStatus = true;
            db.Entry(products).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var user = db.AspNetUsers.Where(x => x.Email == products.UserEmail).FirstOrDefault();
  
            var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your Product" + products.Name + "has Rejected by Administrator.</p></div>";
            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "QIGI Administration";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(user.Email);
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

            return RedirectToAction("ListProducts", "Admin");
        }

        public ActionResult Details(int id)
        {
            var products = db.Products.Find(id);
            return View(products);
        }
        public ActionResult PPGuruusers()
        {
            var roles = db.AspNetRoles.ToList().Where(r => r.Name == "PropertyGuru").FirstOrDefault();
            var user = db.AspNetUsers.ToList().Where(u => u.role_id == roles.Id && u.AdminApproval == false && u.AdminRejected == false);

            return View(user);
        }

        public ActionResult ApproveGuru(int id)
        {
            var users = db.AspNetUsers.Find(id);
            users.AdminApproval = true;
            users.AdminRejected = false;
            db.Entry(users).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p> Your account has Approved successfully.</p></div>";


            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "QIGI Administration";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(users.Email);
            mymessage.From = new MailAddress(_email, _dispName);
            mymessage.Subject = "Property Guru Approve in Application";
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

            return RedirectToAction("PPGuruusers", "Admin");
        }

        public ActionResult RemoveGuru(int id)
        {
            var users = db.AspNetUsers.Find(id);
            users.AdminRejected = true;
            users.AdminApproval = false;

            db.Entry(users).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var _msgForAdmin = "<div><h3>Dear Property Guru,</h3><br/><p>Oops! Your account has been Removed</p></div>";
            EmailSend _emailsend = new EmailSend();

            _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

            var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
            var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
            var _dispName = "QIGI Administration";
            MailMessage mymessage = new MailMessage();
            mymessage.To.Add(users.Email);
            mymessage.From = new MailAddress(_email, _dispName);
            mymessage.Subject = "Property Guru Remove";
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

            return RedirectToAction("PPGuruusers", "Admin");
        }
    }
}