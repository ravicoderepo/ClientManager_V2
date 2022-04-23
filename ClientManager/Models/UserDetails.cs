// Decompiled with JetBrains decompiler
// Type: ClientManager.Models.UserDetails
// Assembly: ClientManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A31CD02-2A37-4A80-A7EA-942AEB12790F
// Assembly location: C:\Users\kanim\Downloads\Websiteapp\httpdocs\bin\ClientManager.dll

using System;
using System.Collections.Generic;

namespace ClientManager.Models
{
  public class UserDetails
  {
    public int Id { get; set; }

    public string Email { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int? ModifiedBy { get; set; }

    public string FullName { get; set; }

    public int? ReportingManager { get; set; }

    public List<int> ReportingToMe { get; set; }

    public List<UserRole> UserRoles { get; set; }
  }
}
