namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DespatchItem
    {
        public int Id { get; set; }

        public int DespatchId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public virtual Despatch Despatch { get; set; }

        public virtual Item Item { get; set; }
    }
}
