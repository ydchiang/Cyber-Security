using Newtonsoft.Json.Linq;

namespace NessusHelperLib.Common
{
    public class ApiExecuteResult
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public JObject JsonObject { get; set; } = new JObject();
    }
}
