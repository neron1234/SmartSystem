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
    public class ProgramFolderInOut : BaseInOut
    {
        public HubReadWriterResultModel Reader(HubReadWriterModel hubRead)
        {
            ReadProgramFolderItemModel data = new ReadProgramFolderItemModel();
            data.Folder = hubRead.Data[0].ToString();
            data.RegNum = 0;
            var res = new ProgramFolderHelper().ReadProgramFolder(flib, ref data);
            return new HubReadWriterResultModel()
            {
                Result = data,
                Error = res.Item2,
                Success = res.Item1 == 0,
                SuccessTip = hubRead.SuccessTip,
                ErrorTip = hubRead.ErrorTip
            };
        }
    }
}
