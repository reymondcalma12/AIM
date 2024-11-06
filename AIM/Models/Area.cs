using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Area
    {
        [Key, Column("area_code")]
        public int AreaCode { get; set; }

        [Column("area_name")]
        public string AreaName { get; set; }

        [Column("area_status")]
        public string AreaStatus { get; set; }

        [Column("area_createdby")]
        public string AreaCreatedBy { get; set; }

        [Column("area_createddt")]
        public DateTime AreaCreatedDt { get; set; }

        [Column("area_updatedby")]
        public string AreaUpdatedBy { get; set; }

        [Column("area_updateddt")]
        public DateTime? AreaUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("AreaStatus")]
        public Status? Status { get; set; }

        [ForeignKey("AreaCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("AreaUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
