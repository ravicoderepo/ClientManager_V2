namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClientContact
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ContactId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Client Client { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
