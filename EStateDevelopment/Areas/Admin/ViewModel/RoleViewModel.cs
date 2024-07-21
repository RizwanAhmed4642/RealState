using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Admin.ViewModel
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Role")]
        public string Name { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}