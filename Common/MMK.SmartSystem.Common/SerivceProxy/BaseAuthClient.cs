using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.SerivceProxy
{
    public class BaseAuthClient
    {
        protected void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            request.Headers.Add("Abp.TenantId", "");
            request.Headers.Add(".AspNetCore.Culture", SmartSystemCommonConsts.CurrentCulture);
            request.Headers.Add("Authorization", $"Bearer {SmartSystemCommonConsts.AuthenticateModel.AccessToken}");

        }
    }
}
