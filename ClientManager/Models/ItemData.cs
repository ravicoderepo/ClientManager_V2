using System;

namespace ClientManager.Models
{
    public class ItemData
    {
        public int ItemId { get; set; }
        public int MaterialId { get; set; }
        public int TypeId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? MinimumAvailableQuantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
