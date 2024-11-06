using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class OperatingSystem
    {
        [Key]
        [Column("os_code")]
        public int OsCode { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Operating System Name")]
        [Column("os_name")]
        public string OsName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Operating System Status")]
        [Column("os_status")]
        public string OsStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("os_createdby")]
        public string OsCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("os_createddt")]
        public DateTime OsCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("os_updatedby")]
        public string? OsUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("os_updateddt")]
        public DateTime? OsUpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("OsStatus")]
        public Status? Status { get; set; }

        [ForeignKey("OsCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("OsUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
