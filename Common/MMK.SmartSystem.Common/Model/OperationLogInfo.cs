using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MMK.SmartSystem.Common.Model
{
    public class OperationLogInfo
    {
        [JsonProperty("userId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("serviceName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ServiceName { get; set; }

        [Newtonsoft.Json.JsonProperty("methodName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MethodName { get; set; }

        [Newtonsoft.Json.JsonProperty("parameters", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Parameters { get; set; }

        [Newtonsoft.Json.JsonProperty("moduleName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ModuleName { get; set; }

        [Newtonsoft.Json.JsonProperty("pageName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PageName { get; set; }

        [Newtonsoft.Json.JsonProperty("returnValue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ReturnValue { get; set; }

        [Newtonsoft.Json.JsonProperty("browserInfo", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BrowserInfo { get; set; }

        [Newtonsoft.Json.JsonProperty("clientIpAddress", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ClientIpAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("clientName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ClientName { get; set; }

        [Newtonsoft.Json.JsonProperty("customData", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CustomData { get; set; }

        [Newtonsoft.Json.JsonProperty("executionTime", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? ExecutionTime { get; set; }

        [Newtonsoft.Json.JsonProperty("executionDuration", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? ExecutionDuration { get; set; }

        [Newtonsoft.Json.JsonProperty("exception", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Exception { get; set; }

        [Newtonsoft.Json.JsonProperty("creationTime", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? CreationTime { get; set; }

        [JsonProperty("id", Required = Newtonsoft.Json.Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
    }
}
