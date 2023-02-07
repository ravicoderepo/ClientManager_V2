using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DBOperation
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ClientManagerEntities : DbContext
    {
        public ClientManagerEntities()
            : base("ClientManagerEntities")
        {
        }
        public virtual DbSet<ClientContact> ClientContacts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Despatch> Despatches { get; set; }
        public virtual DbSet<DespatchItem> DespatchItems { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ExpenceCategory> ExpenceCategories { get; set; }
        public virtual DbSet<ExpenseTracker> ExpenseTrackers { get; set; }
        public virtual DbSet<Inward> Inwards { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Outward> Outwards { get; set; }
        public virtual DbSet<PettyCash> PettyCashes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProjectClient> ProjectClients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStatu> ProjectStatus { get; set; }
        public virtual DbSet<RepresentativeSaleTarget> RepresentativeSaleTargets { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SaleActivity> SaleActivities { get; set; }
        public virtual DbSet<SaleProduct> SaleProducts { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalesStatu> SalesStatus { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<UserContact> UserContacts { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VRM_ErrorLog> VRM_ErrorLog { get; set; }
        public virtual DbSet<VRM_InwardStock> VRM_InwardStock { get; set; }
        public virtual DbSet<VRM_InwardStockTransaction> VRM_InwardStockTransaction { get; set; }
        public virtual DbSet<VRM_OutwardStock> VRM_OutwardStock { get; set; }
        public virtual DbSet<VRM_OutwardStockTransaction> VRM_OutwardStockTransaction { get; set; }
        public virtual DbSet<Inward1> Inward1 { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.ClientContacts)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .HasMany(e => e.ClientContacts)
                .WithRequired(e => e.Contact)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Despatch>()
                .HasMany(e => e.DespatchItems)
                .WithRequired(e => e.Despatch)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExpenceCategory>()
                .HasMany(e => e.ExpenseTrackers)
                .WithRequired(e => e.ExpenceCategory)
                .HasForeignKey(e => e.ExpenseCategoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExpenseTracker>()
                .Property(e => e.ExpenseAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.DespatchItems)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.VRM_InwardStock)
                .WithRequired(e => e.Item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.Types)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.VRM_InwardStock)
                .WithRequired(e => e.Material)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Outward>()
                .HasMany(e => e.Despatches)
                .WithRequired(e => e.Outward)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PettyCash>()
                .Property(e => e.AmountReceived)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.SaleProducts)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.EstimatedBudget)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Project>()
                .Property(e => e.TotalAmountSpent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ProjectStatu>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.ProjectStatu)
                .HasForeignKey(e => e.Status);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleActivity>()
                .Property(e => e.InvoiceAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.SaleProducts)
                .WithRequired(e => e.Sale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesStatu>()
                .HasMany(e => e.SaleActivities)
                .WithRequired(e => e.SalesStatu)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesStatu>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.SalesStatu)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Type>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Type>()
                .HasMany(e => e.VRM_InwardStock)
                .WithRequired(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClientContacts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ClientContacts1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Clients)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Clients1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Contacts)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Contacts1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Documents)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Documents1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExpenceCategories)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExpenceCategories1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExpenseTrackers)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ExpenseTrackers1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Outwards)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Outwards1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PettyCashes)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PettyCashes1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.ProjectLead)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RepresentativeSaleTargets)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RepresentativeSaleTargets1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SaleActivities)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.SalesRepresentativeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SaleActivities1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SaleActivities2)
                .WithRequired(e => e.User2)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.RepresentativeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sales1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserContacts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserContacts1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserContacts2)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles2)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles3)
                .WithRequired(e => e.User3)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ModifiedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users11)
                .WithOptional(e => e.User2)
                .HasForeignKey(e => e.ReportingManager);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users12)
                .WithOptional(e => e.User3)
                .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<VRM_ErrorLog>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<VRM_ErrorLog>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<VRM_ErrorLog>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);
        }
    }
}
