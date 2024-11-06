using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Department
    {
        [Key, Column("dept_code")]
        public int DeptCode { get; set; }

        [Column("dept_name")]
        public string DeptName { get; set; }

        [Column("dept_status")]
        public string DeptStatus { get; set; }

        [Column("dept_createdby")]
        public string DeptCreatedBy { get; set; }

        [Column("dept_createddt")]
        public DateTime DeptCreatedDt { get; set; }

        [Column("dept_updatedby")]
        public string DeptUpdatedBy { get; set; }

        [Column("dept_updateddt")]
        public DateTime? DeptUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("DeptStatus")]
        public Status? Status { get; set; }

        [ForeignKey("DeptCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("DeptUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
