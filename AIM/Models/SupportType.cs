using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class SupportType
    {
        [Key]
        [Column("supporttype_code")]
        public int SupportTypeCode { get; set; }

        [Column("supporttype_name")]
        [StringLength(50)]
        [Required]
        public string SupportTypeName { get; set; }

        [Column("supporttype_status")]
        [StringLength(2)]
        [Required]
        public string SupportTypeStatus { get; set; }

        [Column("supporttype_createdby")]
        [StringLength(15)]
        [Required]
        public string SupportTypeCreatedBy { get; set; }

        [Column("supporttype_createddt")]
        [Required]
        public DateTime SupportTypeCreatedDt { get; set; }

        [Column("supporttype_updatedby")]
        [StringLength(15)]
        public string SupportTypeUpdatedBy { get; set; }

        [Column("supporttype_updateddt")]
        public DateTime? SupportTypeUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("SupportTypeStatus")]
        public Status? Status { get; set; }

        [ForeignKey("SupportTypeCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("SupportTypeUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
