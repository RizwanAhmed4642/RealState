using EStateDevelopment.Areas.Admin;
using EStateDevelopment.Areas.Investors.ViewModel;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Investors.Controllers
{
    public class InvestorController : Controller
    {
        protected QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs AAL = new AdminActivity_Logs();
       protected EncryptDecrypt encry = new EncryptDecrypt();

        [Authorize(Roles = "Investor")]
        public ActionResult Index()
        {
            try
            {
                if (Session["InvQualified"] != null)
                {
                    if (Session["InvQualified"].ToString() == "false")
                    {
                        return RedirectToAction("InvQualification", "Investor", new { area = "Investors" });
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Home", new { area = "Public" });
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Index", "Investor", new { area = "Investors" });
            }
        }


        [AllowAnonymous]
        public ActionResult AddAnswer()
        {
           // ViewBag.InvesterQuestionslist = db.InvesterQuestions.ToList();
            var _areaOfStocklist = db.AreaOfStocks.ToList();
            ViewBag.AreaOfStocklist = new SelectList(_areaOfStocklist.ToList(), "Name", "Name");
            var _typicalYields = db.TypicalYields.ToList();
            ViewBag.TypicalYields = new SelectList(_typicalYields.ToList(), "Id", "Name");   
            var _financialAdvisors = db.FinancialAdvisors.ToList();
            ViewBag.FinancialAdvisors = new SelectList(_financialAdvisors.ToList(), "Id", "Name");
            var _averageInvestments = db.AverageInvestments.ToList();
            ViewBag.AverageInvestments = new SelectList(_averageInvestments.ToList(), "Id", "Amount");

            var _productType = db.ProductTypes.ToList();
            ViewBag.ProductTypeID = new SelectList(_productType.ToList(), "TypeName", "TypeName");

            var listquestions = db.InvesterQuestions.ToList();
            var listOptions = db.InvestorOptionQuestions.ToList();
            for (int i = 0; i < listquestions.Count(); i++)
            {
                listquestions[i].Investoroptionquestion = new List<InvestorOptionQuestion>();
                if (listquestions[i].Control == "Multiple")
                {
                  
                    foreach (var item in listOptions.ToList().Where(x=>x.QuestionId == listquestions[i].InvesterQuestionsID).ToList())
                    {
                        listquestions[i].Investoroptionquestion.Add(item);
                        ViewData["Option" + item.QuestionId] = listquestions[i].Investoroptionquestion;
                    }
                }
                else
                {
                    listquestions[i].Investoroptionquestion = null;
                }

            }
            ViewBag.InvesterQuestionslist = listquestions;
            return View();
        }
        [HttpPost]
        public ActionResult AddAnswer(InvestorSubmittedQuestionsViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    InvestorSubmittedQuestion _entity = new InvestorSubmittedQuestion();
                    List<string> ans = new List<string>();
          

                    Session["InvestorQuestionList"] = model;
                    return RedirectToAction("ProductsAccordingToAnswer");

                }
                catch (Exception ex)
                {

                    var Massage = ex.Message;
                }
            }
            return View();

        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult AreaOfStock(string IAOS)
        {
            Session["InvestorAreaOfStock"] = IAOS;


            var Result = Json("Done", JsonRequestBehavior.AllowGet);
            return Result;


        }




        [AllowAnonymous]
        public ActionResult ProductsAccordingToAnswer()
        {
            if(Session["InvestorQuestionList"] != null)
            {
                ////Session["InvestorAreaOfStock"]= "Energy Sector";
                if (Session["InvestorAreaOfStock"] !=null)
                {
                    string _areaOfStock = Session["InvestorAreaOfStock"].ToString();
                    var product = db.Products.ToList().Where(x => x.AreaOfStock == _areaOfStock).ToList();

                    var images = db.ProductImages.ToList();

                    for(int i=0; i<product.Count();i++)
                    {
                        foreach(var item in images.ToList().Where(x => x.ProductID == product[i].ProductID).ToList())
                        {
                            product[i].ProductImages = new List<ProductImage>();
                            product[i].ProductImages.Add(item);
                        }
                       

                    }





                    return View(product);
                }



                return View();
            }

            else
            {
                TempData["response"] = "Please Answers the Questions";
                return RedirectToAction("AddAnswer");
            }

           
        }
        [AllowAnonymous]
        public ActionResult AddInvestorsApplication(int ProductID)
        {
            if (Session["InvestorQuestionList"] != null)
            {
                var submitedAnswers = Session["InvestorQuestionList"] as InvestorSubmittedQuestionsViewModel;
                for (int i = 0; i < submitedAnswers.Answer.Count(); i++)
                {
                   // var listofproduct=db.Products.Where(x=>x.AreaOfStocks==submitedAnswers.Answer[i].Contains)

                }

                InvestorApplicationViewModel model = new InvestorApplicationViewModel();
               
                model.ProductID = ProductID;
                return View(model);
            }
            else
            {
                     TempData["response"] = "Please Answers the Questions";
                        return RedirectToAction("AddAnswer");
            }
            
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddInvestorsApplication(InvestorApplicationViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    var ExistEmail = db.AspNetUsers.ToList().Exists(x => x.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase));
                    if (ExistEmail == false)
                    {

                        InvestorApplication _entity = new InvestorApplication();
                        _entity.FirstName = model.FirstName;
                        _entity.LastName = model.LastName;
                        _entity.Email = model.Email;
                        _entity.State = model.State;
                        _entity.ZipCode = model.ZipCode;
                        _entity.ApplicationNumber = model.ApplicationNumber;
                        _entity.CompleteAddress = model.CompleteAddress;
                        _entity.City = model.City;
                        _entity.FullName = model.FirstName + " " + model.LastName;
                        _entity.Mobile = model.Mobile;
                        _entity.Phone = model.Phone;
                        _entity.WorkPhone = model.WorkPhone;
                        _entity.TotalNoofApplication = model.TotalNoofApplication;
                        _entity.InvestorSubmittedQuestionsID = model.InvestorSubmittedQuestionsID;
                        _entity.ProductID = model.ProductID;
                        _entity.Descripation = model.Descripation;

                        var lists = db.InvestorApplications.ToList();
                        string fullcode;
                        if (lists.Count > 0)
                        {
                            if (lists[0].ApplicationNumber != null)
                            {
                                int large, small;
                                int ApplD = 0;

                                large = Convert.ToInt32(lists[0].ApplicationNumber.Split('-')[2]);
                                small = Convert.ToInt32(lists[0].ApplicationNumber.Split('-')[2]);
                                for (int i = 0; i < lists.Count; i++)
                                {
                                    if (lists[i].ApplicationNumber != null)
                                    {
                                        var t = Convert.ToInt32(lists[i].ApplicationNumber.Split('-')[2]);
                                        if (Convert.ToInt32(lists[i].ApplicationNumber.Split('-')[2]) > large)
                                        {
                                            ApplD = lists[i].InvestorApplicationID;
                                            large = Convert.ToInt32(lists[i].ApplicationNumber.Split('-')[2]);

                                        }
                                        else if (Convert.ToInt32(lists[i].ApplicationNumber.Split('-')[2]) < small)
                                        {
                                            small = Convert.ToInt32(lists[i].ApplicationNumber.Split('-')[2]);
                                        }
                                        else
                                        {
                                            if (large < 2)
                                            {
                                                ApplD = lists[i].InvestorApplicationID;
                                            }
                                        }
                                    }
                                }
                                var newitems = lists.ToList().Where(x => x.InvestorApplicationID == Convert.ToInt32(ApplD)).FirstOrDefault();
                                if (newitems != null)
                                {
                                    if (newitems.ApplicationNumber != null)
                                    {
                                        var VcodeSplit = newitems.ApplicationNumber.Split('-');
                                        int code = Convert.ToInt32(VcodeSplit[2]) + 1;
                                        fullcode = "App" + "-" + "IA" + "-" + "00" + Convert.ToString(code);
                                    }
                                    else
                                    {
                                        fullcode = "App" + "-" + "IA" + "-" + "001";
                                    }
                                }
                                else
                                {
                                    fullcode = "App" + "-" + "IA" + "-" + "001";
                                }
                            }
                            else
                            {
                                fullcode = "App" + "-" + "IA" + "-" + "001";
                            }
                        }
                        else
                        {
                            fullcode = "App" + "-" + "IA" + "-" + "001";
                        }


                        _entity.ApplicationNumber = fullcode;



                        var application = db.InvestorApplications.Add(_entity);
                        db.SaveChanges();
                        /// user register
                        if (Session["InvestorQuestionList"] != null)
                        {
                            InvestorSubmittedQuestion _QAentity = new InvestorSubmittedQuestion();
                            var submitedAnswers = Session["InvestorQuestionList"] as InvestorSubmittedQuestionsViewModel;
                            if (submitedAnswers.Answer != null)
                            {

                                var questions = db.InvesterQuestions.ToList();
                                for (int i = 0; i < submitedAnswers.Answer.Count(); i++)
                                {
                                    _QAentity.Answer = submitedAnswers.Answer[i];
                                    _QAentity.InvestorQuestionID = questions[i].InvesterQuestionsID;
                                    var QAID = db.InvestorSubmittedQuestions.Add(_QAentity);
                                    db.SaveChanges();
                                    InvestorApplicationCollection _IACentity = new InvestorApplicationCollection();
                                    _IACentity.QuestionID = questions[i].InvesterQuestionsID;
                                    _IACentity.AppID = application.InvestorApplicationID;
                                    _IACentity.UserId = 1;
                                    db.InvestorApplicationCollections.Add(_IACentity);
                                    db.SaveChanges();
                                }
                            }

                        }
                        var roles = db.AspNetRoles.ToList().Where(x => x.Name == "Investor").FirstOrDefault();
                        if (roles != null)
                        {
                        }
                        else
                        {
                            AspNetRole newrole = new AspNetRole();
                            newrole.Name = "Investor";
                            newrole.Status = true;
                            db.AspNetRoles.Add(newrole);
                            db.SaveChanges();
                        }
                        var Currentroles = db.AspNetRoles.AsQueryable().ToList().Where(x => x.Name == "Investor").FirstOrDefault();

                        AspNetUser newuser = new AspNetUser();
                        newuser.Email = application.Email;
                        newuser.PasswordHash = encry.Encrypt(application.Email);
                        newuser.AdminApproval = false;
                        newuser.AdminRejected = false;
                        newuser.address = application.CompleteAddress;
                        newuser.city = application.City;
                        newuser.state = application.State;
                        newuser.UserName = application.FirstName + application.LastName;
                        newuser.phone = application.Phone;
                        newuser.is_active = true;
                        newuser.lastname = application.LastName;
                        newuser.firstname = application.FirstName;
                        newuser.role_id = Currentroles.Id;
                        var currentuser = db.AspNetUsers.Add(newuser);
                        db.SaveChanges();

                        InvestorQualification qualification = new InvestorQualification();

                        //qualification.Name = " ";
                        //qualification.Adress = " ";
                        //qualification.Phone = " ";
                        //qualification.Fax = " ";
                        //qualification.Email = "";
                        //qualification.Income = false;
                        //qualification.NetWorth = false;
                        //qualification.DegreeandDesignations = " ";
                        //qualification.CurrentNoOfProperties = " ";
                        //qualification.CurrentTypesOfProperties = " ";
                        //qualification.PriorNoOfProperties = " ";
                        //qualification.PriorTypesOfProperties = " ";
                        //qualification.RealEstateExperience = " ";
                        //qualification.InvestmentValue = " ";
                        //qualification.PreviousBusiness = " ";
                        //qualification.Memberof = "";
                        //qualification.HomebuyersofGA = " ";

                        //qualification.Signature = " ";
                        //qualification.ConfirmSignature = " ";
                        //qualification.ConfirmDate = DateTime.Now;


                        qualification.Date = DateTime.Now;
                        qualification.InvestorID = Convert.ToInt32(currentuser.Id);
                        qualification.ProcessStatus = "Incomplete";
                        qualification.Adress = currentuser.address;
                        qualification.Phone = currentuser.phone;

                        qualification.Adress = currentuser.address;

                        db.InvestorQualifications.Add(qualification);
                        db.SaveChanges();

                        var _msgForAdmin = "<div><h1> Thank For Your Application </h3 ><h4> Your new account has generated, you can login from the following link </h4><a href='http://localhost:52317/Public/Home/Login' target='_blank'> QIGI Application </a><h3>User your email address as username and password.</h3><div>";
                        EmailSend _emailsend = new EmailSend();

                        _emailsend.FromEmailAddress = ConfigurationManager.AppSettings["From_Email"];

                        var _email = ConfigurationManager.AppSettings["MENAFATF_Email"];
                        var _pass = ConfigurationManager.AppSettings["From_Email_Password"];
                        var _dispName = "QIGI Administration";
                        MailMessage mymessage = new MailMessage();
                        mymessage.To.Add(application.Email);
                        mymessage.From = new MailAddress(_email, _dispName);
                        mymessage.Subject = "Application Submitted";
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
                        AAL.AddLog("Investor New Application", "Investor Submitted New Application", application.Email);

                        TempData["response"] = "Thank you for application, please check your email";
                        return RedirectToAction("Login", "Home", new { area = "Public" });
                    }
                    else
                    {
                        TempData["response"] = "Same Email Already Exists !";
                        return RedirectToAction("AddInvestorsApplication", "Investor",new {area="Investors", ProductID= model.ProductID });

                    }
                }
               
                catch (Exception ex)
                {
                    TempData["response"] = "Oops Error " + ex.Message;
                    return RedirectToAction("Index", "Home", new { area = "Public" });
                }
            }
            return View(model);
        }

        [AllowAnonymous]

        public ActionResult InvestorProductDetailPage(int ProductID)
        {
            try
            {
                var users = db.AspNetUsers.ToList();
                var charges = db.ChargesSlabs.ToList();
                var chargesType = db.ProductChargesTypes.ToList();
                var Pcharges = db.ProductCharges.ToList();

                var images = db.ProductImages.ToList().Where(i => i.ProductID == ProductID);

                var colletralTypes = db.CollateralTypes.ToList();

                var products = db.Products.Find(ProductID);

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

                foreach (var item in colletralTypes.ToList().Where(c => c.ProductID == products.ProductID).ToList())
                {
                    products.CollateralTypes = new List<CollateralType>();
                    products.CollateralTypes.Add(item);
                }

                foreach (var item in images.ToList().Where(i => i.ProductID == products.ProductID).ToList())
                {
                    products.ProductImages = new List<ProductImage>();
                    products.ProductImages.Add(item);

                }

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("Home", "Dashboard", new { area = "PropertyGuru" });
            }
        }
        [Authorize(Roles = "Investor")]
        public ActionResult ListInvestorsApplication()
        {
            var _list = db.InvestorApplications.ToList();
            return View(_list);
        }
        [Authorize(Roles = "Investor")]
        public ActionResult AddInvestorFunds()
        {
            return View();
        }
        [Authorize(Roles = "Investor")]
        public ActionResult ListInvestorFunds()
        {
            var _list = db.InvestorFunds.ToList();
            return View(_list);
        }
        [Authorize(Roles = "Investor")]
        public ActionResult AddInvestorDocuments()
        {
            return View();
        }
        [Authorize(Roles = "Investor")]
        public ActionResult ListInvestorFeedback()
        {
            var _list = db.InvestorFeedbackHistories.ToList();
            return View(_list);
        }
        [Authorize(Roles = "Investor")]
        public ActionResult AddFillFundDetails()
        {
            return View();
        }
        [Authorize(Roles = "Investor")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Investor")]
        public ActionResult InvQualification()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    var user = db.AspNetUsers.Find(Convert.ToInt32(Session["UserId"]));
                    var invqualification = db.InvestorQualifications.Where(q => q.InvestorID == user.Id).FirstOrDefault();

                    if (invqualification.ProcessStatus == "complete")
                    {
                        var qualification = db.InvestorQualifications.Where(q => q.InvestorID == user.Id).FirstOrDefault();
                        return PartialView("updateInvQualification", qualification);

                        //return RedirectToAction("Dashboard", "Investor", new { area = "Investors" });
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {

                    var massaage = ex.Message;

                    TempData["response"] = "Opps" + massaage;
                    return RedirectToAction("Login", "Home", new { area = "Public" });
                }
              
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        public ActionResult InvQualification(InvestorQualification model)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    var user = db.AspNetUsers.Find(Convert.ToInt32(Session["UserId"]));
                    var qualification = db.InvestorQualifications.Where(q => q.InvestorID == user.Id).FirstOrDefault();


                    qualification.Name = model.Name;
                    qualification.Adress = model.Adress;
                    qualification.Phone = model.Phone;
                    qualification.Fax = model.Fax;
                    qualification.Email = model.Email;
                    qualification.Income = model.Income;
                    qualification.NetWorth = model.NetWorth;
                    qualification.Occupation = model.Occupation;
                    qualification.BusinessorFinancialExperience = model.BusinessorFinancialExperience;
                    qualification.DegreeandDesignations = model.DegreeandDesignations;
                    qualification.CurrentNoOfProperties = model.CurrentNoOfProperties;
                    qualification.CurrentTypesOfProperties = model.CurrentTypesOfProperties;
                    qualification.PriorNoOfProperties = model.PriorNoOfProperties;
                    qualification.PriorTypesOfProperties = model.PriorTypesOfProperties;
                    qualification.RealEstateExperience = model.RealEstateExperience;
                    qualification.InvestmentValue = model.InvestmentValue;
                    qualification.PreviousBusiness = model.PreviousBusiness;
                    qualification.Memberof = model.Memberof;
                    qualification.HomebuyersofGA = model.HomebuyersofGA;
                    qualification.ProcessStatus = "complete";
                    qualification.Signature = model.Signature;
                    qualification.Date = model.Date;
                    qualification.ConfirmDate = model.ConfirmDate;
                    qualification.ConfirmSignature = model.ConfirmSignature;
                    qualification.createdDate = model.createdDate;


                    db.Entry(qualification).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["response"] = "Qualification Form Submitted Successfully";
                    return RedirectToAction("Dashboard", "Investor", new { area = "Investors" });
                }
                catch (Exception ex)
                {
                    TempData["response"] = "Oops Error " + ex.Message;
                    return RedirectToAction("Dashboard", "Investor", new { area = "Investors" });
                }

            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        public ActionResult updateInvQualification(InvestorQualification investorQualification)
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    db.Entry(investorQualification).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    var massage = ex.Message;
                }

                return RedirectToAction("userprofile", "Home", new { area = "Public" });
            }
            else
            {
                TempData["response"] = "Session Expired or Please Login";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }

        }

        public ActionResult UpdateQualification()
        {
            if (Session["UserId"] != null)
            {
                try
                {
                    var user = db.AspNetUsers.Find(Convert.ToInt32(Session["UserId"]));
                    var qualification = db.InvestorQualifications.Where(q => q.InvestorID == user.Id).FirstOrDefault();

                    return PartialView("updateInvQualification", qualification);
                }
                catch (Exception ex)
                {

                    var massage = ex.Message;
                }

                return RedirectToAction("userprofile", "Home", new { area = "Public" });
            }
            else
            {
                TempData["response"] = "Session Expired or Please Login";
                return RedirectToAction("Login", "Home", new { area = "Public" });
            }
        }

    }

}