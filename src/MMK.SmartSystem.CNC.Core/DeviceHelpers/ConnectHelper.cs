using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public static class ConnectHelper
    {
        public static short BuildConnect(ref ushort flib,string ip, ushort port,int timeout)
        {
            Focas1.cnc_freelibhndl(flib);

            var ret = Focas1.cnc_allclibhndl3(ip, port, timeout, out flib);
            return ret;
        }

        public static void FreeConnect(ushort flib)
        {
            Focas1.cnc_freelibhndl(flib);
            flib = 0;
        }
    }
}
