using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadParaReferencePositionModel : CncReadDecoplilersModel<ReadParaReferencePositionTypeModel, DecompReadParaReferencePositionItemModel>
    {

    }
    public class ReadParaReferencePositionResultItemModel : BaseCncResultModel
    {
        [Newtonsoft.Json.JsonProperty("id")]

        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]

        public double Value { get; set; }
    }

    public class ReadParaReferencePositionTypeModel : IEquatable<ReadParaReferencePositionTypeModel>
    {
        public int ReferencePositionType { get; set; }

        public int Qty { get; set; }

        public bool Equals(ReadParaReferencePositionTypeModel other)
        {
            return ReferencePositionType == other.ReferencePositionType && Qty == other.Qty;
        }
        public override int GetHashCode()
        {
            return ReferencePositionType.GetHashCode();
        }
    }
    public class DecompReadParaReferencePositionItemModel : IEquatable<DecompReadParaReferencePositionItemModel>
    {
        public string Id { get; set; }

        public int ReferencePositionType { get; set; }

        public int AxisNum { get; set; }

        public bool Equals(DecompReadParaReferencePositionItemModel other)
        {
            return Id == other.Id && ReferencePositionType == other.ReferencePositionType && AxisNum == other.AxisNum;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + AxisNum * 10;
        }
    }
}
