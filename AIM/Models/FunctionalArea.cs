using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class FunctionalArea
    {
        [Key]
        [Column("functional_code")]
        public int FunctionalCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Functional Area Name")]
        [Column("functional_name")]
        public string FunctionalName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Functional Area Status")]
        [Column("functional_status")]
        public string FunctionalStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("functional_createdby")]
        public string FunctionalCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("functional_createddt")]
        public DateTime FunctionalCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("functional_updatedby")]
        public string? FunctionalUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("functional_updateddt")]
        public DateTime? FunctionalUpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("FunctionalStatus")]
        public Status? Status { get; set; }

        [ForeignKey("FunctionalCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("FunctionalUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
