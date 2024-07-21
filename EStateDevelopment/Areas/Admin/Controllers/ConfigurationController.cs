using EStateDevelopment.Areas.Admin.ViewModel;
using EStateDevelopment.Areas.Borrower.ViewModel;
using EStateDevelopment.Areas.Investors.ViewModel;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Admin.Controllers
{
    [Authorize(Roles = "QIGIAdmin, Admin,Administrator")]
    public class ConfigurationController : Controller
    {
        protected QIGIEntities db = new QIGIEntities();
        AdminActivity_Logs AAL = new AdminActivity_Logs();



        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductType()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult InvestorQuestion()

        {
            InvestorQuestionViewModel model = new InvestorQuestionViewModel();
            ViewBag.QuestionList = db.InvesterQuestions.ToList();

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]

        public ActionResult AddInvestorQuestion(InvestorQuestionViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    InvesterQuestion _entity = new InvesterQuestion();


                    _entity.Question = model.Question;

                    _entity.Control = model.Control;
                    _entity.Priority = model.Priority;

                    var _InvesterQuestion = db.InvesterQuestions.Add(_entity);

                    db.SaveChanges();
                    if (model.Control == "Multiple")
                    {
                        InvestorOptionQuestion _InvestorOptionQuestionEntity = new InvestorOptionQuestion();


                      
                        List<string> optionList = (List<string>)Session["OPtionQuestions"];

                        for (int i = 0; i < optionList.Count; i++)
                        {
                            _InvestorOptionQuestionEntity.QuestionId = _InvesterQuestion.InvesterQuestionsID;
                            _InvestorOptionQuestionEntity.OptionQuestion = optionList[i].ToString();

                            db.InvestorOptionQuestions.Add(_InvestorOptionQuestionEntity);
                            db.SaveChanges();
                        }


                    
                    }


                    AAL.AddLog("Add", "Add Investor Question In Application");
                    TempData["response"] = "Investor Question Added  Successfully.";
                    return RedirectToAction("InvestorQuestion");



                }
                catch (Exception ex)
                {
                    var massage = ex.Message;
                }
            }
            TempData["response"] = "Question not Added Successfully.";
            return RedirectToAction("InvestorQuestion");

        }

     

        [AllowAnonymous]
        public ActionResult SetOPtionQuestions(string SetOPtionQuestions)
        {

            var _OPtionQuestions = SetOPtionQuestions.TrimEnd().Split(',');

            var optionData = (_OPtionQuestions.Length - 1);

            var _OPtionQuestionsLIst = new List<string>();



            for (int i = 0; i < optionData; i++)
            {
                var _option = _OPtionQuestions[i].ToString();
                _OPtionQuestionsLIst.Add(_option);

            }
            Session["OPtionQuestions"] = _OPtionQuestionsLIst;


            return View();
        }

        [AllowAnonymous]
        public ActionResult SetBorrowerOPtionQuestions(string SetOPtionQuestions)
        {

            var _OPtionQuestions = SetOPtionQuestions.TrimEnd().Split(',');

            var optionData = (_OPtionQuestions.Length - 1);

            var _OPtionQuestionsLIst = new List<string>();



            for (int i = 0; i < optionData; i++)
            {
                var _option = _OPtionQuestions[i].ToString();
                _OPtionQuestionsLIst.Add(_option);

            }
            Session["BorrowerOPtionQuestions"] = _OPtionQuestionsLIst;


            return View();
        }




        public ActionResult UpdateInvestorQuestion(int InvesterQuestionsID)
        {
            InvestorQuestionViewModel model = new InvestorQuestionViewModel();
            var _entity = db.InvesterQuestions.Find(InvesterQuestionsID);
            model.Question = _entity.Question;
            model.InvesterQuestionsID = _entity.InvesterQuestionsID;


            return View(model);

        }

        [HttpPost]
        public ActionResult UpdateInvestorQuestion(InvestorQuestionViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    var _entity = db.InvesterQuestions.Find(model.InvesterQuestionsID);


                    _entity.Question = model.Question;
                    db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    AAL.AddLog("Update", "Update Investor Question In Application");
                    TempData["response"] = "Question Updated  Successfully.";
                    return RedirectToAction("InvestorQuestion");



                }
                catch (Exception ex)
                {
                    var massage = ex.Message;
                }
            }
            TempData["response"] = "Question not Added Successfully.";
            return RedirectToAction("InvestorQuestion");

        }

        public ActionResult DeleteInvestorQuestion(int id)
        {
            try
            {
                var investerQuestion = db.InvesterQuestions.Find(id);
                if (investerQuestion != null)
                {
                    db.InvesterQuestions.Remove(investerQuestion);

                    db.SaveChanges();
                    AAL.AddLog("Delete", "Delete Invester Question in Application");
                    TempData["response"] = "Invester Question Deleted Successfully !";
                    return RedirectToAction("InvestorQuestion");
                }
                else
                {
                    TempData["response"] = "Delete Invester Question   Failed !";
                    return RedirectToAction("InvestorQuestion");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("InvestorQuestion");
            }
        }

        public ActionResult BorrowerQuestion()

        {
            QuestionViewModel model = new QuestionViewModel();
            ViewBag.QuestionList = db.BorrowerQuestions.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult AddBorrowerQuestion(QuestionViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    BorrowerQuestion _entity = new BorrowerQuestion();


                    _entity.Question = model.Question;
                    _entity.Control = model.Control;
                    _entity.Priority = model.Priority;


                    db.BorrowerQuestions.Add(_entity);

                    db.SaveChanges();

                    if (model.Control == "Multiple")
                    {
                        BorrowerOptionQuestion _BorrowerOptionQuestionEntity = new BorrowerOptionQuestion();



                        List<string> optionList = (List<string>)Session["BorrowerOPtionQuestions"];

                        for (int i = 0; i < optionList.Count; i++)
                        {
                            //   _InvestorOptionQuestionEntity.QuestionId = _InvesterQuestion.InvesterQuestionsID;
                            _BorrowerOptionQuestionEntity.OptionQuestion = optionList[i].ToString();

                            db.BorrowerOptionQuestions.Add(_BorrowerOptionQuestionEntity);
                            db.SaveChanges();
                        }



                    }

                    AAL.AddLog("Add", "Add Borrower Question In Application");
                    TempData["response"] = "Borrower Question Added  Successfully.";
                    return RedirectToAction("BorrowerQuestion");



                }
                catch (Exception ex)
                {
                    var massage = ex.Message;
                }
            }
            TempData["response"] = "Question not Added Successfully.";
            return RedirectToAction("BorrowerQuestion");

        }
        public ActionResult UpdateBorrowerQuestion(int BorrowerQuestionID)
        {
            QuestionViewModel model = new QuestionViewModel();
            var _entity = db.BorrowerQuestions.Find(BorrowerQuestionID);
            model.Question = _entity.Question;
            model.BorrowerQuestionID = _entity.BorrowerQuestionID;


            return View(model);

        }

        [HttpPost]
        public ActionResult UpdateBorrowerQuestion(QuestionViewModel model)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    var _entity = db.BorrowerQuestions.Find(model.BorrowerQuestionID);


                    _entity.Question = model.Question;
                    db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    AAL.AddLog("Update", "Update Borrower Question In Application");
                    TempData["response"] = "Question Updated  Successfully.";
                    return RedirectToAction("BorrowerQuestion");



                }
                catch (Exception ex)
                {
                    var massage = ex.Message;
                }
            }
            TempData["response"] = "Question not Added Successfully.";
            return RedirectToAction("BorrowerQuestion");

        }

        public ActionResult DeleteBorrowerQuestion(int id)
        {
            try
            {
                var BorrowerQuestion = db.BorrowerQuestions.Find(id);
                if (BorrowerQuestion != null)
                {
                    db.BorrowerQuestions.Remove(BorrowerQuestion);

                    db.SaveChanges();
                    AAL.AddLog("Delete", "Delete Borrower Questionin Application");
                    TempData["response"] = "Borrower Question Deleted Successfully !";
                    return RedirectToAction("BorrowerQuestion");
                }
                else
                {
                    TempData["response"] = "Delete Borrower Question   Failed !";
                    return RedirectToAction("BorrowerQuestion");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("BorrowerQuestion");
            }
        }

      



        public ActionResult AddProductType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProductType(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(productType);
                db.SaveChanges();
                ModelState.Clear();
            }
            return View();
        }

        public ActionResult ListProductType()
        {
            var protype = db.ProductTypes.ToList();
            return View(protype);
        }

        public ActionResult EditProType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductType productType = db.ProductTypes.Find(id);
            if (productType == null)
            {
                return HttpNotFound();
            }
            return View(productType);
        }

        [HttpPost]
        public ActionResult EditProType(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProductType", "Configuration");
            }
            return View(productType);
        }

        public ActionResult DeletePro(int id)
        {
            ProductType productType = db.ProductTypes.Find(id);
            db.ProductTypes.Remove(productType);
            db.SaveChanges();
            return RedirectToAction("ListProductType", "Configuration");
        }


        public ActionResult AddProductDocType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProducDoctType(ProductDocumentType productDocumentType)
        {
            if (ModelState.IsValid)
            {
                db.ProductDocumentTypes.Add(productDocumentType);
                db.SaveChanges();
                ModelState.Clear();
            }
            return View();
        }

        public ActionResult ListProductDocType()
        {
            var prodoctype = db.ProductDocumentTypes.ToList();
            return View(prodoctype);
        }

        public ActionResult EditProDocType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductDocumentType prodoctypes = db.ProductDocumentTypes.Find(id);
            if (prodoctypes == null)
            {
                return HttpNotFound();
            }
            return View(prodoctypes);
        }

        [HttpPost]
        public ActionResult EditProDocType(ProductDocumentType productDocumentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDocumentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProductDocType", "Configuration");
            }
            return View(productDocumentType);
        }

        public ActionResult DeleteProDocType(int id)
        {
            ProductDocumentType prodoctype = db.ProductDocumentTypes.Find(id);
            db.ProductDocumentTypes.Remove(prodoctype);
            db.SaveChanges();
            return RedirectToAction("ListProductType", "Configuration");
        }
        public ActionResult AddAreaOfStouck()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddAreaOfStouck(AreaOfStockViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ExistStock = db.AreaOfStocks.ToList().Exists(x => x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase));

                    if (ExistStock == false)
                    {

                        AreaOfStock _entity = new AreaOfStock();
                        _entity.Name = model.Name;

                        db.AreaOfStocks.Add(_entity);
                        db.SaveChanges();
                        AAL.AddLog("Add", "Add Area of stock  in Application");
                        TempData["response"] = "Area of stock  Add Successfully.";
                        return RedirectToAction("ListAreaOfStock", "Configuration");
                    }
                    else
                    {
                        TempData["response"] = "Same Record Already Exists !";
                        return RedirectToAction("AddAreaOfStouck", "Configuration");
                    }


                }
                catch (Exception ex)
                {

                    var massage = ex.Message;
                }
            }



            return View();
        }
        public ActionResult ListAreaOfStock()
        {
            var ListAreaOfStock = db.AreaOfStocks.ToList();

            return View(ListAreaOfStock);
        }
        public ActionResult DeleteAreaOfStock(int id)
        {
            try
            {
                var areaOfStock = db.AreaOfStocks.Find(id);
                if (areaOfStock != null)
                {
                    db.AreaOfStocks.Remove(areaOfStock);

                    db.SaveChanges();
                    AAL.AddLog("Delete", "Delete Area Of Stock in Application");
                    TempData["response"] = "Area Of Stock Deleted Successfully !";
                    return RedirectToAction("ListAreaOfStock", "Configuration");
                }
                else
                {
                    TempData["response"] = "Area Of Stock Delete Failed !";
                    return RedirectToAction("ListAreaOfStock", "Configuration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListAreaOfStock", "Configuration");
            }
        }

        public ActionResult UpdateAreaOfStock(int id)
        {
            try
            {
                var areaOfStock = db.AreaOfStocks.Find(id);
                AreaOfStockViewModel model = new AreaOfStockViewModel();
                if (areaOfStock != null)
                {
                    model.Id = areaOfStock.Id;
                    model.Name = areaOfStock.Name;

                    return View(model);
                }
                else
                {
                    TempData["response"] = "Area Of Stock Not Found !";
                    return RedirectToAction("ListAreaOfStock", "Configuration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListAreaOfStock", "Configuration");
            }
        }
        [HttpPost]
        public ActionResult UpdateAreaOfStock(AreaOfStockViewModel model, int id)
        {
            try
            {
                var ExistStock = db.AreaOfStocks.ToList().Exists(x => x.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != id);
                if (ExistStock == false)
                {
                    var _entity = db.AreaOfStocks.Find(id);
                    if (_entity != null)
                    {
                        _entity.Name = model.Name;

                        db.Entry(_entity).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                        AAL.AddLog("Update", "Update Area Of Stocks in Application");
                        TempData["response"] = "Area Of Stocks Updated Successfully !";
                        return RedirectToAction("ListAreaOfStock", "Configuration");
                    }
                    else
                    {
                        TempData["response"] = "Role Update Failed !";
                        return RedirectToAction("ListAreaOfStock", "Configuration");
                    }
                }
                else
                {
                    TempData["response"] = "Same Record Already Exists !";
                    return RedirectToAction("UpdateAreaOfStock", "Configuration");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Oops Error " + ex.Message;
                return RedirectToAction("ListAreaOfStock", "Configuration");
            }
        }


    }
}