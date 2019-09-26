using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceHandlers.CNC
{
    public class MacroHandler : BasePollCNCHandler<ReadMacroModel, ReadMacroResultItemModel, ReadMacroTypeModel, DecompReadMacroItemModel>
    {
        double[] datas;
        public MacroHandler(ushort flib) : base(flib)
        {

        }
        protected override object PollDecompiler(List<ReadMacroResultItemModel> res, DecompReadMacroItemModel deModel)
        {
            double data = 0;
            var ret_dec = MacroHelper.DecompilerReadMacroInfo(datas, deModel, ref data);
            if (string.IsNullOrEmpty(ret_dec))
            {
                res.Add(new ReadMacroResultItemModel() { Id = deModel.Id, Value = data });
            }
            return ret_dec;
        }

        protected override Tuple<short, string> PollRead(ReadMacroTypeModel inputModel)
        {
            datas = new double[inputModel.Quantity];

            var ret = MacroHelper.ReadMacroRange(flib, inputModel.StartNum, inputModel.Quantity, ref datas);
            return ret;
        }
    }
}
