using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Borrower.ViewModel
{
    public class BorrowerProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ProductType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string RejectedBy { get; set; }
        public Nullable<int> UserID { get; set; }
        public string UserEmail { get; set; }
        public string RateofReturn { get; set; }
        public Nullable<bool> ProductStatus { get; set; }
        public string MinimumPeriod { get; set; }
        public string MaximumPeriod { get; set; }
        public Nullable<int> ProductTypeID { get; set; }
        public Nullable<int> BorrowerSubmittedQuestionsID { get; set; }
    }
}