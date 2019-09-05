using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMK.SmartSystem.Common.SerivceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.SerivceProxy.Tests
{
    [TestClass()]
    public class TokenAuthClientTests
    {
        [TestMethod()]
        public void AuthenticateAsyncTest()
        {
            TokenAuthClient tokenAuthClient = new TokenAuthClient(SmartSystemCommonConsts.ApiHost, new System.Net.Http.HttpClient());
            var obj = tokenAuthClient.AuthenticateAsync(new AuthenticateModel() { UserNameOrEmailAddress = "admin", Password = "123qwe" }).Result;

        }
    }
}