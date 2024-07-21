using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductTypeModel
    {
        [Required(ErrorMessage = "Please Enter Product Type")]
        public string TypeName { get; set; }
    }

    [MetadataType(typeof(ProductTypeModel))]
    public partial class ProductType
    {

    }
}