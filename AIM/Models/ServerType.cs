using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class ServerType
    {
        [Key]
        [Column("type_code")]
        public int TypeCode { get; set; }

        [Column("type_name")]
        [StringLength(50)]
        [Required]
        public string TypeName { get; set; }

        [Column("type_status")]
        [StringLength(2)]
        [Required]
        public string TypeStatus { get; set; }

        [Column("type_createdby")]
        [StringLength(15)]
        [Required]
        public string TypeCreatedBy { get; set; }

        [Column("type_createddt")]
        [Required]
        public DateTime TypeCreatedDt { get; set; }

        [Column("type_updatedby")]
        [StringLength(15)]
        public string? TypeUpdatedBy { get; set; }

        [Column("type_updateddt")]
        public DateTime? TypeUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("TypeStatus")]
        public Status? Status { get; set; }

        [ForeignKey("TypeCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("TypeUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
