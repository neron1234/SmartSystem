using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class RequestResult<T> where T : class
    {
        [JsonProperty("result", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public T Result { get; set; }

        [JsonProperty("targetUrl", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string TargetUrl { get; set; }

        [JsonProperty("success", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Success { get; set; }
        [JsonProperty("error", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public RequestError Error { get; set; }
        [JsonProperty("unAuthorizedRequest", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public bool UnAuthorizedRequest { get; set; }
    }


    public class RequestError
    {
        [JsonProperty("code", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public int Code { get; set; }

        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public string Message { get; set; }

        [JsonProperty("details", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public string Details { get; set; }
        [JsonProperty("validationErrors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public string ValidationErrors { get; set; }
    }
}
