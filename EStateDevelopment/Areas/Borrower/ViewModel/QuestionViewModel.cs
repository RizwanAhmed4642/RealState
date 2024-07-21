﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Borrower.ViewModel
{
    public class QuestionViewModel
    {


        public int BorrowerQuestionID { get; set; }
        [Required(ErrorMessage = "Please Enter Question")]
        public string Question { get; set; }
        public string Control { get; set; }
        public string Priority { get; set; }
        public string Question1 { get; set; }

        
        public string Question2 { get; set; }


       
        public string Question3 { get; set; }



        public string Question4 { get; set; }

    }
}