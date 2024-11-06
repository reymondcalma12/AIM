using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class CriticalLevel
    {
        [Key]
        [Column("critlevel_code")]
        public int CriticalLevelCode { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Critical Level Name")]
        [Column("critlevel_name")]
        public string CriticalLevelName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Critical Level Status")]
        [Column("critlevel_status")]
        public string CriticalLevelStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("critlevel_createdby")]
        public string CriticalLevelCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("critlevel_createddt")]
        public DateTime CriticalLevelCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("critlevel_updatedby")]
        public string? CriticalLevelUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("critlevel_updateddt")]
        public DateTime? CriticalLevelUpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("CriticalLevelStatus")]
        public Status? Status { get; set; }

        [ForeignKey("CriticalLevelCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("CriticalLevelUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
