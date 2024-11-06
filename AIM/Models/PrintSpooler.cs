using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class PrintSpooler
    {
        [Key]
        [Column("spooler_code")]
        public int SpoolerCode { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Print Spooler Name")]
        [Column("spooler_name")]
        public string SpoolerName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Print Spooler Status")]
        [Column("spooler_status")]
        public string SpoolerStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("spooler_createdby")]
        public string SpoolerCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("spooler_createddt")]
        public DateTime SpoolerCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("spooler_updatedby")]
        public string? SpoolerUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("spooler_updateddt")]
        public DateTime? SpoolerUpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("SpoolerStatus")]
        public Status? Status { get; set; }

        [ForeignKey("SpoolerCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("SpoolerUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
