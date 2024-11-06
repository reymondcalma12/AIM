using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppContact
    {
        [Key, Column("app_code", Order = 0)]
        [StringLength(15)]
        [Required]
        public string AppCode { get; set; }

        [Key, Column("contact_no", Order = 1)]
        [StringLength(15)]
        [Required]
        public string ContactNo { get; set; }

        [Key, Column("supporttype_code", Order = 2)]
        [Required]
        public int SupportTypeCode { get; set; }

        [Column("created_by")]
        [StringLength(15)]
        [Required]
        public string CreatedBy { get; set; }

        [Column("created_dt")]
        [Required]
        public DateTime CreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        [ForeignKey("SupportTypeCode")]
        public SupportType? SupportType { get; set; }

        [ForeignKey("CreatedBy")]
        public User? User { get; set; }
    }
}
