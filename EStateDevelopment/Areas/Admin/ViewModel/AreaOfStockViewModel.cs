using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Admin.ViewModel
{
    public class AreaOfStockViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Area of Stock")]
        public string Name { get; set; }
    }
}