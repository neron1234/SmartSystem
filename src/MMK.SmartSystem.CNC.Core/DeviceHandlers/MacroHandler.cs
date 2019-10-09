using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class MacroHandler : BasePollCNCHandler<ReadMacroModel, ReadMacroResultItemModel, ReadMacroTypeModel, DecompReadMacroItemModel>
    {
        double[] datas;
        public MacroHandler() 
        {

        }
        protected override object PollDecompiler(List<ReadMacroResultItemModel> res, DecompReadMacroItemModel deModel)
        {
            double data = 0;
            var ret_dec = new MacroHelper().DecompilerReadMacroInfo(datas, deModel, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadMacroResultItemModel() { Id = deModel.Id, Value = data });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadMacroTypeModel inputModel)
        {
            datas = new double[inputModel.Quantity];

            var ret = new MacroHelper().ReadMacroRange(flib, inputModel.StartNum, inputModel.Quantity, ref datas);
            return ret;
        }

        protected override Tuple<short, T> MargePollRequest<T>(T current, CncEventData data)
        {
            var paraModel = JsonConvert.DeserializeObject<T>(data.Para);

            var macro = current.Readers[0];
            var start = paraModel.Readers[0].StartNum > macro.StartNum ? paraModel.Readers[0].StartNum : macro.StartNum;
            var end = (paraModel.Readers[0].StartNum + paraModel.Readers[0].Quantity) 
                > (macro.StartNum + macro.Quantity) ? (paraModel.Readers[0].StartNum + paraModel.Readers[0].Quantity) : 
                (macro.StartNum + macro.Quantity);

            macro.StartNum = start;
            macro.Quantity = end - start;

            current.Decompilers.AddRange(paraModel.Decompilers);

            foreach (var item in current.Decompilers)
            {
                item.RelStartNum = (short)(item.StartNum - start);
            }

            return new Tuple<short, T>(0, current);
        }
    }
}
