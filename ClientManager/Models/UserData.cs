// Decompiled with JetBrains decompiler
// Type: ClientManager.Models.UserData
// Assembly: ClientManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A31CD02-2A37-4A80-A7EA-942AEB12790F
// Assembly location: C:\Users\kanim\Downloads\Websiteapp\httpdocs\bin\ClientManager.dll

using System;

namespace ClientManager.Models
{
  public class UserData
  {
    public int Id { get; set; }

    public string UserId { get; set; }

    public bool IsActive { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string EmpId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? DateofJoining { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string State { get; set; }

    public string City { get; set; }

    public string PinCode { get; set; }

    public int? SaleTarget { get; set; }

    public int? ReportingManager { get; set; }
  }
}
