namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public User()
        {
            ClientContacts = new HashSet<ClientContact>();
            ClientContacts1 = new HashSet<ClientContact>();
            Clients = new HashSet<Client>();
            Clients1 = new HashSet<Client>();
            Contacts = new HashSet<Contact>();
            Contacts1 = new HashSet<Contact>();
            Documents = new HashSet<Document>();
            Documents1 = new HashSet<Document>();
            ExpenceCategories = new HashSet<ExpenceCategory>();
            ExpenceCategories1 = new HashSet<ExpenceCategory>();
            ExpenseTrackers = new HashSet<ExpenseTracker>();
            ExpenseTrackers1 = new HashSet<ExpenseTracker>();
            Outwards = new HashSet<Outward>();
            Outwards1 = new HashSet<Outward>();
            PettyCashes = new HashSet<PettyCash>();
            PettyCashes1 = new HashSet<PettyCash>();
            Projects = new HashSet<Project>();
            RepresentativeSaleTargets = new HashSet<RepresentativeSaleTarget>();
            RepresentativeSaleTargets1 = new HashSet<RepresentativeSaleTarget>();
            Roles = new HashSet<Role>();
            Roles1 = new HashSet<Role>();
            SaleActivities = new HashSet<SaleActivity>();
            SaleActivities1 = new HashSet<SaleActivity>();
            SaleActivities2 = new HashSet<SaleActivity>();
            Sales = new HashSet<Sale>();
            Sales1 = new HashSet<Sale>();
            UserContacts = new HashSet<UserContact>();
            UserContacts1 = new HashSet<UserContact>();
            UserContacts2 = new HashSet<UserContact>();
            UserRoles = new HashSet<UserRole>();
            UserRoles1 = new HashSet<UserRole>();
            UserRoles2 = new HashSet<UserRole>();
            UserRoles3 = new HashSet<UserRole>();
            Users1 = new HashSet<User>();
            Users11 = new HashSet<User>();
            Users12 = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoining { get; set; }

        [StringLength(50)]
        public string EmployeeId { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Pincode { get; set; }

        public int? ReportingManager { get; set; }

        public int? SaleTarget { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<ClientContact> ClientContacts { get; set; }

        public virtual ICollection<ClientContact> ClientContacts1 { get; set; }

        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<Client> Clients1 { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<Contact> Contacts1 { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<Document> Documents1 { get; set; }

        public virtual ICollection<ExpenceCategory> ExpenceCategories { get; set; }

        public virtual ICollection<ExpenceCategory> ExpenceCategories1 { get; set; }

        public virtual ICollection<ExpenseTracker> ExpenseTrackers { get; set; }

        public virtual ICollection<ExpenseTracker> ExpenseTrackers1 { get; set; }

        public virtual ICollection<Outward> Outwards { get; set; }

        public virtual ICollection<Outward> Outwards1 { get; set; }

        public virtual ICollection<PettyCash> PettyCashes { get; set; }

        public virtual ICollection<PettyCash> PettyCashes1 { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<RepresentativeSaleTarget> RepresentativeSaleTargets { get; set; }

        public virtual ICollection<RepresentativeSaleTarget> RepresentativeSaleTargets1 { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ICollection<Role> Roles1 { get; set; }

        public virtual ICollection<SaleActivity> SaleActivities { get; set; }

        public virtual ICollection<SaleActivity> SaleActivities1 { get; set; }

        public virtual ICollection<SaleActivity> SaleActivities2 { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<Sale> Sales1 { get; set; }

        public virtual ICollection<UserContact> UserContacts { get; set; }

        public virtual ICollection<UserContact> UserContacts1 { get; set; }

        public virtual ICollection<UserContact> UserContacts2 { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UserRole> UserRoles1 { get; set; }

        public virtual ICollection<UserRole> UserRoles2 { get; set; }

        public virtual ICollection<UserRole> UserRoles3 { get; set; }

        public virtual ICollection<User> Users1 { get; set; }

        public virtual User User1 { get; set; }

        public virtual ICollection<User> Users11 { get; set; }

        public virtual User User2 { get; set; }

        public virtual ICollection<User> Users12 { get; set; }

        public virtual User User3 { get; set; }
    }
}
