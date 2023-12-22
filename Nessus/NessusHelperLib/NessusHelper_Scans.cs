using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NessusHelperLib.Model;
using NessusHelperLib.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace NessusHelperLib
{
    public partial class NessusHelper
    {
        public List<Scan> GetScansList()
        {
            var task = GetScansListAsync();
            task.Wait();

            if (task.Result.Success)
            {
                List<Scan> scans = new List<Scan>();

                var jObj = task.Result.JsonObject;
                if (jObj["scans"] != null && jObj["scans"].HasValues)   //檢查是否設定 scans
                {
                    JArray jsonArray = (JArray)task.Result.JsonObject["scans"];
                    foreach (var json in jsonArray)
                    {
                        scans.Add(new Scan((JObject)json));
                    }
                }

                return scans;
            }
            else
            {
                throw new Exception(task.Result.ErrorMessage);
            }
        }

        public List<ScanHistory> GetScanHistory(string scanId, int limit = 50, int offset = 0, string sort = "desc", bool exclude_rollover = false)
        {
            var task = GetScansHistoryAsync(scanId, limit, offset, sort, exclude_rollover);
            task.Wait();

            if (task.Result.Success)
            {
                List<ScanHistory> scans = new List<ScanHistory>();

                JArray jsonArray = (JArray)task.Result.JsonObject["history"];
                foreach (var json in jsonArray)
                {
                    scans.Add(new ScanHistory((JObject)json));
                }

                return scans;
            }
            else
            {
                throw new Exception(task.Result.ErrorMessage);
            }
        }

        public async Task<ApiExecuteResult> GetScansListAsync()
        {
            string url = $"{_serverUrl}/scans";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                return new ApiExecuteResult
                {
                    Success = true,
                    JsonObject = result
                };

            }
            else
            {
                return new ApiExecuteResult
                {
                    ErrorMessage = response.StatusCode.ToString()
                };
            };
        }

        //limit: The number of records to retrieve. If this parameter is omitted, Tenable.io uses the default value of 50.
        //offset: The starting record to retrieve. If this parameter is omitted, Tenable.io uses the default value of 0.
        //sort: The field you want to use to sort the results by along with the sort order. The field is specified first,
        //      followed by a colon, and the order is specified second (asc or desc).
        //      For example, start_date:desc would sort results by the start_date field in descending order.
        //exclude_rollover: Indicates whether or not to exclude rollover scans from the scan history. If no value is provided for this parameter, Tenable.io uses the default value false.
        public async Task<ApiExecuteResult> GetScansHistoryAsync(string scanId, int limit = 50, int offset = 0, string sort = "desc", bool exclude_rollover = false)
        {
            string url = $"{_serverUrl}/scans/{scanId}/history";

            JObject param = new JObject(
                   new JProperty("limit", limit),
                   new JProperty("offset", offset),
                   new JProperty("sort", "start_date:{sort}"),
                   new JProperty("exclude_rollover", exclude_rollover)
                );
            var postContent = new StringContent(param.ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, postContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                return new ApiExecuteResult
                {
                    Success = true,
                    JsonObject = result
                };

            }
            else
            {
                return new ApiExecuteResult
                {
                    ErrorMessage = response.StatusCode.ToString()
                };
            };
        }
    }
}
