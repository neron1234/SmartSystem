using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceInOut
{
    public class PmcInOut : BaseInOut
    {
        public HubReadWriterResultModel Reversal(HubReadWriterModel hubRead)
        {
            short adrType = (short)Convert.ToInt32(hubRead.Data[0].ToString());
            ushort adr = (ushort)Convert.ToInt32(hubRead.Data[1].ToString());
            ushort bit = (ushort)Convert.ToInt32(hubRead.Data[2].ToString());

            var res = new PmcHelper().ReversalPmcBit(flib, adrType, adr, bit);
            return new HubReadWriterResultModel()
            {
                Result = res,
                Error = res,
                Success = string.IsNullOrEmpty(res)
            };
        }
    }
}
