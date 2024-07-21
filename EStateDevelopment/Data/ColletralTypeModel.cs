using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ColletralTypeModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "Details Required")]
        public string ColleteralDetail { get; set; }

        [Required(ErrorMessage = "Please Select Product")]
        public Nullable<int> ProductID { get; set; }

    }

    [MetadataType(typeof(ColletralTypeModel))]
    public partial class CollateralType
    {

    }
}