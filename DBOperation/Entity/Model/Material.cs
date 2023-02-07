namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Material
    {
        public Material()
        {
            Items = new HashSet<Item>();
            Types = new HashSet<Type>();
            VRM_InwardStock = new HashSet<VRM_InwardStock>();
        }

        public int MaterialId { get; set; }

        [Required]
        [StringLength(500)]
        public string MaterialName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual ICollection<Type> Types { get; set; }

        public virtual ICollection<VRM_InwardStock> VRM_InwardStock { get; set; }
    }
}
