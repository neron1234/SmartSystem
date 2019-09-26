using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers.CNC
{
    public class PositionHandler : BasePollCNCHandler<ReadPositionModel, ReadPositionResultItemModel, ReadPositionTypeModel, DecompReadPositionItemModel>
    {
        Dictionary<CncPositionTypeEnum, int[]> datas;

        public PositionHandler(ushort flib) : base(flib)
        {
            datas = new Dictionary<CncPositionTypeEnum, int[]>();
        }
        protected override object PollDecompiler(List<ReadPositionResultItemModel> res, DecompReadPositionItemModel item)
        {
            int data = 0;
            var ret_dec = PositionHelper.DecompilerReadPositionInfo(datas[item.PositionType], item, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadPositionResultItemModel()
                {
                    Id = item.Id,
                    Value = (double)data / SmartSystemRealTimeConsts.CncIncrement
                });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadPositionTypeModel item)
        {
            int[] data = new int[Focas1.MAX_AXIS];
            var ret = PositionHelper.ReadPositionRange(flib, item.PositionType, ref data);
            if (ret.Item1 == 0)
            {
                datas.Add(item.PositionType, data);
            }
            return ret;
        }
    }
}
