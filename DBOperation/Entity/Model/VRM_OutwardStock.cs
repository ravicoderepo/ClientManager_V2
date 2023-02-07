namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VRM_OutwardStock
    {
        [Key]
        public int OutwardId { get; set; }

        [Required]
        [StringLength(500)]
        public string InvoiceNumber { get; set; }

        public DateTime OutwardDate { get; set; }

        public bool IsActive { get; set; }

        public string CustomerInformation { get; set; }
    }
}
