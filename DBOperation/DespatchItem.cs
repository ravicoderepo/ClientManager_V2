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
    
    public partial class DespatchItem
    {
        public int Id { get; set; }
        public int DespatchId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    
        public virtual Despatch Despatch { get; set; }
        public virtual Item Item { get; set; }
    }
}
