using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class DepartmentProcessOwner
    {
        [Key, Column("rportosla_code", Order = 0)]
        public string RPortOslaCode { get; set; }

        [Key, Column("dept_code", Order = 1)]
        public int DeptCode { get; set; }

        [Column("deptproc_createdby")]
        public string DeptProcessOwnerCreatedBy { get; set; }

        [Column("deptproc_createddt")]
        public DateTime DeptProcessOwnerCreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("RPortOslaCode")]
        public RPortOsla RPortOsla { get; set; }

        [ForeignKey("DeptCode")]
        public Department Department { get; set; }

        [ForeignKey("DeptProcessOwnerCreatedBy")]
        public User CreatedBy { get; set; }
    }
}
