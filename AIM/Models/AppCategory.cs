using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AppCategory
    {
        [Key]
        [Column("cat_code")]
        public int CatCode { get; set; }

        [Column("cat_name")]
        [StringLength(50)]
        [Required]
        public string CatName { get; set; }

        [Column("cat_status")]
        [StringLength(2)]
        [Required]
        public string CatStatus { get; set; }

        [Column("cat_createdby")]
        [StringLength(15)]
        [Required]
        public string CatCreatedBy { get; set; }

        [Column("cat_createddt")]
        [Required]
        public DateTime CatCreatedDt { get; set; }

        [Column("cat_updatedby")]
        [StringLength(15)]
        public string CatUpdatedBy { get; set; }

        [Column("cat_updateddt")]
        public DateTime? CatUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("CatStatus")]
        public Status? Status { get; set; }

        [ForeignKey("CatCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("CatUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
