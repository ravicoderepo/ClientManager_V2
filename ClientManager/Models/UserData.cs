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
