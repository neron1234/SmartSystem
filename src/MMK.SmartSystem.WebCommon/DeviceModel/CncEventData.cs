using Abp.Events.Bus;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class CncEventData : EventData
    {
        public CncEventEnum Kind { get; set; }

        // 
        public string Para { get; set; }
    }

  
}
