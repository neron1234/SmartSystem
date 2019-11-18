using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceInOut
{
    public class ProgramListInOut : BaseInOut
    {
        public HubReadWriterResultModel Reader(HubReadWriterModel hubRead)
        {           
            List<ReadProgramListItemResultModel> data = new List<ReadProgramListItemResultModel>();
            var res =new ProgramListHelper().ReadProgramList(flib, hubRead.Data[0].ToString(), ref data);
            return new HubReadWriterResultModel()
            {             
                Result = data,
                Error = res.Item2,
                Success = res.Item1 == 0,
            };
        }
    }
}
