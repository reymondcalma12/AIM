using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppBusinessImpact
    {
        [Key, Column("rportosla_code", Order = 0)]
        public string RPortOslaCode { get; set; }

        [Key, Column("impact_code", Order = 1)]
        public int ImpactCode { get; set; }

        [Column("impact_createdby")]
        public string ImpactCreatedBy { get; set; }

        [Column("impact_createddt")]
        public DateTime ImpactCreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("RPortOslaCode")]
        public RPortOsla? RPortOsla { get; set; }

        [ForeignKey("ImpactCode")]
        public BusinessImpact? BusinessImpact { get; set; } // Corrected here

        [ForeignKey("ImpactCreatedBy")]
        public User? CreatedBy { get; set; }
    }
}
