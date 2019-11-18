using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class PositionHandler : BasePollCNCHandler<ReadPositionModel, ReadPositionResultItemModel, ReadPositionTypeModel, DecompReadPositionItemModel>
    {
        Dictionary<CncPositionTypeEnum, int[]> datas;

        public PositionHandler()
        {
            datas = new Dictionary<CncPositionTypeEnum, int[]>();
        }
        protected override object PollDecompiler(List<ReadPositionResultItemModel> res, DecompReadPositionItemModel item)
        {
            int data = 0;
            if (!datas.ContainsKey(item.PositionType))
            {
                return "Position未正确读取，无法解析！";
            }
            var ret_dec =new PositionHelper().DecompilerReadPositionInfo(datas[item.PositionType], item, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadPositionResultItemModel()
                {
                    Id = item.Id,
                    Value = (double)data / SmartSystemCNCCoreConsts.CncIncrement
                });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadPositionTypeModel item)
        {
            
            int[] data = new int[Focas1.MAX_AXIS];
            var ret =new PositionHelper().ReadPositionRange(flib, item.PositionType, ref data);
            if (ret.Item1 == 0)
            {
                if (datas.ContainsKey(item.PositionType))
                {
                    datas[item.PositionType] = data;

                }
                else
                {
                    datas.Add(item.PositionType, data);

                }
            }
            return ret;
        }
    }
}
