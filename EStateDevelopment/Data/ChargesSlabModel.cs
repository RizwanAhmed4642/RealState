using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ChargesSlabModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string SlabName { get; set; }

        [Required(ErrorMessage = "Minimum Amount Required")]
        public string MinAmount { get; set; }

        [Required(ErrorMessage = "Max Amount Required")]
        public string MaxAmount { get; set; }

        [Required(ErrorMessage = "Charges Amount Required")]
        public string ChargesAmount { get; set; }
    }


    [MetadataType(typeof(ChargesSlabModel))]
    public partial class ChargesSlab
    {

    }
}