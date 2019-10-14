using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class DecompReadPmcItemModel : IEquatable<DecompReadPmcItemModel>
    {
        public string Id { get; set; }

        public short AdrType { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public DataTypeEnum DataType { get; set; }

        public short StartAdr { get; set; }

        public short RelStartAdr { get; set; }

        public ushort? Bit { get; set; }

        public bool Equals(DecompReadPmcItemModel other)
        {
            return Id == other.Id;
        }
    }

    public enum DataTypeEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Boolean")]
        Boolean,

        [System.Runtime.Serialization.EnumMember(Value = @"Byte")]
        Byte,

        [System.Runtime.Serialization.EnumMember(Value = @"Int16")]
        Int16,

        [System.Runtime.Serialization.EnumMember(Value = @"Int32")]
        Int32
    }
}
