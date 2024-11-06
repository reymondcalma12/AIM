using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Group
    {
        [Key]
        [Column("group_code")]
        public int GroupCode { get; set; }

        [Column("group_name")]
        [StringLength(50)]
        [Required]
        public string GroupName { get; set; }

        [Column("group_status")]
        [StringLength(2)]
        [Required]
        public string GroupStatus { get; set; }

        [Column("group_createdby")]
        [StringLength(15)]
        [Required]
        public string GroupCreatedBy { get; set; }

        [Column("group_createddt")]
        [Required]
        public DateTime GroupCreatedDt { get; set; }

        [Column("group_updatedby")]
        [StringLength(15)]
        public string GroupUpdatedBy { get; set; }

        [Column("group_updateddt")]
        public DateTime? GroupUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("GroupStatus")]
        public Status? Status { get; set; }

        [ForeignKey("GroupCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("GroupUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
