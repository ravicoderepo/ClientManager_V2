namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public Contact()
        {
            ClientContacts = new HashSet<ClientContact>();
        }

        public int Id { get; set; }

        public int ContactType { get; set; }

        [Required]
        [StringLength(50)]
        public string ContactPersonName { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(10)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Website { get; set; }

        [StringLength(50)]
        public string WorkPhoneNo { get; set; }

        [StringLength(50)]
        public string PersonalPhoneNo { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<ClientContact> ClientContacts { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
