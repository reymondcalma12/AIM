using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppDependency
    {
        [Key, Column("rportosla_code", Order = 0)]
        public string RPortOslaCode { get; set; }

        [Key, Column("app_code", Order = 1)]
        public string AppCode { get; set; }

        [Column("dependency_createdby")]
        public string DependencyCreatedBy { get; set; }

        [Column("dependency_createddt")]
        public DateTime DependencyCreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("RPortOslaCode")]
        public RPortOsla? RPortOsla { get; set; }

        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        [ForeignKey("DependencyCreatedBy")]
        public User? CreatedBy { get; set; }
    }
}
