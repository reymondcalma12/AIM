using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class SLALevel
    {
        [Key, Column("slalevel_code")]
        public int SLALevelCode { get; set; }

        [Column("slalevel_name")]
        public string SLALevelName { get; set; }

        [Column("slalevel_status")]
        public string SLALevelStatus { get; set; }

        [Column("slalevel_createdby")]
        public string SLALevelCreatedBy { get; set; }

        [Column("slalevel_createddt")]
        public DateTime SLALevelCreatedDt { get; set; }

        [Column("slalevel_updatedby")]
        public string SLALevelUpdatedBy { get; set; }

        [Column("slalevel_updateddt")]
        public DateTime? SLALevelUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("SLALevelStatus")]
        public Status? Status { get; set; }

        [ForeignKey("SLALevelCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("SLALevelUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
