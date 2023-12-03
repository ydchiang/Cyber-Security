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
        public List<Folder> GetFoldersList()
        {
            var task = GetFoldersListAsync();
            task.Wait();

            if (task.Result.Success)
            {
                List<Folder> folders = new List<Folder>();

                JArray jsonArray = (JArray)task.Result.JsonObject["folders"];
                foreach (var json in jsonArray)
                {
                    folders.Add(new Folder((JObject)json));
                }

                return folders;
            }
            else
            {
                throw new Exception(task.Result.ErrorMessage);
            }
        }

        public async Task<ApiExecuteResult> GetFoldersListAsync()
        {
            string url = $"{_serverUrl}/folders";
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
