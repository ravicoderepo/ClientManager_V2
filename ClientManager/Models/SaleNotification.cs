using System;

namespace ClientManager.Models
{
  public class SaleNotification
  {
    public int Id { get; set; }

    public string SaleDate { get; set; }

    public string Status { get; set; }

    public string ClientName { get; set; }

    public string ProductName { get; set; }

    public string FollowUpDate { get; set; }

    public string Remarks { get; set; }
  }
}
