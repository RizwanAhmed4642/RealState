using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EStateDevelopment.Areas.Public.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly QIGIEntities db = new QIGIEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AspNetUser data)
        {
            try
            {
                if (Membership.ValidateUser(data.Email, data.PasswordHash))
                {
                    FormsAuthentication.SetAuthCookie(data.Email, false);
                    var currentRole = Roles.GetRolesForUser();
                    if (data.Email == "admin@qigi.com" || User.IsInRole("QIGIAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
                    {
                        Session["RoleName"] = "Admin";
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });

                    }
                    else
                    {
                        var currentuser = db.AspNetUsers.ToList().Where(x => x.Email == data.Email && x.AdminApproval == true).FirstOrDefault();
                        if (currentuser != null)
                        {
                            if (currentuser.is_active != false)
                            {
                                var role = db.AspNetRoles.Find(currentuser.role_id);
                                Session["RoleName"] = role.Name;
                                Session["UserId"] = currentuser.Id;
                                if (role != null)
                                {
                                    if (role.Name == "Account Manager")
                                    {
                                    
                                        return RedirectToAction("LendingDashboard", "Home", new { area = "AccountManager" });

                                    }
                                    else if (role.Name == "Risk Manager")
                                    {
                                    }
                                    else if (role.Name == "Reviewer")
                                    {
                                        return RedirectToAction("Index", "Reviewer", new { area = "Reviewers" });
                                    }
                                    else if (role.Name == "Investor")
                                    {
                                        var getQualification = db.InvestorQualifications.ToList().Where(x => x.InvestorID == currentuser.Id).FirstOrDefault();
                                        if(getQualification != null)
                                        {
                                            if(getQualification.ProcessStatus == "Incomplete")
                                            {
                                                Session["InvQualified"] = "false";
                                            }
                                            else
                                            {
                                                Session["InvQualified"] = "true";
                                            }
                                        }
                                        else
                                        {
                                            Session["InvQualified"] = "false";
                                        }
                                        return RedirectToAction("Index", "Investor", new { area = "Investors" });
                                    }
                                    else if (role.Name == "Borrower")
                                    {
                                        return RedirectToAction("Index", "Borrowers", new { area = "Borrower" });
                                    }
                                    else if (role.Name == "Legal Team")
                                    {
                                        return RedirectToAction("Dashboard", "LegalTeam", new { area = "LegalTeams" });
                                    }
                                    else if (role.Name == "Credit Committee")
                                    {
                                        return RedirectToAction("Home", "Home", new { area = "CreditCommittee" });

                                    }
                                    else if (role.Name == "Property Guru")
                                    {
                                        return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
                                    }
                                }
                            }
                            else
                            {
                                TempData["response"] = "You are blocked.Please contact us";
                                return RedirectToAction("Login", "Home", new { area = "Public" });
                            }
                            
                        }
                        else
                        {
                            TempData["response"] = "You Account is not approved or blocked, Please contact administrator.";
                            return RedirectToAction("Index", "Home", new { area = "Public" });
                        }
                        return RedirectToAction("Index", "Home", new { area = "Public" });
                    }
                }
                else
                {
                    TempData["response"] = "Invalid Email or Password";
                    return RedirectToAction("Login", "Home", new { area = "Public" });
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "Public" });
        }
        public ActionResult userprofile(int? id)
        {
            if (Session["UserId"] != null)
            {
                id = Convert.ToInt32(Session["UserId"]);
                AspNetUser user = db.AspNetUsers.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
            else
            {
                TempData["response"] = "Session Expired or Please Login";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        public ActionResult userprofile(AspNetUser user)
        {
            try
            {
                var entity = db.AspNetUsers.Find(user.Id);

                entity.UserName = user.UserName;
                entity.firstname = user.firstname;
                entity.lastname = user.lastname;
                entity.mobile = user.mobile;
                entity.address = user.address;
                entity.city = user.city;
                entity.state = user.state;
                entity.zip_code = user.zip_code;

                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                var massage = ex.Message;
            }

            return RedirectToAction("userprofile", "Home");
        }

    }
}