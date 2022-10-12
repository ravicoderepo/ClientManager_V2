using System;

namespace ClientManager.Models
{
    public class OutwardItemData
    {
        public int Id { get; set; }
        public int OutwardId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string MaterialName { get; set; }
        public string TypeName { get; set; }
        public string ItemName { get; set; }
    }
}
