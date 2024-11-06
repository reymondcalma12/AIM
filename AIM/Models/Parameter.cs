namespace AIM.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Parameter
    {
        [Key]
        [MaxLength(10)]
        [Column("parm_code")]

        public string ParmCode { get; set; }
        [Column("parm_value")]

        public int ParmValue { get; set; }

        [Required]
        [Column("parm_string")]
        [MaxLength(150)]
        public string ParmString { get; set; }
    }

}
