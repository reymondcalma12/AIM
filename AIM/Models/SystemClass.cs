using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class SystemClass
    {
        [Key]
        [Column("class_code")]
        public int ClassCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "System Class Name")]
        [Column("class_name")]
        public string ClassName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "System Class Status")]
        [Column("class_status")]
        public string ClassStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("class_createdby")]
        public string ClassCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("class_createddt")]
        public DateTime ClassCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("class_updatedby")]
        public string? ClassUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("class_updateddt")]
        public DateTime? ClassUpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("ClassStatus")]
        public Status? Status { get; set; }

        [ForeignKey("ClassCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("ClassUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
