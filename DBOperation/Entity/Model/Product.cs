namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public Product()
        {
            SaleActivities = new HashSet<SaleActivity>();
            SaleProducts = new HashSet<SaleProduct>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        public string ProductImage { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime PurchasedDate { get; set; }

        public DateTime ExpiredOn { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<SaleActivity> SaleActivities { get; set; }

        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
