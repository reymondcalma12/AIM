using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Level2
    {
        [Key]
        [Column("level2_code")]
        public int Level2Code { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Level 2 Name")]
        [Column("level2_name")]
        public string Level2Name { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Level 2 Status")]
        [Column("level2_status")]
        public string Level2Status { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("level2_createdby")]
        public string Level2CreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("level2_createddt")]
        public DateTime Level2CreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("level2_updatedby")]
        public string? Level2UpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("level2_updateddt")]
        public DateTime? Level2UpdatedDt { get; set; }

        // Nullable Foreign Keys
        [ForeignKey("Level2Status")]
        public Status? Status { get; set; }

        [ForeignKey("Level2CreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("Level2UpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
