using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadMacroModel : CncReadDecoplilersModel<ReadMacroTypeModel, DecompReadMacroItemModel>
    {
     
    }

    public class ReadMacroTypeModel:IEquatable<ReadMacroTypeModel>
    {
        public ushort StartNum { get; set; }

        public int Quantity { get; set; }

        public bool Equals(ReadMacroTypeModel other)
        {
            return StartNum == other.StartNum && Quantity == other.Quantity;
        }
    }

    public class DecompReadMacroItemModel:IEquatable<DecompReadMacroItemModel>
    {
        public string Id { get; set; }

        public short StartNum { get; set; }

        public short RelStartNum { get; set; }

        public bool Equals(DecompReadMacroItemModel other)
        {
            return Id == other.Id && StartNum == other.StartNum && RelStartNum == other.RelStartNum;
        }
    }

    public class ReadMacroResultItemModel : BaseCncResultModel
    {
        public string Id { get; set; }

        public double Value { get; set; }
    }
}
