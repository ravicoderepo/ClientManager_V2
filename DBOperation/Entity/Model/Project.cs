namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public Project()
        {
            ProjectClients = new HashSet<ProjectClient>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public int? Status { get; set; }

        public int ClientCompany { get; set; }

        public int ProjectLead { get; set; }

        public decimal EstimatedBudget { get; set; }

        public decimal TotalAmountSpent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EstimatedEndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<ProjectClient> ProjectClients { get; set; }

        public virtual ProjectStatu ProjectStatu { get; set; }

        public virtual User User { get; set; }
    }
}
