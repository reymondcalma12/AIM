using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Level3
    {
        [Key, Column("level3_code")]
        public int Level3Code { get; set; }

        [Column("level3_name")]
        public string Level3Name { get; set; }

        [Column("level3_status")]
        public string Level3Status { get; set; }

        [Column("level3_createdby")]
        public string Level3CreatedBy { get; set; }

        [Column("level3_createddt")]
        public DateTime Level3CreatedDt { get; set; }

        [Column("level3_updatedby")]
        public string Level3UpdatedBy { get; set; }

        [Column("level3_updateddt")]
        public DateTime? Level3UpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("Level3Status")]
        public Status? Status { get; set; }

        [ForeignKey("Level3CreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("Level3UpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
