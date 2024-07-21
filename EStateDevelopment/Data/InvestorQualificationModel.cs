using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class InvestorQualificationModel
    {
      
        public string Name { get; set; }


        public string Adress { get; set; }


        public string Phone { get; set; }


        public string Fax { get; set; }


        public string Email { get; set; }


        public Nullable<bool> Income { get; set; }


        public Nullable<bool> NetWorth { get; set; }

        public string Occupation { get; set; }


        public string BusinessorFinancialExperience { get; set; }

    
        public string DegreeandDesignations { get; set; }

        public string CurrentNoOfProperties { get; set; }

        public string CurrentTypesOfProperties { get; set; }

        public string PriorNoOfProperties { get; set; }


        public string PriorTypesOfProperties { get; set; }


        public string RealEstateExperience { get; set; }


        public string InvestmentValue { get; set; }


        public string PreviousBusiness { get; set; }


        public string Memberof { get; set; }


        public string HomebuyersofGA { get; set; }


        public int InvestorID { get; set; }


        public string Signature { get; set; }


        public Nullable<System.DateTime> Date { get; set; }


        public string ConfirmSignature { get; set; }


        public Nullable<System.DateTime> ConfirmDate { get; set; }

    }

    [MetadataType(typeof(InvestorQualificationModel))]
    public partial class InvestorQualification
    {

    }
}