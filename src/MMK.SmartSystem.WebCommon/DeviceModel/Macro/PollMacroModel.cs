using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadMacroModel : CncReadDecoplilersModel<ReadMacroTypeModel, DecompReadMacroItemModel>
    {
        protected override void ReaderUnionOperation(ReadMacroTypeModel macro)
        {
            var paraModel = Readers.Count > 0 ? Readers[0] : null;
            if (paraModel == null)
            {
                Readers.Add(macro);
                return;
            }
            var start = paraModel.StartNum > macro.StartNum ? paraModel.StartNum : macro.StartNum;
            var end = (paraModel.StartNum + paraModel.Quantity) > (macro.StartNum + macro.Quantity) ? (paraModel.StartNum + paraModel.Quantity) : (macro.StartNum + macro.Quantity);

            macro.StartNum = start;
            macro.Quantity = end - start;
        }
    }

    public class ReadMacroTypeModel
    {
        public ushort StartNum { get; set; }

        public int Quantity { get; set; }
    }

    public class DecompReadMacroItemModel
    {
        public string Id { get; set; }

        public short StartNum { get; set; }

        public short RelStartNum { get; set; }
    }

    public class ReadMacroResultItemModel : BaseCncResultModel
    {
        public string Id { get; set; }

        public double Value { get; set; }
    }
}
