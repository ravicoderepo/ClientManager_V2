namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PettyCash")]
    public partial class PettyCash
    {
        public int Id { get; set; }

        //[Column(TypeName = "money")]
        public decimal AmountReceived { get; set; }

        public DateTime AmountRecivedDate { get; set; }

        [Required]
        [StringLength(200)]
        public string ModeOfPayment { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
