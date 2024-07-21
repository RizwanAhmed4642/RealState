using EStateDevelopment.Areas.Investors.ViewModel;
using EStateDevelopment.Areas.Reviewers.ViewModel;
using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Reviewers.Controllers
{
    [Authorize(Roles = "Reviewer")]
    public class ReviewerController : Controller
    {
        QIGIEntities db = new QIGIEntities();
        // GET: Reviewers/Reviewer
        public ActionResult Index()
        {
            ViewBag.InvesterApplication = db.InvestorApplications.ToList();
            ViewBag.BorrowerApplication = db.BorrowerApplications.ToList();
            var BorrowerApplicationColection = db.BorrowerApplications.ToList();

            var _resultModel = new List<BorrowerApplicationViewModel>();

            var _allApplicationCollection = db.BorrowerApplicationCollections.ToList();
            foreach (var item in _allApplicationCollection)
            {
                var _model = new BorrowerApplicationViewModel();

                _model.AppID = item.AppID;
                _model.QuestionID = item.QuestionID;
                _model.BorrowerAppCollectionId = item.BorrowerAppCollectionId;
                _model.UserId = item.UserId;
                _resultModel.Add(_model);


            }
            var _BorrowerApplications = db.BorrowerApplications.ToList();
            if (_resultModel.Count > 0)
            {
                for (int i = 0; i < _resultModel.Count(); i++)
                {
                    foreach (var item in _BorrowerApplications.ToList().Where(x => x.BorrowerApplicationID == _resultModel[i].AppID))
                    {
                        _resultModel[i].FirstName = item.FirstName;
                        _resultModel[i].LastName = item.LastName;
                        _resultModel[i].Email = item.Email;
                        _resultModel[i].CompleteAddress = item.CompleteAddress;
                        _resultModel[i].ZipCode = item.ZipCode;
                        _resultModel[i].State = item.State;
                        _resultModel[i].Phone = item.Phone;
                        _resultModel[i].Mobile = item.Mobile;
                        _resultModel[i].Descripation = item.Descripation;
                        _resultModel[i].FullName = item.FullName;
                        _resultModel[i].ApplicationNumber = item.ApplicationNumber;
                        _resultModel[i].AppID = item.BorrowerApplicationID;


                    }
                }
            }
            ViewBag.ApplicationCollection = _resultModel;
            return View();




        }
        public ActionResult BorrowerAssignAccountManagerForReviewAndAssignment(int BorrowerApplicationID)
        {
            var _resultModel = new List<UserViewModel>();
            var _Rolelist = db.AspNetUserRoles.ToList().Where(x => x.RoleName == "Account Manager").ToList();
            foreach (var item in _Rolelist)
            {
                var _model = new UserViewModel();

                _model.UserId = item.User_id;
                _model.RoleName = item.RoleName;
                _model.Roles_id = item.Roles_id;
                _model.UserName = item.UserName;
                _resultModel.Add(_model);


            }
            var _user = db.AspNetUsers.ToList();
            if (_resultModel.Count > 0)
            {
                for (int i = 0; i < _resultModel.Count(); i++)
                {
                    foreach (var item in _user.ToList().Where(x => x.Id == _resultModel[i].UserId))
                    {
                        _resultModel[i].UserName = item.UserName;
                        _resultModel[i].UserId = item.Id;



                    }
                }
            }
            ViewBag.AccountManager = new SelectList(_resultModel, "UserId", "UserName");
            BorrowerApplicationViewModel model = new BorrowerApplicationViewModel();
            var _borrowerEntity = db.BorrowerApplications.Find(BorrowerApplicationID);
            model.AppID = BorrowerApplicationID;
            model.FirstName = _borrowerEntity.FirstName;
            model.LastName = _borrowerEntity.LastName;
            model.Email = _borrowerEntity.Email;
            model.Mobile = _borrowerEntity.Mobile;
            model.CompleteAddress = _borrowerEntity.CompleteAddress;
            model.City = _borrowerEntity.City;
            model.State = _borrowerEntity.State;
            model.WorkPhone = _borrowerEntity.WorkPhone;
            model.TotalNoofApplication = _borrowerEntity.TotalNoofApplication;
            //model.InvestorSubmittedQuestionsID = _investorEntity.InvestorSubmittedQuestionsID;
            model.ProductID = _borrowerEntity.ProductID;
            model.Descripation = _borrowerEntity.Descripation;


            return View(model);
        }
        [HttpPost]
        public ActionResult BorrowerAssignAccountManagerForReviewAndAssignment(BorrowerApplicationViewModel model)
        {
            BAppAm _entity = new BAppAm();
            _entity.AppId = model.AppID;
            _entity.AccountManagerId = model.UserId;
            _entity.ReviewerID =Convert.ToInt32(Session["UserId"]);
            _entity.CreatedDate = DateTime.Now;
            db.BAppAms.Add(_entity);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult InvestorAssignAccountManagerForReviewAndAssignment(int InvestorApplicationID)
        {
            InvestorApplicationViewModel model = new InvestorApplicationViewModel();
            var _investorEntity = db.InvestorApplications.Find(InvestorApplicationID);
            model.FirstName = _investorEntity.FirstName;
            model.LastName = _investorEntity.LastName;
            model.Email = _investorEntity.Email;
            model.Mobile = _investorEntity.Mobile;
            model.CompleteAddress = _investorEntity.CompleteAddress;
            model.City = _investorEntity.City;
            model.State = _investorEntity.State;
            model.WorkPhone = _investorEntity.WorkPhone;
            model.TotalNoofApplication = _investorEntity.TotalNoofApplication;
            model.InvestorSubmittedQuestionsID = _investorEntity.InvestorSubmittedQuestionsID;
            model.ProductID = _investorEntity.ProductID;
            model.Descripation = _investorEntity.Descripation;

            var _answer = db.InvestorSubmittedQuestions.Find(model.InvestorSubmittedQuestionsID);

            var splitQuestions = _answer.Answer.Split(',').ToList();
            for (int i = 0; i < splitQuestions.Count; i++)
            {
                model.Answer1 = splitQuestions[0].ToString();
                model.Answer2 = splitQuestions[1].ToString();
                model.Answer3 = splitQuestions[2].ToString();
                model.Answer4 = splitQuestions[3].ToString();
            }

            return View(model);
        }
    }
}