using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppSystemAffected
    {
        [Key, Column("app_code", Order = 0)]
        [StringLength(15)]
        [Required]
        public string AppCode { get; set; }

        [Key, Column("system_code", Order = 1)]
        [StringLength(15)]
        [Required]
        public string SystemCode { get; set; }

        [Column("created_by")]
        [StringLength(15)]
        [Required]
        public string? CreatedBy { get; set; }

        [Column("created_dt")]
        [Required]
        public DateTime? CreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        // Rename the navigation property to represent system_code
        [ForeignKey("SystemCode")]
        public Application? System { get; set; }

        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }
    }
}
