using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class DespatchData
    {
        public int Id { get; set; }
        public int OutwardId { get; set; }
        public string DespatchNo { get; set; }
        public System.DateTime DespatchDate { get; set; }
        public string LRNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string TransportBy { get; set; }
        public string ShipToCity { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

        public IEnumerable<DespatchItemData> DespatchItems { get; set; }
    }
}