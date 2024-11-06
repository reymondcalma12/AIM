namespace AIM.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Key]
        [MaxLength(15)]
		[Column("user_code")]

		public string? UserCode { get; set; }

        [MaxLength(60)]
		[Column("user_pass")]

		public string? UserPass { get; set; }

        [MaxLength(50)]
		[Column("user_adlogin")]

		public string? UserADLogin { get; set; }

        [MaxLength(150)]
		[Column("user_fullname")]

		public string? UserFullName { get; set; }

		[Column("user_profile")]

		public int? UserProfile { get; set; }

        [MaxLength(2)]
		[Column("user_status")]

		public string? UserStatus { get; set; }

        [MaxLength(15)]
		[Column("user_created")]

		public string? UserCreated { get; set; }

		[Column("user_dtcreated")]

		public DateTime? UserDtCreated { get; set; }

        [MaxLength(15)]
		[Column("user_updated")]

		public string? UserUpdated { get; set; }
		[Column("user_dtupdated")]


		public DateTime? UserDtUpdated { get; set; }

        // Navigation properties (if needed)
        [ForeignKey("UserStatus")]
        public virtual Status? Status { get; set; }

        [ForeignKey("UserProfile")]
        public virtual Profile? Profile { get; set; }
    }

}
