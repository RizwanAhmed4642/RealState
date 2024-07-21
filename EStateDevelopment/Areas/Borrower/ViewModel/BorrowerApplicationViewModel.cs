using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Borrower.ViewModel
{
    public class BorrowerApplicationViewModel
    {
        public int InvestorApplicationID { get; set; }
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }
        public string CompleteAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string WorkPhone { get; set; }
        [Required(ErrorMessage = "Please Enter Phone Number")]
        public string Mobile { get; set; }
        public string TotalNoofApplication { get; set; }
        public string ApplicationNumber { get; set; }
        public Nullable<int> BorrowerSubmittedQuestionsID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Descripation { get; set; }
    }
}