using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPositionModel : CncReadDecoplilersModel<ReadPositionTypeModel, DecompReadPositionItemModel>
    {

    }
    public class ReadPositionResultItemModel : BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]

        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]

        public double Value { get; set; }
    }
    public enum CncPositionTypeEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Absolute")]
        Absolute,
        [System.Runtime.Serialization.EnumMember(Value = @"Relative")]

        Relative,
        [System.Runtime.Serialization.EnumMember(Value = @"Machine")]

        Machine,
        [System.Runtime.Serialization.EnumMember(Value = @"Distance")]

        Distance,
    }
    public class ReadPositionTypeModel : IEquatable<ReadPositionTypeModel>
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CncPositionTypeEnum PositionType { get; set; }

        public bool Equals(ReadPositionTypeModel other)
        {
            return PositionType == other.PositionType;
        }
        public override int GetHashCode()
        {
            return PositionType.GetHashCode();
        }
    }
    public class DecompReadPositionItemModel : IEquatable<DecompReadPositionItemModel>
    {
        public string Id { get; set; }

        public CncPositionTypeEnum PositionType { get; set; }

        public int AxisNum { get; set; }

        public bool Equals(DecompReadPositionItemModel other)
        {
            return Id == other.Id && PositionType == other.PositionType && AxisNum == other.AxisNum;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + AxisNum * 10;
        }
    }

}
