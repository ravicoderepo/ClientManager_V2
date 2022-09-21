using System;

namespace ClientManager.Models
{
    public class MaterialData
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
