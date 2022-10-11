using System;

namespace ClientManager.Models
{
    public class OutwardStockData
    {
        public int OutwardId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime OutwardDate { get; set; }
        public bool IsActive { get; set; }
        public string CustomerInformation { get; set; }
    }

    public class OutwardTransactionData
    {
        public int OutwardStackDetailId { get; set; }
        public int OutwardId { get; set; }
        public int StockId { get; set; }
        public int OutwardQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
