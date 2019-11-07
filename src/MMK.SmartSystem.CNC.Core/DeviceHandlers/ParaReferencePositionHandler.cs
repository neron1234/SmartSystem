using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class ParaReferencePositionHandler : BasePollCNCHandler<ReadParaReferencePositionModel, ReadParaReferencePositionResultItemModel, ReadParaReferencePositionTypeModel, DecompReadParaReferencePositionItemModel>
    {
        double[,] datas;

        public ParaReferencePositionHandler()
        {
            datas = new double[4, Focas1.MAX_AXIS];
        }

        protected override object PollDecompiler(List<ReadParaReferencePositionResultItemModel> res, DecompReadParaReferencePositionItemModel item)
        {
            double data = 0;

            var ret_dec = new ParaReferencePositionHelper().DecompilerReadParaReferencePositionInfo(datas, item, ref data);
            res.Add(new ReadParaReferencePositionResultItemModel()
            {
                Id = item.Id,
                Value = data
            });

            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadParaReferencePositionTypeModel inputModel)
        {
            var ret = new ParaReferencePositionHelper().ReadParaReferencePositionRange(flib, inputModel.ReferencePositionType, inputModel.Qty, ref datas);

            return ret;
        }

        public override ReadParaReferencePositionModel MargePollRequest(ReadParaReferencePositionModel pre, ReadParaReferencePositionModel current)
        {
            foreach (var item in current.Decompilers)
            {
                if(pre.Readers.Count() == 0)
                {
                    pre.Readers.Add(new ReadParaReferencePositionTypeModel
                    {
                        ReferencePositionType= item.ReferencePositionType,
                        Qty=1,
                    });
                }
                else
                {
                    var start = pre.Readers[0].ReferencePositionType;
                    var end = pre.Readers[0].ReferencePositionType + pre.Readers[0].Qty;


                    var temp_start = start > item.ReferencePositionType ? item.ReferencePositionType : start;
                    var temp_end = item.ReferencePositionType > end ? item.ReferencePositionType : end;
                    pre.Readers[0].ReferencePositionType = temp_start;
                    pre.Readers[0].Qty = temp_end - temp_start + 1;
                }

                pre.Decompilers.Add(new DecompReadParaReferencePositionItemModel()
                {
                    Id = item.Id,
                    ReferencePositionType = item.ReferencePositionType,
                    AxisNum = item.AxisNum,
                });

            }

            
            return pre;
        }
    }
}
