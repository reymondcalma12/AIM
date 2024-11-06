using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class AuthMethod
    {
        [Key]
        [Column("auth_code")]
        public int AuthCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Authentication Name")]
        [Column("auth_name")]
        public string AuthName { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Authentication Status")]
        [Column("auth_status")]
        public string AuthStatus { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Created By")]
        [Column("auth_createdby")]
        public string AuthCreatedBy { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [Column("auth_createddt")]
        public DateTime AuthCreatedDt { get; set; }

        [StringLength(15)]
        [Display(Name = "Updated By")]
        [Column("auth_updatedby")]
        public string AuthUpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [Column("auth_updateddt")]
        public DateTime? AuthUpdatedDt { get; set; }

        // Navigation properties

        [ForeignKey("AuthStatus")]
        public Status? Status { get; set; }

        [ForeignKey("AuthCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("AuthUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
