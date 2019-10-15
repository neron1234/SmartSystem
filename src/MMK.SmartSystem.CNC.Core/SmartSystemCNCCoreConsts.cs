using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
namespace MMK.SmartSystem.CNC.Core
{
    public class SmartSystemCNCCoreConsts
    {
        public const string CncIP = "192.168.21.134";

        public const int CncPort = 8193;

        public const int CncTimeout = 10;
        public const double CncIncrement = 1000;

        public static ConcurrentDictionary<string, List<CncEventData>> PageCncEventDict = new ConcurrentDictionary<string, List<CncEventData>>();

    }
}
