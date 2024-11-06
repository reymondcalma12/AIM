using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class BusinessImpact
    {
        [Key, Column("impact_code")]
        public int ImpactCode { get; set; }

        [Column("impact_name")]
        public string ImpactName { get; set; }

        [Column("impact_status")]
        public string ImpactStatus { get; set; }

        [Column("impact_createdby")]
        public string ImpactCreatedBy { get; set; }

        [Column("impact_createddt")]
        public DateTime? ImpactCreatedDt { get; set; }

        [Column("impact_updatedby")]
        public string? ImpactUpdatedBy { get; set; }

        [Column("impact_updateddt")]
        public DateTime? ImpactUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("ImpactStatus")]
        public Status? Status { get; set; }

        [ForeignKey("ImpactCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("ImpactUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
