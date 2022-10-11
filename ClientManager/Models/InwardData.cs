using System;

namespace ClientManager.Models
{
    public class InwardData
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int AvailableQuantity { get; set; }
        public int Quantity { get; set; }
        public int InwardStockTransactionId { get; set; }
       // public int AddQuantity { get; set; }
        public string Description { get; set; }
        public string PONumber { get; set; }
        public int ReceivedFrom { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string GRNnumber { get; set; }
    }
}
