using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class BorrowerQuestionModel
    {
    }
    [MetadataType(typeof(BorrowerQuestionModel))]
    public partial class BorrowerQuestion
    {
        public virtual ICollection<BorrowerOptionQuestion> Borroweroptionquestion { get; set; }
    }
}

