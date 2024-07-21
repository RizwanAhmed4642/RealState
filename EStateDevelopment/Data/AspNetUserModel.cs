using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class AspNetUserModel
    {
        [Required(ErrorMessage = "Please Enter Your Email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Your Password.")]
        public string PasswordHash { get; set; }
    }
    [MetadataType(typeof(AspNetUserModel))]
    public partial class AspNetUser
    {
        public string ConfirmPasswordHash { get; set; }
    }
}