namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaleActivity")]
    public partial class SaleActivity
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; }

        public int Status { get; set; }

        [StringLength(100)]
        public string ClientName { get; set; }

        [StringLength(100)]
        public string ClientEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientPhoneNo { get; set; }

        public int? ProductId { get; set; }

        [StringLength(100)]
        public string ProductName { get; set; }

        [StringLength(500)]
        public string Capacity { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public DateTime RecentCallDate { get; set; }

        public DateTime? AnticipatedClosingDate { get; set; }

        public int? NoOfFollowUps { get; set; }

        [Required]
        public string Remarks { get; set; }

        public int SalesRepresentativeId { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        public decimal? InvoiceAmount { get; set; }

        public DateTime? DateOfClosing { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Product Product { get; set; }

        public virtual SalesStatu SalesStatu { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
