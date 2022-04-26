using System;

namespace ClientManager.Models
{
    public class PettyCashData
    {
        public int Id { get; set; }
        public decimal AmountReceived { get; set; }
        public DateTime AmountRecivedDate { get; set; }
        public string ModeOfPayment { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
