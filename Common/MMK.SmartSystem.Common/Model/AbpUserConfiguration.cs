using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class PermissionNode
    {
        public string Name { get; set; }

        public bool Value { get; set; }
    }

    public class LocalNode
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
    public class AbpUserConfiguration
    {
        [JsonProperty("auth", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public AuthNode Auth { get; set; }

        [JsonProperty("localization", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public LocalizationNode Localization { get; set; }
    }


    public class AuthNode
    {
        [JsonProperty("allPermissions")]
        public Dictionary<string, string> AllPermissions { get; set; }

        [JsonProperty("grantedPermissions")]
        public Dictionary<string, string> GrantedPermissions { get; set; }
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
        [JsonProperty("SmartSystem", Required = Required.Default)]
        public Dictionary<string, string> SmartSystem { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("userName", Required = Required.Always)]
        public string UserName { get; set; }

        [JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        public string Name { get; set; }

        [JsonProperty("surname", Required = Newtonsoft.Json.Required.Always)]
        public string Surname { get; set; }

        [JsonProperty("emailAddress", Required = Newtonsoft.Json.Required.Always)]
        public string EmailAddress { get; set; }

        [JsonProperty("isActive", Required = Newtonsoft.Json.Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsActive { get; set; }

        [JsonProperty("fullName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        [JsonProperty("lastLoginTime", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTimeOffset? LastLoginTime { get; set; }

        [JsonProperty("creationTime", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTimeOffset? CreationTime { get; set; }

        [JsonProperty("roleNames", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> RoleNames { get; set; }

        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }
    }

    public class LoginMessage
    {
        public bool Success { get; set; }

        public string Msg { get; set; }

    }
}
