namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        public Item()
        {
            DespatchItems = new HashSet<DespatchItem>();
            VRM_InwardStock = new HashSet<VRM_InwardStock>();
        }

        public int ItemId { get; set; }

        public int? ParentId { get; set; }

        public int MaterialId { get; set; }

        public int TypeId { get; set; }

        [Required]
        [StringLength(500)]
        public string ItemName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? MinimumAvailableQuantity { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<DespatchItem> DespatchItems { get; set; }

        public virtual Material Material { get; set; }

        public virtual Type Type { get; set; }

        public virtual ICollection<VRM_InwardStock> VRM_InwardStock { get; set; }
    }
}
