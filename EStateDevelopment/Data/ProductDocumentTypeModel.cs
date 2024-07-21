using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductDocumentTypeModel
    {
        [Required(ErrorMessage = "Please Enter Product Doc Type")]
        public string DocumentType { get; set; }
    }

    [MetadataType(typeof(ProductDocumentTypeModel))]
    public partial class ProductDocumentType
    {

    }
}