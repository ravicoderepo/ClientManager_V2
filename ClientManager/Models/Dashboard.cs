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
