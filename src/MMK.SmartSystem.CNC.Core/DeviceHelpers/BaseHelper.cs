using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHelpers
{
    public class BaseHelper
    {
        public string Ip { get; set; }

        public ushort Port { get; set; }

        public ushort TimeOut { get; set; }

        public short LaserLibraryCuttingDataQuantity { get; set; } = 10;

        public short LaserLibraryEdgeCuttingDataQuantity { get; set; } = 5;

        public short LaserLibraryPiercingDataQuantity { get; set; } = 3;

        public short LaserLibrarySlopeControlDataQuantity { get; set; } = 5;

        public short BuildConnect(ref ushort flib)
        {
            Focas1.cnc_freelibhndl(flib);

            var ret = Focas1.cnc_allclibhndl3(Ip, Port, TimeOut, out flib);
            return ret;
        }

        public void FreeConnect(ushort flib)
        {
            Focas1.cnc_freelibhndl(flib);
            flib = 0;
        }

    }
}
