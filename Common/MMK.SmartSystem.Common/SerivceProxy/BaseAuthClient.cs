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
           // request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            request.Headers.Add("Cache-Control", "no-cache");
            
        }
    }
}
