using EStateDevelopment.Areas.Admin;
using EStateDevelopment.Areas.Borrower.ViewModel;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Borrower.Controllers
{
    public class BorrowersController : Controller
    {

        // GET: Borrower/Borrowers
        QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs AAL = new AdminActivity_Logs();

        public ActionResult Index()
        {
            return View();
        }
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

            var listquestions = db.BorrowerQuestions.ToList();
            var listOptions = db.BorrowerOptionQuestions.ToList();
            for (int i = 0; i < listquestions.Count(); i++)
            {
                listquestions[i].Borroweroptionquestion = new List<BorrowerOptionQuestion>();
                if (listquestions[i].Control == "Multiple")
                {

                    foreach (var item in listOptions.ToList().Where(x => x.QuestionId == listquestions[i].BorrowerQuestionID).ToList())
                    {
                        listquestions[i].Borroweroptionquestion.Add(item);
                        ViewData["Option" + item.QuestionId] = listquestions[i].Borroweroptionquestion;
                    }
                }
                else
                {
                    listquestions[i].Borroweroptionquestion = null;
                }

            }
            ViewBag.BorrowerQuestionslist = listquestions;
            return View();
        }
        [HttpPost]
        public ActionResult AddAnswer(BorrowerSubmittedQuestionsViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    BorrowerSubmittedQuestion _entity = new BorrowerSubmittedQuestion();
                    List<string> ans = new List<string>();


                    Session["BorrowerQuestionList"] = model;
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
        public ActionResult AreaOfStock(string AOS)
        {
            Session["AreaOfStock"] = AOS;


            var Result = Json("Done", JsonRequestBehavior.AllowGet);
            return Result;


        }


        [AllowAnonymous]
        public ActionResult ProductsAccordingToAnswer()
        {
            if (Session["BorrowerQuestionList"] != null)
            {
                if (Session["AreaOfStock"] != null)
                {
                    string _areaOfStock = Session["AreaOfStock"].ToString();
                    var product = db.Products.ToList().Where(x => x.AreaOfStock == _areaOfStock).ToList();

                    var images = db.ProductImages.ToList();

                    for (int i = 0; i < product.Count(); i++)
                    {
                        foreach (var item in images.ToList().Where(x => x.ProductID == product[i].ProductID).ToList())
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

        public ActionResult AddApplication( int ProductID)
        {
            if (Session["BorrowerQuestionList"] != null)
            {
                var submitedAnswers = Session["BorrowerQuestionList"] as BorrowerSubmittedQuestionsViewModel;
                for (int i = 0; i < submitedAnswers.Answers.Count(); i++)
                {
                    // var listofproduct=db.Products.Where(x=>x.AreaOfStocks==submitedAnswers.Answer[i].Contains)

                }

                BorrowerApplicationViewModel model = new BorrowerApplicationViewModel();

                model.ProductID = ProductID;
                return View(model);
            }
            else
            {
                TempData["response"] = "Please Answers the Questions";
                return RedirectToAction("AddAnswer");
            }

        }
        [AllowAnonymous]

        [HttpPost]
        public ActionResult AddApplication(BorrowerApplicationViewModel model)
        {
            if (ModelState.IsValid)

            {
                var isExsist = db.BorrowerApplications.ToList().Where(x => x.Email == model.Email).ToList();
                if (isExsist.Count == 0)
                {
                    try
                    {

                        BorrowerApplication _entity = new BorrowerApplication();
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
                        _entity.BorrowerSubmittedQuestionsID = model.BorrowerSubmittedQuestionsID;
                        _entity.ProductID = model.ProductID;
                        _entity.Descripation = model.Descripation;
                        var lists = db.BorrowerApplications.ToList();
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
                                            ApplD = lists[i].BorrowerApplicationID;
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
                                                ApplD = lists[i].BorrowerApplicationID;
                                            }
                                        }
                                    }
                                }
                                var newitems = lists.ToList().Where(x => x.BorrowerApplicationID == Convert.ToInt32(ApplD)).FirstOrDefault();
                                if (newitems != null)
                                {
                                    if (newitems.ApplicationNumber != null)
                                    {
                                        var VcodeSplit = newitems.ApplicationNumber.Split('-');
                                        int code = Convert.ToInt32(VcodeSplit[2]) + 1;
                                        fullcode = "App" + "-" + "BA" + "-" + "00" + Convert.ToString(code);
                                    }
                                    else
                                    {
                                        fullcode = "App" + "-" + "BA" + "-" + "001";
                                    }
                                }
                                else
                                {
                                    fullcode = "App" + "-" + "BA" + "-" + "001";
                                }
                            }
                            else
                            {
                                fullcode = "App" + "-" + "BA" + "-" + "001";
                            }
                        }
                        else
                        {
                            fullcode = "App" + "-" + "BA" + "-" + "001";
                        }


                        _entity.ApplicationNumber = fullcode;

                        var application = db.BorrowerApplications.Add(_entity);
                        db.SaveChanges();


                        /// user register
                        if (Session["BorrowerQuestionList"] != null)
                        {
                            BorrowerSubmittedQuestion _QAentity = new BorrowerSubmittedQuestion();
                            var submitedAnswers = Session["BorrowerQuestionList"] as BorrowerSubmittedQuestionsViewModel;
                            if (submitedAnswers.Answers != null)
                            {

                                var questions = db.BorrowerQuestions.ToList();
                                for (int i = 0; i < submitedAnswers.Answers.Count(); i++)
                                {
                                    _QAentity.Answers = submitedAnswers.Answers[i];
                                    _QAentity.BorrowerQuestionID = questions[i].BorrowerQuestionID;
                                    var QAID = db.BorrowerSubmittedQuestions.Add(_QAentity);
                                    db.SaveChanges();
                                    BorrowerApplicationCollection _BACentity = new BorrowerApplicationCollection();
                                    _BACentity.QuestionID = questions[i].BorrowerQuestionID;
                                    _BACentity.AppID = application.BorrowerApplicationID;
                                    _BACentity.UserId = 1;
                                    db.BorrowerApplicationCollections.Add(_BACentity);
                                    db.SaveChanges();
                                }
                                AAL.AddLog("New Application", "Borrower Add new Application Successfully", application.Email);
                            }

                        }

                        return RedirectToAction("Login", "Home", new { area = "Public" });
                    }
                    catch (Exception ex)
                    {

                        var massage = ex.Message;
                    }
                }
                else
                {
                    TempData["response"] = "Email already Exsist.";

                    return View(model);
                }
            }
            return View();
        }
        [AllowAnonymous]

        public ActionResult BorrowerProductDetailPage(Nullable<int> BorrowerSubmittedQuestionsID, int ProductID)
        {
            BorrowerApplicationViewModel model = new BorrowerApplicationViewModel();
            model.BorrowerSubmittedQuestionsID = BorrowerSubmittedQuestionsID;
            model.ProductID = ProductID;
            return View(model);
        }
        [Authorize(Roles = "Borrower")]
        public ActionResult ListBorrowerApplication()
        {
            var _list = db.BorrowerApplications.ToList();
            return View(_list);
        }
        [Authorize(Roles = "Borrower")]
        public ActionResult DetailApplication(int id)
        {
            var _entity = db.BorrowerApplications.Find(id);
            return View(_entity);
        }

        [Authorize(Roles = "Borrower")]
        public ActionResult AddBorrowerDocuments()
        {
            return View();
        }
        [Authorize(Roles = "Borrower")]
        public ActionResult AddBorrowerAssets()
        {
            return View();
        }

    }
}