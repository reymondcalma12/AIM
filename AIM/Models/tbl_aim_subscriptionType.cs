namespace AIM.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tbl_aim_subscriptionType
    {

        [Key]
        [Column("subscriptionType_code")]
        public int subscriptionType_code { get; set; }

        [Column("subscriptionType_name")]
        [StringLength(50)]
        public string? subscriptionType_name { get; set; }


    }
}
