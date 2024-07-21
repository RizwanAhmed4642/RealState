using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ProductModel
    {

        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }
     
        [Required(ErrorMessage = "Start Date is Required")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date is Required")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Intrest Rate is Required")]
        public string RateofReturn { get; set; }
     
        [Required(ErrorMessage = "Please Select Product Type")]
        public Nullable<int> ProductTypeID { get; set; }

        [Required(ErrorMessage = "Please Select Area of stock")]
        public Nullable<int> AreaOfStock { get; set; }

        public string ProductType { get; set; }

       
        [Required(ErrorMessage = "Please Select Max Tenure")]
        public string MinimumPeriod { get; set; }
        [Required(ErrorMessage = "Please Select Min Tenure")]
        public string MaximumPeriod { get; set; }

   
    }
    [MetadataType(typeof(ProductModel))]
    public partial class Product
    {
        public ICollection<ProductCharge> ProductCharges { get; set; }
        public ICollection<AspNetUser> AspNetUsers { get; set; }
        public ICollection<BorrowerApplication> BorrowerApplications { get; set; }
        public ICollection<InvestorApplication> InvestorApplications { get; set; }
        public virtual ICollection<ApplicationModel> ApplicationModels { get; set; }

        public virtual ICollection<CollateralType> CollateralTypes { get; set; }
        public ICollection<AreaOfStock> AreaOfStocks { get; set; }

        public HttpPostedFileBase ImagePath { get; set; }
    }
}