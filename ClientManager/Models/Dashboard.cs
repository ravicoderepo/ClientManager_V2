using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class Dashboard
    {
        public int? TotalCalls { get; set; }
        public int CancelledRate { get; set; }
        public int TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int Closed { get; set; }
        public int InitialCall { get; set; }
        public int InDiscussion { get; set; }
        public int PendingfromCustomer { get; set; }
        public int POReceivedWIP { get; set; }
        public MonthlySalesReport MonthlySalesReport { get; set; }
        public List<MonthlySummaryReport> MonthlySummaryReportData { get; set; }
    }

    public class MonthlySalesReport
    {
        public int[] Mname { get; set; }
        public int[] Calls { get; set; }
        public int[] Orders { get; set; }
        public int[] Cancels { get; set; }
    }
    public class MonthlySummaryReport
    {

    }
}