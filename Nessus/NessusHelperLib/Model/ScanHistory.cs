using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NessusHelperLib.Common;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace NessusHelperLib.Model
{
    public class ScanHistory
    {
        //The TimeStamp when the scan finished running.
        public DateTime TimeEnd { get; set; }

        //The TimeStamp when the scan started running.
        public DateTime TimeStart { get; set; }

        //The UUID for the specific scan run.
        public string ScanUUID { get; set; }

        //The unique identifier for the specific scan run.
        public int Id { get; set; }

        //Indicates whether the scan results are older than 35 days.
        //If this parameter is true, Tenable.io returns limited data for the scan run.
        //For complete scan results that are older than 35 days, use the POST /scans/{scan_id}/export endpoint instead.
        public bool IsArchived { get; set; }

        //The visibility of the scan results in workbenches
        public string Visibilit { get; set; }

        //The status of the scan
        public string Status { get; set; }

        public ScanHistory(JObject obj)
        {
            TimeStart = CommonUtility.EpochToDateTime(Convert.ToInt64(obj["time_start"]));
            TimeEnd = CommonUtility.EpochToDateTime(Convert.ToInt64(obj["time_end"]));
            ScanUUID = obj["scan_uuid"].ToString();
            Id = Convert.ToInt32(obj["id"]);
            IsArchived = Convert.ToBoolean(obj["is_archived"]);
            Visibilit = obj["visibility"].ToString();
            Status = obj["status"].ToString();
        }
    }
}
