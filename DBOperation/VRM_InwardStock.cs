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
    
    public partial class VRM_InwardStock
    {
        public int StockId { get; set; }
        public int MaterialId { get; set; }
        public int TypeId { get; set; }
        public int ItemId { get; set; }
        public int AvailableQuantity { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual Type Type { get; set; }
        public virtual Item Item { get; set; }
    }
}
