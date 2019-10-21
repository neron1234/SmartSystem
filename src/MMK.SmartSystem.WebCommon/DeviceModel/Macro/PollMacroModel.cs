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
        public CncMacroTypeEnum Type { get; set; }

        public ushort StartNum { get; set; }

        public ushort EndNum { get; set; }

        public int Quantity { get; set; }

        public bool Equals(ReadMacroTypeModel other)
        {
            return StartNum == other.StartNum && Quantity == other.Quantity;
        }
    }

    public class DecompReadMacroItemModel:IEquatable<DecompReadMacroItemModel>
    {
        public string Id { get; set; }

        public CncMacroTypeEnum Type { get; set; }

        public ushort StartNum { get; set; }

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

    public enum CncMacroTypeEnum
    {
        LOCAL,
        ARRAY_TYPE,
        VOLATILE,
        NONVOLATILE,
        LASER_COMMON,
        PCODE_CONTROL,
        PCODE,
        EXTENDED_PCODE,
        OTHER,
    }
}
