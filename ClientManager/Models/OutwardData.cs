using System;
using System.Collections.Generic;

namespace ClientManager.Models
{
    public class OutwardData
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string LRNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmailId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Comments { get; set; }

        //public OutwardItemData OutwardItemData { get; set; }
        public IEnumerable<OutwardItemData> OutwardItems { get; set; }
    }
}