using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon
{
    public class MMKSmartSystemWebCommonConsts
    {
        public const short LaserLibraryCuttingDataQuantity = 10;

        public const short LaserLibraryEdgeCuttingDataQuantity = 5;

        public const short LaserLibraryPiercingDataQuantity = 3;

        public const short LaserLibrarySlopeControlDataQuantity = 5;

        public static ConcurrentDictionary<string, List<CncEventData>> PageCncEventDict = new ConcurrentDictionary<string, List<CncEventData>>();

    }
}
