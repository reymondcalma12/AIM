using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppLocation
    {
        [Key, Column("location_code")]
        public int LocationCode { get; set; }

        [Column("location_name")]
        public string LocationName { get; set; }

        [Column("location_status")]
        public string LocationStatus { get; set; }

        [Column("location_createdby")]
        public string LocationCreatedBy { get; set; }

        [Column("location_createddt")]
        public DateTime LocationCreatedDt { get; set; }

        [Column("location_updatedby")]
        public string LocationUpdatedBy { get; set; }

        [Column("location_updateddt")]
        public DateTime? LocationUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("LocationStatus")]
        public Status? Status { get; set; }

        [ForeignKey("LocationCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("LocationUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
