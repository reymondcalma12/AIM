using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Level3Owner
    {
        [Key, Column("rportosla_code", Order = 0)]
        public string RPortOslaCode { get; set; }

        [Key, Column("level3_code", Order = 1)]
        public int Level3Code { get; set; }

        [Column("level3_createdby")]
        public string Level3OwnerCreatedBy { get; set; }

        [Column("level3_createddt")]
        public DateTime Level3OwnerCreatedDt { get; set; }

        // Navigation properties
        [ForeignKey("RPortOslaCode")]
        public RPortOsla? RPortOsla { get; set; }

        [ForeignKey("Level3Code")]
        public Level3? Level3 { get; set; }

        [ForeignKey("Level3OwnerCreatedBy")]
        public User? CreatedBy { get; set; }
    }
}
