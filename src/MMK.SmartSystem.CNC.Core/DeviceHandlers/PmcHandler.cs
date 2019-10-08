using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class PmcHandler : BasePollCNCHandler<ReadPmcModel, ReadPmcResultItemModel, ReadPmcTypeModel, DecompReadPmcItemModel>
    {
        Dictionary<short, int[]> datas;
        public PmcHandler()
        {
            datas = new Dictionary<short, int[]>();

        }

        protected override object PollDecompiler(List<ReadPmcResultItemModel> res, DecompReadPmcItemModel deModel)
        {
            string data = "";
            if (!datas.ContainsKey(deModel.AdrType))
            {
                return "PMC未正确读取，无法解析！";
            }
            var ret_dec =  new PmcHelper().DecompilerReadPmcInfo(datas[deModel.AdrType], deModel, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadPmcResultItemModel() { Id = deModel.Id, Value = data });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadPmcTypeModel inputModel)
        {
            int[] data = new int[inputModel.DwordQuantity];
            var ret = new PmcHelper().ReadPmcRange(flib, inputModel.AdrType, inputModel.StartNum, inputModel.DwordQuantity, ref data);
            if (ret.Item1 == 0)
            {
                if (datas.ContainsKey(inputModel.AdrType))
                {
                    datas[inputModel.AdrType] = data;
                }
                else
                {
                    datas.Add(inputModel.AdrType, data);

                }
            }
            return ret;
        }
    }
}
