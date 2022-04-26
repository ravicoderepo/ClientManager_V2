using System;

namespace ClientManager.Models
{
    public class ExpenceTrackerData
    {
        public int Id { get; set; }
        public decimal ExpenseAmount { get; set; }
        public string ExpenseDate { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
