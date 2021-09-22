using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public decimal InvoiceAmount { get; set; }
        public DateTime? DateOfClosing { get; set; }
        public int NoOfFollowUps { get; set; }
        public string Remarks { get; set; }
    }
}