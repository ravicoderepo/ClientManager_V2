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
    
    public partial class VRM_InwardStockTransaction
    {
        public int InwardStockTransactionId { get; set; }
        public int StockId { get; set; }
        public int AddQuantity { get; set; }
        public string Description { get; set; }
        public string PONumber { get; set; }
        public int ReceivedFrom { get; set; }
        public string ReceivedBy { get; set; }
        public System.DateTime ReceivedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string GRNnumber { get; set; }
    }
}
