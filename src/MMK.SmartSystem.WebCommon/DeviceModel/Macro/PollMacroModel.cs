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
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

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
        [System.Runtime.Serialization.EnumMember(Value = @"LOCAL")]
        LOCAL,
        [System.Runtime.Serialization.EnumMember(Value = @"ARRAY_TYPE")]
        ARRAY_TYPE,
        [System.Runtime.Serialization.EnumMember(Value = @"VOLATILE")]

        VOLATILE,
        [System.Runtime.Serialization.EnumMember(Value = @"NONVOLATILE")]

        NONVOLATILE,
        [System.Runtime.Serialization.EnumMember(Value = @"LASER_COMMON")]

        LASER_COMMON,
        [System.Runtime.Serialization.EnumMember(Value = @"PCODE_CONTROL")]

        PCODE_CONTROL,
        [System.Runtime.Serialization.EnumMember(Value = @"PCODE")]

        PCODE,
        [System.Runtime.Serialization.EnumMember(Value = @"EXTENDED_PCODE")]

        EXTENDED_PCODE,
        [System.Runtime.Serialization.EnumMember(Value = @"OTHER")]
        OTHER,
    }
}
