using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class AbpUserConfiguration
    {
        [JsonProperty("auth", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public AuthNode Auth { get; set; }

        [JsonProperty("localization", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public LocalizationNode Localization { get; set; }
    }

    public class AuthNode
    {
        [JsonProperty("allPermissions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AllPermissions { get; set; }

        [JsonProperty("grantedPermissions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string GrantedPermissions { get; set; }
    }

    public class LocalizationNode
    {
        [JsonProperty("currentCulture", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Culture CurrentCulture { get; set; }

        [JsonProperty("values", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public LocalizationValue Values { get; set; }
    }

    public class Culture
    {
        [JsonProperty("displayName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class LocalizationValue
    {
        [JsonProperty("SmartSystem", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]

        public string SmartSystem { get; set; }
    }
}
