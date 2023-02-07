namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SalesStatu
    {
        public SalesStatu()
        {
            SaleActivities = new HashSet<SaleActivity>();
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<SaleActivity> SaleActivities { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
