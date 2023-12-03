using System;

namespace NessusHelperLib.Common
{
    public class CommonUtility
    {
        public static DateTime EpochToDateTime(long epochValue)
        {
            DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeSeconds(epochValue);
           return dateTimeOffSet.DateTime;
        }
   }
}
