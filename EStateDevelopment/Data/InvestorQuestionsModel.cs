using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class InvestorQuestionsModel
    {
    }
    [MetadataType(typeof(InvestorQuestionsModel))]
    public partial class InvesterQuestion
    {
        public virtual ICollection<InvestorOptionQuestion> Investoroptionquestion { get; set; }
    }
}