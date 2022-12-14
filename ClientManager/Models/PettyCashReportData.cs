using DBOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class PettyCashReportData
    {
        public int Id { get; set; }
        public decimal ExpenseAmount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}