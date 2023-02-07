namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VRM_OutwardStockTransaction
    {
        [Key]
        public int OutwardStackDetailId { get; set; }

        public int OutwardId { get; set; }

        public int StockId { get; set; }

        public int OutwardQuantity { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
