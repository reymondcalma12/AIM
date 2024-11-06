namespace AIM.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        [Key]
        [Column("category_id")]

        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("category_name")]

        public string CategoryName { get; set; }

        [Required]
        [MaxLength(2)]
        [Column("category_status")]

        public string CategoryStatus { get; set; }

        [ForeignKey("CategoryStatus")]
        public Status? Status { get; set; }
    }

}
