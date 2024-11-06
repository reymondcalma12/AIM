using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppDeptProcOwner
    {
        [Key, Column("app_code", Order = 0)]
        [StringLength(15)]
        [Required]
        public string AppCode { get; set; }

        [Key, Column("dept_code", Order = 1)]
        [Required]
        public int DeptCode { get; set; }

        [Column("created_by")]
        [StringLength(15)]
        [Required]
        public string CreatedBy { get; set; }

        [Column("created_dt")]
        [Required]
        public DateTime CreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        [ForeignKey("DeptCode")]
        public Department? Department { get; set; }

        [ForeignKey("CreatedBy")]
        public User? User { get; set; }
    }
}
