//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EStateDevelopment.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BorrowerDocument
    {
        public int BorrowerDocumentID { get; set; }
        public byte[] UplaodedDocument { get; set; }
        public string UploadedDocumentPath { get; set; }
        public Nullable<int> BorrowerApplicationID { get; set; }
    
        public virtual BorrowerApplication BorrowerApplication { get; set; }
    }
}