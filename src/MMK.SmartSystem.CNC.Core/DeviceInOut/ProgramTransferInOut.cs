using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceInOut
{
    public class ProgramTransferInOut : BaseInOut
    {
        /// <summary>
        /// 删除CNC程序
        /// </summary>
        /// <param name="hubRead"></param>
        /// <returns></returns>
        public HubReadWriterResultModel DeleteProgram(HubReadWriterModel hubRead)
        {

            var res = new ProgramTransferHelper().DeleteProgramInCnc(flib, hubRead.Data[0].ToString());
            return new HubReadWriterResultModel()
            {
                Result = res,
                Error = res,
                Success = string.IsNullOrEmpty(res)
            };
        }
        public HubReadWriterResultModel UploadProgramToCNC(HubReadWriterModel hubRead)
        {

            var res = new ProgramTransferHelper().LocalDownloadProgramFromPcToCnc(flib, hubRead.Data[0].ToString(), hubRead.Data[1].ToString());
            return new HubReadWriterResultModel()
            {
                Result = res,
                Error = res,
                Success = string.IsNullOrEmpty(res)
            };
        }
        
    }
}
