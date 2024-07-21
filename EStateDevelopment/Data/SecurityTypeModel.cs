using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class SecurityTypeModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string SecurityName { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "Details Required")]
        public string SecurityDetail { get; set; }

        [Required(ErrorMessage = "Please Select Product")]
        public Nullable<int> ProductID { get; set; }
     
    }
    [MetadataType(typeof(SecurityTypeModel))]
    public partial class SecurityType
    {

    }
}