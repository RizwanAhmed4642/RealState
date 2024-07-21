using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductChargesTypesModel
    {
        

        [Required(ErrorMessage = "Please Select Charges Slab")]
        public int ChargesSlabID { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string ChargesName { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> EndDate { get; set; }
    }

    [MetadataType(typeof(ProductChargesTypesModel))]
    public partial class ProductChargesType
    {
        public ICollection<ChargesSlab> ChargesSlabs { get; set; }
    }

}