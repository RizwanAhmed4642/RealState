using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Investors.ViewModel
{
    public class InvestorApplicationViewModel
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
        public Nullable<int> InvestorSubmittedQuestionsID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Descripation { get; set; }
        public string Answer { get; set; }
       
        public string Answer1 { get; set; }
   
        public string Answer2 { get; set; }

        public string Answer3 { get; set; }
       
        public string Answer4 { get; set; }

    }
}