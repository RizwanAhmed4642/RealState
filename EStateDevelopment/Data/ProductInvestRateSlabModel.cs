using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductInvestRateSlabModel
    {
        [Required(ErrorMessage = "Please Select Product")]
        public Nullable<int> ProductID { get; set; }


        [Required(ErrorMessage = "Please Select Product Charges")]
        public Nullable<int> PChargesID { get; set; }

        [Required(ErrorMessage = "Please Enter Interest Rate")]
        public string IntrestRate { get; set; }

        [Required(ErrorMessage = "Please Enter Tenor")]
        public string Tenor { get; set; }
    }

    [MetadataType(typeof(ProductInvestRateSlabModel))]
    public partial class ProductInteresRateSlab
    {

    }
}