namespace AIM.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Module
    {
        [Key]
        [Column("module_id")]

        public int ModuleId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("module_title")]

        public string? ModuleTitle { get; set; }
        [Column("module_category")]
        public int ModuleCategory { get; set; }

        [Required]
        [MaxLength(2)]
        [Column("module_status")]

        public string? ModuleStatus { get; set; }

        [Required]
        [MaxLength(15)]
        [Column("module_created")]

        public string? ModuleCreated { get; set; }

        [Required]
        [Column("module_dtcreated")]

        public DateTime ModuleDtCreated { get; set; }

        [MaxLength(15)]
        [Column("module_updated")]

        public string? ModuleUpdated { get; set; }
        [Column("module_dtupdated")]

        public DateTime? ModuleDtUpdated { get; set; }

        // Navigation properties (if needed)
        [ForeignKey("ModuleCategory")]
        public Category? Category { get; set; }

        [ForeignKey("ModuleStatus")]
        public Status? Status { get; set; }

        [ForeignKey("ModuleCreated")]
        public User? CreatedBy { get; set; }

        [ForeignKey("ModuleUpdated")]
        public User? UpdatedBy { get; set; }
    }

}
