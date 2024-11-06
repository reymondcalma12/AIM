using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Server
    {
        [Key]
        [Column("server_code")]
        public int ServerCode { get; set; }

        [Column("server_name")]
        [StringLength(50)]
        [Required]
        public string ServerName { get; set; }

        [Column("server_ipadd")]
        [StringLength(15)]
        [Required]
        public string ServerIPAddress { get; set; }

        [Column("server_type")]
        [Required]
        public int ServerType { get; set; }

        [Column("server_hostname")]
        [StringLength(50)]
        public string ServerHostName { get; set; }

        [Column("server_hostipadd")]
        [StringLength(15)]
        public string ServerHostIPAddress { get; set; }

        [Column("server_status")]
        [StringLength(2)]
        [Required]
        public string ServerStatus { get; set; }

        [Column("server_createdby")]
        [StringLength(15)]
        [Required]
        public string ServerCreatedBy { get; set; }

        [Column("server_createddt")]
        [Required]
        public DateTime ServerCreatedDt { get; set; }

        [Column("server_updatedby")]
        [StringLength(15)]
        public string? ServerUpdatedBy { get; set; }

        [Column("server_updateddt")]
        public DateTime? ServerUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("ServerStatus")]
        public Status? Status { get; set; }

        [ForeignKey("ServerCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("ServerUpdatedBy")]
        public User? UpdatedBy { get; set; }

        // Navigation property for ServerType
        [ForeignKey("ServerType")]
        public ServerType? Type { get; set; }
    }
}
