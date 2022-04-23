// Decompiled with JetBrains decompiler
// Type: ClientManager.Models.SaleData
// Assembly: ClientManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A31CD02-2A37-4A80-A7EA-942AEB12790F
// Assembly location: C:\Users\kanim\Downloads\Websiteapp\httpdocs\bin\ClientManager.dll

using System;

namespace ClientManager.Models
{
  public class SaleData
  {
    public int Id { get; set; }

    public DateTime SaleDate { get; set; }

    public int Status { get; set; }

    public int SalesRepresentativeId { get; set; }

    public string ClientName { get; set; }

    public string ClientEmail { get; set; }

    public string ClientPhoneNo { get; set; }

    public string ProductName { get; set; }

    public string Capacity { get; set; }

    public string Unit { get; set; }

    public DateTime RecentCallDate { get; set; }

    public DateTime? AnticipatedClosingDate { get; set; }

    public string InvoiceNo { get; set; }

    public Decimal InvoiceAmount { get; set; }

    public DateTime? DateOfClosing { get; set; }

    public int NoOfFollowUps { get; set; }

    public string Remarks { get; set; }
  }
}
