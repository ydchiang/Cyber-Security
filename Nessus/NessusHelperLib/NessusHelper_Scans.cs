using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NessusHelperLib.Model;
using NessusHelperLib.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                JArray jsonArray = (JArray)task.Result.JsonObject["scans"];
                foreach (var json in jsonArray)
                {
                    scans.Add(new Scan((JObject)json));
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
    }
}
