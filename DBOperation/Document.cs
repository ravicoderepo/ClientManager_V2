//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public int Id { get; set; }
        public string DocumentSource { get; set; }
        public int ReferenceRecId { get; set; }
        public string DocumentType { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileData { get; set; }
        public string URL { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}