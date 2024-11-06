namespace AIM.Models
{
    using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

    public class Status
    {
        [Key]
        [MaxLength(2)]
		[Column("status_code")]

		public string StatusCode { get; set; }

        [Required]
        [MaxLength(20)]
		[Column("status_name")]

		public string StatusName { get; set; }
    }

}
