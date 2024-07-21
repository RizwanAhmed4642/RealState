using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Borrower.ViewModel
{
    public class BorrowerSubmittedQuestionsViewModel
    {

        public int BorrowerSubmittedQuestionsID { get; set; }
        public Nullable<int> BorrowerQuestionID { get; set; }
        [Required(ErrorMessage = "Please Enter Answer ")]
        // public string Answer { get; set; }
        public List<string> Answers { get; set; }


        public string Answer4 { get; set; }
        public int InvesterQuestionsID { get; set; }
        public string Question { get; set; }


    }
}