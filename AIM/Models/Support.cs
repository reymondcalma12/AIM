using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Support
    {
        [Key]
        [Column("support_code")]
        public int SupportCode { get; set; }

        [Column("support_name")]
        [StringLength(50)]
        [Required]
        public string SupportName { get; set; }

        [Column("support_status")]
        [StringLength(2)]
        [Required]
        public string SupportStatus { get; set; }

        [Column("support_createdby")]
        [StringLength(15)]
        [Required]
        public string SupportCreatedBy { get; set; }

        [Column("support_createddt")]
        [Required]
        public DateTime SupportCreatedDt { get; set; }

        [Column("support_updatedby")]
        [StringLength(15)]
        public string SupportUpdatedBy { get; set; }

        [Column("support_updateddt")]
        public DateTime? SupportUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("SupportStatus")]
        public Status? Status { get; set; }

        [ForeignKey("SupportCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("SupportUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
