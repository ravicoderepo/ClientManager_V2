namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExpenseTracker")]
    public partial class ExpenseTracker
    {
        public int Id { get; set; }

        //[Column(TypeName = "money")]
        public decimal ExpenseAmount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public int ExpenseCategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ExpenceCategory ExpenceCategory { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
