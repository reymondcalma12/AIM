using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIM.Models
{
    public class Application
    {
        [Key, Column("app_code")]
        public string AppCode { get; set; }

        [Column("app_name")]
        public string AppName { get; set; }

        [Column("app_category")]
        public int AppCategory { get; set; }

        [Column("app_critlvl")]
        public int AppCritLevel { get; set; }

        [Column("app_slalvl")]
        public int AppSLALevel { get; set; }

        [Column("app_version")]
        public string? AppVersion { get; set; }

        [Column("app_location")]
        public int AppLocation { get; set; }

        [Column("app_description")]
        public string AppDescription { get; set; }

        [Column("app_prgapiendpoint")]
        public string? AppProgramApiEndpoint { get; set; }

        [Column("app_apiurlendpoint")]
        public string? AppApiUrlEndpoint { get; set; }

        [Column("app_portused")]
        public int? AppPortUsed { get; set; }

        [Column("app_sdsupport")]
        public char AppSupportsSD { get; set; }

        [Column("app_servername")]
        public int? AppServerName { get; set; }

        [Column("app_os")]
        public int? AppOS { get; set; }

        [Column("app_authmethod")]
        public int? AppAuthMethod { get; set; }

        [Column("app_printspool")]
        public int? AppPrintSpool { get; set; }

        [Column("app_groupname")]
        public int? AppGroupName { get; set; }

        [Column("app_systemclass")]
        public int? AppSystemClass { get; set; }

        [Column("app_status")]
        public string AppStatus { get; set; }

        [Column("app_createdby")]
        public string AppCreatedBy { get; set; }

        [Column("app_createddt")]
        public DateTime AppCreatedDt { get; set; }

        [Column("app_updatedby")]
        public string? AppUpdatedBy { get; set; }

        [Column("app_updateddt")]
        public DateTime? AppUpdatedDt { get; set; }

        [Column("app_expirydate")]
        public DateTime? AppExpiryDate { get; set; }

        [Column("subscriptionType_code")]
        public int? subscriptionType_code { get; set; }






        // Navigation properties

        [ForeignKey("subscriptionType_code")]
        public tbl_aim_subscriptionType? tbl_aim_subscriptionType { get; set; }

        [ForeignKey("AppCategory")]
        public AppCategory? Category { get; set; }

        [ForeignKey("AppCritLevel")]
        public CriticalLevel? CriticalLevel { get; set; }

        [ForeignKey("AppSLALevel")]
        public SLALevel? SLALevel { get; set; }

        [ForeignKey("AppLocation")]
        public AppLocation? Location { get; set; }

        [ForeignKey("AppServerName")]
        public Server? Server { get; set; }

        [ForeignKey("AppOS")]
        public OperatingSystem? OS { get; set; }

        [ForeignKey("AppAuthMethod")]
        public AuthMethod? AuthMethod { get; set; }

        [ForeignKey("AppPrintSpool")]
        public PrintSpooler? PrintSpooler { get; set; }

        [ForeignKey("AppGroupName")]
        public Group? Group { get; set; }

        [ForeignKey("AppSystemClass")]
        public SystemClass? SystemClass { get; set; }

        [ForeignKey("AppStatus")]
        public Status? Status { get; set; } // Added Status property

        [ForeignKey("AppCreatedBy")]
        public User? CreatedBy { get; set; }

        [ForeignKey("AppUpdatedBy")]
        public User? UpdatedBy { get; set; }
    }
}
