using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceInOut
{
    public class ProgramTransferInOut:BaseInOut
    {
        public HubReadWriterResultModel Reader(HubReadWriterModel hubRead)
        {
            return new HubReadWriterResultModel();
        }
    }
}
