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
    
    public partial class ExpenseTracker
    {
        public int Id { get; set; }
        public decimal ExpenseAmount { get; set; }
        public System.DateTime ExpenseDate { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
    
        public virtual ExpenceCategory ExpenceCategory { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
