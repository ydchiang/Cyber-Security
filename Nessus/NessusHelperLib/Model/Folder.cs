using System;
using Newtonsoft.Json.Linq;

namespace NessusHelperLib.Model
{

    public class Folder
    {
        //The unique ID of the folder.
        public int Id { get; set; }

        //The name of the folder. This value corresponds to the folder type as follows:
        //main—My Scans
        //trash—Trash
        //-custom—user-defined string.
        public string Name { get; set; }

        //The type of the folder:
        //main—Tenable-provided folder.Contains all scans that you create but do not assign to a custom folder, as well as any scans shared with you by other users. If you do not specify a scan folder when creating a scan, Tenable.io stores scans in this folder by default. This folder corresponds to the My Scans folder in the Tenable.io user interface.
        //trash—Tenable-provided folder.This folder corresponds to the Trash folder in the Tenable.io user interface. It contains all scans that the current user has moved to the trash folder.After you move a scan to the trash folder, the scan remains in the trash folder until a user with at least Can Edit[64] scan permissions permanently deletes the scan.
        //custom—User-created folder. Contains scans as assigned by the current user.You can create custom folders to meet your organizational needs.
        public string FolderType { get; set; }
        
        //Indicates whether or not the folder is a custom folder:
        //1—User-created folder.You can rename or delete this folder.
        //0—System-created folder. You cannot rename or delete this folder.
        public int Custom { get; set; } = 0;

        //The number of scans in the folder that the current user has not yet viewed in the Tenable.io user interface.
        public int UnreadCount { get; set; } = 0;

        //Indicates whether or not the folder is the default:
        //1—The folder is the default.
        //0—The folder is not the default.
        //The main folder is the default folder.You cannot change the default folder.
        public int DefaultTag { get; set; } = 0;

        public Folder(JObject obj)
        {
            Id = Convert.ToInt32(obj["id"]);
            Name = obj["name"].ToString();
            FolderType=obj["type"].ToString();
            Custom = Convert.ToInt32(obj["custom"]);
            UnreadCount = Convert.ToInt32(obj["unread_count"].HasValues? obj["unread_count"]:"0");
            DefaultTag = Convert.ToInt32(obj["default_tag"]);
        }
    }
}
