// Decompiled with JetBrains decompiler
// Type: ClientManager.Models.Dashboard
// Assembly: ClientManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A31CD02-2A37-4A80-A7EA-942AEB12790F
// Assembly location: C:\Users\kanim\Downloads\Websiteapp\httpdocs\bin\ClientManager.dll

using System.Collections.Generic;

namespace ClientManager.Models
{
    public class Dashboard
    {
        //Finance
        public decimal MonthlyTotalPettyCash { get; set; }
        public decimal MonthlyAvailablePettyCash { get; set; }
        public decimal MonthlyUnApprovedExpenses { get; set; }
        public decimal MonthlyVerifiedExpenses { get; set; }
        public decimal MonthlyUnVerifiedExpenses { get; set; }

        public decimal TotalPettyCash { get; set; }
        public decimal AvailablePettyCash { get; set; }
        public decimal UnApprovedExpenses { get; set; }
        public decimal VerifiedExpenses { get; set; }
        public decimal UnVerifiedExpenses { get; set; }

        public string CurrentMonthAndYear { get; set; }
        //Others
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
}
