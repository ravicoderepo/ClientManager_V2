using System;

namespace ClientManager.Models
{
    public class ItemData
    {
        public int ItemId { get; set; }
        public int MaterialId { get; set; }
        public int TypeId { get; set; }
        public int ParentId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? MinimumAvailableQuantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class ItemSummaryData
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MaterialName { get; set; }
        public string TypeName { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
