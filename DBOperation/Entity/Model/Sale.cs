namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sale
    {
        public Sale()
        {
            SaleProducts = new HashSet<SaleProduct>();
        }

        public int Id { get; set; }

        public DateTime SaleDate { get; set; }

        public int Status { get; set; }

        [Required]
        [StringLength(100)]
        public string AnticipatedClosing { get; set; }

        public int NoOfFollowUps { get; set; }

        public DateTime? NextFollowUpDate { get; set; }

        public int RepresentativeId { get; set; }

        [StringLength(1000)]
        public string Remarks { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<SaleProduct> SaleProducts { get; set; }

        public virtual SalesStatu SalesStatu { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
