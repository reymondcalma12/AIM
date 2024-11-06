namespace AIM.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Store
    {
        [Key]
        [MaxLength(5)]
        [Column("store_code")]

        public string StoreCode { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("store_name")]

        public string StoreName { get; set; }

        [Required]
        [MaxLength(2)]
        [Column("store_status")]

        public string StoreStatus { get; set; }

        // Navigation property (if needed)
        [ForeignKey("StoreStatus")]
        public Status? Status { get; set; }
    }

}
