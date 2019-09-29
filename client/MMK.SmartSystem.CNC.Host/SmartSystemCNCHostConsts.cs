using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Host
{
    public class SmartSystemCNCHostConsts
    {
        public const string ApiHost = "http://localhost:21021";

        public const string ClientGetCncEvent = "GetCncEvent";
        public const string ClientSuccessEvent = "PushCncDataMessage";
        public const string ClientErrorEvent = "PushErrorMessage";
    }
}
