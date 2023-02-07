namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Client
    {
        public Client()
        {
            ClientContacts = new HashSet<ClientContact>();
            ProjectClients = new HashSet<ProjectClient>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactPersonName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<ClientContact> ClientContacts { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual ICollection<ProjectClient> ProjectClients { get; set; }
    }
}
