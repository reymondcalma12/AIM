using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class RPortOsla
    {
        [Key, Column("rportosla_code")]
        public string RPortOslaCode { get; set; }

        [Column("app_code")]
        public string? AppCode { get; set; }

        [Column("app_location")]
        public int? AppLocation { get; set; }

        [Column("app_currentrpo")]
        public string? AppCurrentRPO { get; set; }

        [Column("app_proposedrpo")]
        public string? AppProposedRPO { get; set; }

        [Column("app_rto")]
        public string? AppRTO { get; set; }

        [Column("app_slalevel")]
        public int AppSLALevel { get; set; }

        [Column("app_status")]
        public string AppStatus { get; set; }

        [Column("app_createdby")]
        public string AppCreatedBy { get; set; }

        [Column("app_createddt")]
        public DateTime? AppCreatedDt { get; set; }

        [Column("app_updatedby")]
        public string? AppUpdatedBy { get; set; }

        [Column("app_updateddt")]
        public DateTime? AppUpdatedDt { get; set; }

        // Navigation properties
        [ForeignKey("AppCode")]
        public Application? Application { get; set; }

        [ForeignKey("AppLocation")]
        public AppLocation? Location { get; set; }

        [ForeignKey("AppSLALevel")]
        public SLALevel? SLALevel { get; set; }

        [ForeignKey("AppStatus")]
        public Status? Status { get; set; }

        [ForeignKey("AppCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("AppUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
