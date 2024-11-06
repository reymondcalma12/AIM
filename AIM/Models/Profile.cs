namespace AIM.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

	public class Profile
	{
		[Key]
		[Column("profile_id")]
		public int ProfileId { get; set; }

		[Required]
		[MaxLength(20)]
		[Column("profile_name")]
		public string ProfileName { get; set; }

		[Required]
		[MaxLength(100)]
		[Column("profile_description")]
		public string ProfileDescription { get; set; }

		[Required]
		[MaxLength(2)]
		[Column("profile_status")]
		public string? ProfileStatus { get; set; }

		[MaxLength(15)]
		[Column("profile_created")]
		public string? ProfileCreated { get; set; }

		[Required] 
		[Column("profile_dtcreated")]
		public DateTime ProfileDtCreated { get; set; }

		[MaxLength(15)]
		[Column("profile_updated")]
		public string? ProfileUpdated { get; set; }

		[Column("profile_dtupdated")]
		public DateTime? ProfileDtUpdated { get; set; }

		// Navigation properties (if needed)
		[ForeignKey("ProfileStatus")]
		public Status? Status { get; set; }

		[ForeignKey("ProfileCreated")]
		public User? CreatedBy { get; set; }

		[ForeignKey("ProfileUpdated")]
		public User? UpdatedBy { get; set; }
	}

}
