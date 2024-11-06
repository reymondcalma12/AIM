namespace AIM.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProfileAccess
    {
        [Key, Column("profile_id", Order = 0)]
        [DisplayName("Profile Id")]

        public int ProfileId { get; set; }

        [Key, Column("module_id", Order = 1)]

        public int ModuleId { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("open_access")]

        public string? OpenAccess { get; set; }

        [Required]
        [MaxLength(15)]
        [Column("user_created")]

        public string? UserCreated { get; set; }

        [Required]
        [Column("user_dtcreated")]

        public DateTime? UserDtCreated { get; set; }

        [MaxLength(15)]
        [Column("user_updated")]

        public string? UserUpdated { get; set; }
        [Column("user_dtupdated")]


        public DateTime? UserDtUpdated { get; set; }

        // Navigation properties (if needed)
        [ForeignKey("ModuleId")]
        public Module? Module { get; set; }

        [ForeignKey("ProfileId")]
        public Profile? Profile { get; set; }

        [ForeignKey("UserCreated")]
        public User? CreatedBy { get; set; }

        [ForeignKey("UserUpdated")]
        public User? UpdatedBy { get; set; }
    }

}
