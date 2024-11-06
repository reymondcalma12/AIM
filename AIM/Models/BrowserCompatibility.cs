using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class BrowserCompatibility
    {
        [Key]
        [Column("app_code")]
        [StringLength(15)]
        public string AppCode { get; set; }

        [Key]
        [ForeignKey("Browser")]
        [Column("browser_code")]
        public int BrowserCode { get; set; }

        [Required]
        [StringLength(15)]
        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Required]
        [Column("created_dt")]
        public DateTime? CreatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        [ForeignKey("CreatedBy")]
        public User? CreatedUser { get; set; }

        public Browser? Browser { get; set; }
    }
}
