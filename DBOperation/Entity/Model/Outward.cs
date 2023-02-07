namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Outward")]
    public partial class Outward
    {
        public Outward()
        {
            Despatches = new HashSet<Despatch>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        
        [StringLength(50)]
        public string LRNumber { get; set; }

        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(500)]
        public string CustomerAddress { get; set; }

        [StringLength(50)]
        public string CustomerPhoneNumber { get; set; }

        [StringLength(50)]
        public string CustomerEmailId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<Despatch> Despatches { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
