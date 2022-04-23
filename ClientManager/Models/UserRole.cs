// Decompiled with JetBrains decompiler
// Type: ClientManager.Models.UserRole
// Assembly: ClientManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A31CD02-2A37-4A80-A7EA-942AEB12790F
// Assembly location: C:\Users\kanim\Downloads\Websiteapp\httpdocs\bin\ClientManager.dll

using System;

namespace ClientManager.Models
{
  public class UserRole
  {
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; }
  }
}
