using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Investors.ViewModel
{
    public class InvestorSubmittedQuestionsViewModel
    {
        public int InvestorSubmittedQuestionsID { get; set; }
        public Nullable<int> InvestorQuestionID { get; set; }
        [Required(ErrorMessage = "Please Enter Answer ")]
       // public string Answer { get; set; }
        public List<string> Answer { get; set; }
        public List<string> InvestorOptionQuestion { get; set; }
      
        public string Answer4 { get; set; }

        public bool IsCheck { get; set; }
       // public int InvesterQuestionsID { get; set; }
        public string Question { get; set; }
        //public bool IsCheck { get; set; }
        public string AreaOfStock { get; set; }
        public int AreaOfStockId { get; set; }


    }
}