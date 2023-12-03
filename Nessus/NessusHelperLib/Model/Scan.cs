using System;
using NessusHelperLib.Common;
using Newtonsoft.Json.Linq;

namespace NessusHelperLib.Model
{
    public class Scan
    {
        //If true, the scan has a schedule and can be launched.
        public bool Control { get; set; }
        
        //For newly-created scans, the date on which the scan configuration was originally created.
        //For scans that have been launched at least once, this attribute does not represent the date on which the scan configuration was originally created. Instead, it represents the date on which the scan was first launched, in Unix time format.
        public DateTime CreationDate { get; set; }

        //Indicates whether the scan schedule is active (true) or inactive (false).
        public bool Enabled { get; set; }

        //The unique ID of the scan.
        public int Id { get; set; }

        //For newly-created scans, the date on which the scan configuration was created.
        //For scans that have been launched at least once, this attribute does not represent the date on which the scan configuration was last modified. Instead, it represents the date on which the scan was last launched, in Unix time format.Tenable.io updates this attribute each time the scan launches.
        public DateTime LastModificationDate { get; set; }

        //A value indicating whether the scan results were created before a change in storage method.
        //If true, Tenable.io stores the results in the old storage method. If false, Tenable.io stores the results in the new storage method.
        public bool Legacy { get; set; }

        //The name of the scan
        public string Name { get; set; }

        //The owner of the scan.
        public string Owner { get; set; }

        //The unique ID of the user-defined template (policy) on which the scan configuration is based.
        public int PolicyId { get; set; }

        //A value indicating whether the user account associated with the request message has viewed the scan in the Tenable.io user interface. If 1, the user account has viewed the scan results.
        public bool Read { get; set; }

        //The UUID for a specific instance in the scan schedule.
        public string ScheduleUUID { get; set; }

        //If true, the scan is shared with users other than the scan owner. The level of sharing is specified in the acls attribute of the scan details.
        public bool Shared { get; set; }

        //The status of the scan. For a list of possible values, see Scan Status[https://developer.tenable.com/docs/scan-status-tio].
        public string Status { get; set; }

        //The UUID of the template.
        public string TemplateUUID { get; set; }
        
        public bool HasTriggers { get; set; }

        //The type of scan.
        public string ScanType { get; set; }

        //The requesting user's permissions for the scan.
        public int Permissions { get; set; }

        //The sharing permissions for the scan.
        public int UserPermissions { get; set; }

        //The UUID of the scan.
        public string UUID { get; set; }

        //The UUID of the Tenable-provided template used to create either the scan or the user-defined template (policy) on which the scan configuration is based.
        public string WizardUUID { get; set; }

        //The progress of the scan ranging from 0 to 100.
        public int Progress { get; set; }

        //The total number of targets in the scan.
        public int TotalTargets { get; set; }

        //The timezone of the scheduled start time for the scan.
        public string Timezone { get; set; }

        public Scan(JObject obj)
        {
            Control = Convert.ToBoolean(obj["control"]);
            CreationDate = CommonUtility.EpochToDateTime(Convert.ToInt64(obj["creation_date"]));
            Enabled = Convert.ToBoolean(obj["enabled"]);
            Id = Convert.ToInt32(obj["id"]);
            LastModificationDate= CommonUtility.EpochToDateTime(Convert.ToInt64(obj["last_modification_date"]));
            Enabled = Convert.ToBoolean(obj["legacy"]);
            Name = obj["name"].ToString();
            Owner = obj["owner"].ToString();
            PolicyId= Convert.ToInt32(obj["policy_id"]);
            Read = Convert.ToBoolean(obj["read"]);
            ScheduleUUID = obj["schedule_uuid"]?.ToString();
            Shared = Convert.ToBoolean(obj["shared"]);
            Status=obj["status"].ToString();
            TemplateUUID= obj["template_uuid"]?.ToString();
            HasTriggers= Convert.ToBoolean(obj["has_triggers"]);
            ScanType=obj["type"].ToString();
            Permissions= Convert.ToInt32(obj["permissions"]);
            UserPermissions = Convert.ToInt32(obj["user_permissions"]);
            UUID = obj["uuid"].ToString();
            WizardUUID = obj["wizard_uuid"]?.ToString();
            Progress= Convert.ToInt32(obj["progress"]);
            TotalTargets = Convert.ToInt32(obj["total_targets"]);
            Timezone = obj["timezone"]?.ToString();
        }

        // https://developer.tenable.com/docs/permissions
        private string getPermissionDesc(int permissionValue)
        {
            switch (permissionValue)
            {
                case 0:
                    return "No Access";
                case 16:
                    return "Can View";
                case 32:
                    return "Can Execute";
                case 64:
                    return "Can Edit";
                case 128:
                    return "Owner";
                default:
                    return "Unknown";
            }
        }

        public string PermissionsDesc
        {
            get
            {
                return getPermissionDesc(Permissions);
            }
        }

        public string UserPermissionsDesc
        {
            get
            {
                return getPermissionDesc(UserPermissions);
            }
        }
    }
}
