using Abp.AutoMapper;
using MMK.SmartSystem.Authentication.External;

namespace MMK.SmartSystem.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
