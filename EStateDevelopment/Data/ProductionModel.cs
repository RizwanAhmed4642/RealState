using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductionModel
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Description Required")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> ProductStartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> ProductEndDate { get; set; }

        [Required(ErrorMessage = "Intrest Rate is Required")]
        public string RateofReturn { get; set; }

        [Required(ErrorMessage = "Please Select Product Type")]
        public Nullable<int> ProductTypeID { get; set; }

        [Required(ErrorMessage = "Please Select Area of stock")]
        public Nullable<int> AreaOfStock { get; set; }

        [Required(ErrorMessage = "Please Select an Image")]
        public HttpPostedFileBase ImagePath { get; set; }

        [Required(ErrorMessage = "Please Select Max Tenure")]
        public string MinimumPeriod { get; set; }
        [Required(ErrorMessage = "Please Select Min Tenure")]
        public string MaximumPeriod { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string ColletralName { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> ColletrallStartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> ColletrallEndDate { get; set; }

        [Required(ErrorMessage = "Details Required")]
        public string ColleteralDetail { get; set; }


        [Required(ErrorMessage = "Please Enter Name")]
        public string SlabName { get; set; }

        [Required(ErrorMessage = "Minimum Amount Required")]
        public string MinAmount { get; set; }

        [Required(ErrorMessage = "Max Amount Required")]
        public string MaxAmount { get; set; }

        [Required(ErrorMessage = "Charges Amount Required")]
        public string ChargesAmount { get; set; }

        [Required(ErrorMessage = "Please Enter Name")]
        public string ChargesName { get; set; }

        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> ChargesStartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> ChargesEndDate { get; set; }

        [Required(ErrorMessage = "Please Enter Interest Rate")]
        public string IntrestRate { get; set; }

        [Required(ErrorMessage = "Rate of Return")]
        public string Tenor { get; set; }

    }

}