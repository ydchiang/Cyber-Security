using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NessusHelper
{
    public class NessusSession : IDisposable
    {
        private readonly string _serverHost;
        private readonly int _serverPort;
        private readonly string _userName;
        private readonly string _password;
        private HttpClient _httpClient;
        private string _apiToken;

        public string APIToken
        {
            get
            {
                return _apiToken;
            }
        }

        public string Url
        {
            get
            {
                return $"https://{_serverHost}:{_serverPort}";
            }
        }

        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        public NessusSession(string serverHost, int serverPort, string userName, string password)
        {
            _serverHost = serverHost;
            _serverPort = serverPort;
            _userName = userName;
            _password = password;

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        public async void Dispose()
        {
            try
            {
                await LogoutAsync();
                _httpClient.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dispose Exception: {ex}");
            }
        }

        public (bool, string) Login()
        {
            try
            {
                Task<JObject> json = LoginAsync();
                json.Wait();

                JObject result = json.Result;
                if (result["success"].Value<bool>())
                {
                    _apiToken = result["message"].ToString();
                    _httpClient.DefaultRequestHeaders.Add("X-Cookie", $"token={_apiToken}");
                    return (true, "");
                }
                else
                {
                    return (false, result["message"].ToString());
                }

            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        public (bool, string) Logout()
        {
            try
            {
                Task<JObject> json = LogoutAsync();
                json.Wait();

                JObject result = json.Result;
                if (result["success"].Value<bool>())
                {
                    _apiToken = "";
                    _httpClient.DefaultRequestHeaders.Remove("X-Cookie");
                    return (true, "");
                }
                else
                {
                    return (false, result["message"].ToString());
                }
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        public async Task<JObject> LoginAsync()
        {
            if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_password))
            {
                throw new Exception("Require Username and Password.");
            }

            JObject param = new JObject(
                new JProperty("username", _userName),
                new JProperty("password", _password)
            );

            try
            {
                string url = $"{Url}/session";
                var postContent = new StringContent(param.ToString(), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, postContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    var token = result["token"].ToString();

                    return new JObject(
                        new JProperty("success", true),
                        new JProperty("message", token));
                }
                else
                {
                    return new JObject(
                        new JProperty("success", false),
                        new JProperty("message", response.StatusCode.ToString()));
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<JObject> LogoutAsync()
        {
            if (string.IsNullOrEmpty(_apiToken))
            {
                return new JObject(
                        new JProperty("success", true),
                        new JProperty("message", ""));
            }

            string url = $"{Url}/session";

            try
            {
                var response = await _httpClient.DeleteAsync(url);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var result = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                    return new JObject(
                        new JProperty("success", true),
                        new JProperty("message", ""));
                }
                else
                {
                    return new JObject(
                        new JProperty("success", false),
                        new JProperty("message", response.StatusCode.ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
