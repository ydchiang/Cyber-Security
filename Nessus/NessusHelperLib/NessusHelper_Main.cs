using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NessusHelperLib.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NessusHelperLib
{
    public partial class NessusHelper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverHost;
        private readonly int _serverPort;
        private readonly string _serverUrl;
        private string _apiToken;

        public NessusHelper(string serverHost = "127.0.0.1", int serverPort = 8834)
        {
            _serverHost = serverHost;
            _serverPort = serverPort;
            _serverUrl = $"https://{_serverHost}:{_serverPort}";

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            };
            _httpClient = new HttpClient(handler);

        }

        public void Dispose()
        {
            try
            {
                Logout();
                _httpClient.Dispose();
            }
            catch
            {

            }
        }

        public ApiExecuteResult Login(string username, string password)
        {
            var asyncLoginTask = LoginAsync(username, password);
            asyncLoginTask.Wait();

            return asyncLoginTask.Result;
        }

        public async Task<ApiExecuteResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new ApiExecuteResult { Success = false, ErrorMessage = "Require username and password." };
            }

            try
            {
                string url = $"{_serverUrl}/session";
                JObject param = new JObject(
                   new JProperty("username", username),
                   new JProperty("password", password)
                );
                var postContent = new StringContent(param.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, postContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    _apiToken = result["token"].ToString();
                    _httpClient.DefaultRequestHeaders.Add("X-Cookie", $"token={_apiToken}");
                    return new ApiExecuteResult
                    {
                        Success = true,
                        ErrorMessage = ""
                    };
                }
                else
                {
                    return new ApiExecuteResult
                    {
                        Success = false,
                        ErrorMessage = response.StatusCode.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiExecuteResult
                {
                    Success = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        public ApiExecuteResult Logout()
        {
            var asyncLogoutTask = LogoutAsync();
            asyncLogoutTask.Wait();

            return asyncLogoutTask.Result;
        }

        public async Task<ApiExecuteResult> LogoutAsync()
        {
            string url = $"{_serverUrl}/session";

            try
            {
                var response = await _httpClient.DeleteAsync(url);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    _apiToken = "";
                    _httpClient.DefaultRequestHeaders.Remove("X-Cookie");
                    return new ApiExecuteResult
                    {
                        Success = true,
                        ErrorMessage = ""
                    };
                }
                else
                {
                    return new ApiExecuteResult
                    {
                        Success = false,
                        ErrorMessage = response.StatusCode.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiExecuteResult
                {
                    Success = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }


    }
}
