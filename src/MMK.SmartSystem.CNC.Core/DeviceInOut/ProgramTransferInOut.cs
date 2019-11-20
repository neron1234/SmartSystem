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
            string name = "";
            var res = new ProgramTransferHelper().LocalDownloadProgramFromPcToCnc(flib, hubRead.Data[0].ToString(), hubRead.Data[1].ToString(), ref name);
            return new HubReadWriterResultModel()
            {
                Result = name,
                Error = res,
                Success = string.IsNullOrEmpty(res)
            };
        }

        public HubReadWriterResultModel MainProgramToCNC(HubReadWriterModel hubRead)
        {
            var res = new ProgramListHelper().SelectMainProgram(flib, hubRead.Data[0].ToString());
            return new HubReadWriterResultModel()
            {
                Result = hubRead.Data[0].ToString(),
                Error = res.Item2,
                Success = res.Item1 == 0
            };
        }

        public HubReadWriterResultModel DownloadProgram(HubReadWriterModel hubRead)
        {
            string name = "";
            var res = new ProgramTransferHelper().LocalUploadProgramFromCncToPc(flib, hubRead.Data[0].ToString(), hubRead.Data[1].ToString(), 0);
            return new HubReadWriterResultModel()
            {
                Result = name,
                Error = res,
                Success = string.IsNullOrEmpty(res)
            };
        }

        public HubReadWriterResultModel ReadProgramInfo(HubReadWriterModel hubRead)
        {
            ReadProgramInfoResultModel readProgramInfo = new ReadProgramInfoResultModel();
            var res = new ProgramInfoHelper().ReadProgramInfo(flib, ref readProgramInfo);
            return new HubReadWriterResultModel()
            {
                Result = readProgramInfo,
                Error = res.Item2,
                Success = res.Item1 == 0
            };
        }

    }
}
