using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Data
{
    public class ApplicationModel
    {
       
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationNumber { get; set; }
        public string AppDescription { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string personType {get;set;}
        public Nullable<bool> AppApprovalStatus { get; set; }
        public Nullable<bool> AppRejectedStatus { get; set; }
        public string AppAdress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        
        

        public string ProductName { get; set; }
        public string RateofReturn { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ProductType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public Nullable<bool> ApprovalStatus { get; set; }
        public Nullable<bool> ProductStatus { get; set; }
        public string MinimumPeriod { get; set; }
        public string MaximumPeriod { get; set; }
        public Nullable<int> ProductTypeID { get; set; }
        public string ChargesAmount { get; set; }

        //public virtual Product Product { get; set; }
    }
}