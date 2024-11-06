using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Browser
    {
        [Key]
        [Column("browser_code")]
        public int BrowserCode { get; set; }

        [Required]
        [StringLength(100)]
        [Column("browser_name")]
        public string BrowserName { get; set; }

        [Required]
        [StringLength(2)]
        [Column("browser_status")]
        public string BrowserStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Column("browser_createdby")]
        public string BrowserCreatedBy { get; set; }

        [Required]
        [Column("browser_createddt")]
        public DateTime BrowserCreatedDt { get; set; }

        [StringLength(15)]
        [Column("browser_updatedby")]
        public string BrowserUpdatedBy { get; set; }

        [Column("browser_updateddt")]
        public DateTime? BrowserUpdatedDt { get; set; }

        [ForeignKey("BrowserStatus")]
        public Status? Status { get; set; }

        [ForeignKey("BrowserCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("BrowserUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
