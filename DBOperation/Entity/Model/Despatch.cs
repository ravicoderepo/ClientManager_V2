namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Despatch")]
    public partial class Despatch
    {
        public Despatch()
        {
            DespatchItems = new HashSet<DespatchItem>();
        }

        public int Id { get; set; }

        public int OutwardId { get; set; }

        [Required]
        [StringLength(50)]
        public string DespatchNo { get; set; }

        public DateTime DespatchDate { get; set; }

        [Required]
        [StringLength(50)]
        public string LRNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string TransportBy { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipToCity { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Outward Outward { get; set; }

        public virtual ICollection<DespatchItem> DespatchItems { get; set; }
    }
}
