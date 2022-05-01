﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBOperation
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ClientManagerEntities : DbContext
    {
        public ClientManagerEntities()
            : base("name=ClientManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ClientContact> ClientContacts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ExpenceCategory> ExpenceCategories { get; set; }
        public virtual DbSet<ExpenseTracker> ExpenseTrackers { get; set; }
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
        public virtual DbSet<UserContact> UserContacts { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<GetMonthlySalesReport_Result> GetMonthlySalesReport(string userType, Nullable<int> currentUserId, Nullable<int> userManagerId)
        {
            var userTypeParameter = userType != null ?
                new ObjectParameter("UserType", userType) :
                new ObjectParameter("UserType", typeof(string));
    
            var currentUserIdParameter = currentUserId.HasValue ?
                new ObjectParameter("CurrentUserId", currentUserId) :
                new ObjectParameter("CurrentUserId", typeof(int));
    
            var userManagerIdParameter = userManagerId.HasValue ?
                new ObjectParameter("UserManagerId", userManagerId) :
                new ObjectParameter("UserManagerId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetMonthlySalesReport_Result>("GetMonthlySalesReport", userTypeParameter, currentUserIdParameter, userManagerIdParameter);
        }
    }
}
