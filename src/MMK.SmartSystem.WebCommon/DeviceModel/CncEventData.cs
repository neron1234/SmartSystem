using Abp.Events.Bus;
using System.Collections.Generic;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class GroupEventData
    {
        public string GroupName { get; set; }

        public List<CncEventData> Data { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public GroupEventOperationEnum Operation { set; get; }
    }

    public enum GroupEventOperationEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"Add")]
        Add,

        [System.Runtime.Serialization.EnumMember(Value = @"Remove")]
        Remove
    }
    public class CncEventData : EventData
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public CncEventEnum Kind { get; set; }

        public string Para { get; set; }
    }


}
