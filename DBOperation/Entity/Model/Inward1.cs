namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ravidev.Inward")]
    public partial class Inward1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InwardStockTransactionId { get; set; }

        public int StockId { get; set; }

        public int AddQuantity { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string PONumber { get; set; }

        public int ReceivedFrom { get; set; }

        [Required]
        [StringLength(500)]
        public string ReceivedBy { get; set; }

        public DateTime ReceivedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        [StringLength(500)]
        public string GRNnumber { get; set; }
    }
}
