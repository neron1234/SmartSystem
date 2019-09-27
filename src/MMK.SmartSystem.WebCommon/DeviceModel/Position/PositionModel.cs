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
        Absolute,
        Relative,
        Machine,
        Distance,
    }
    public class ReadPositionTypeModel
    {
        public CncPositionTypeEnum PositionType { get; set; }

    }
    public class DecompReadPositionItemModel
    {
        public string Id { get; set; }

        public CncPositionTypeEnum PositionType { get; set; }

        public int AxisNum { get; set; }
    }

}
