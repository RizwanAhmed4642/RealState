using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class PropertyGuruUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Email.")]
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required(ErrorMessage = "Please Enter Your Password.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Your Confirm Password.")]
        [Compare("PasswordHash",ErrorMessage ="Password & Confirm Password should be same")]
        public string ConfirmPasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public int role_id { get; set; }
        public string user_pic { get; set; }
        public string mobile { get; set; }
        [Required(ErrorMessage = "Please Select City.")]
        public string city { get; set; }
        public string tax_id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Address.")]
        public string address { get; set; }
        public string zip_code { get; set; }
        public Nullable<System.DateTime> expiry_date { get; set; }
        public Nullable<System.DateTime> last_login { get; set; }
        public Nullable<System.DateTime> last_change_pwd_date { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<bool> deleted { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name.")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Please Enter Your Phone #.")]
        public string phone { get; set; }
        public Nullable<bool> insurance_agent { get; set; }
        public string hearfrom { get; set; }
        public byte[] profile_img { get; set; }
        public string imageupload { get; set; }
        [Required(ErrorMessage = "Please Select State.")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please Enter National ID #.")]
        public string NationalID { get; set; }
        public Nullable<bool> AdminApproval { get; set; }
        public Nullable<bool> AdminRejected { get; set; }

    }
}