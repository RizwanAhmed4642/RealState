using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductChargesModel
    {
        [Required(ErrorMessage = "Please Select Product")]
        public Nullable<int> ProductID { get; set; }

        [Required(ErrorMessage = "Please Select Product Charges Type")]
        public Nullable<int> PChargesTypeID { get; set; }


    }

    [MetadataType(typeof(ProductChargesModel))]
    public partial class ProductCharge
    {

    }
}