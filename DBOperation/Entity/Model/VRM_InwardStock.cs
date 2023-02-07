namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VRM_InwardStock
    {
        [Key]
        public int StockId { get; set; }

        public int MaterialId { get; set; }

        public int TypeId { get; set; }

        public int ItemId { get; set; }

        public int AvailableQuantity { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public virtual Item Item { get; set; }

        public virtual Material Material { get; set; }

        public virtual Type Type { get; set; }
    }
}
