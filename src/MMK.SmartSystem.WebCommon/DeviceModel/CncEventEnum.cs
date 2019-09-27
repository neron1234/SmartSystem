using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class CncCustomEventAttribute : Attribute
    {
        public string ModelName { get; }
        public string HandlerName { get; }
        public CncCustomEventAttribute(string model, string handlerName)
        {
            ModelName = $"MMK.SmartSystem.WebCommon.DeviceModel.{ model}";
            HandlerName = $"MMK.SmartSystem.RealTime.DeviceHandlers.CNC.{handlerName}";
        }
    }
    public enum CncEventEnum
    {
        [CncCustomEvent("ReadMacroModel", "MacroHandler")]
        ReadMacro,

        [CncCustomEvent("ReadPmcModel", "PmcHandler")]
        ReadPmc,
        ReadPosition,
        ReadAlarm,
        ReadNotice,
        ReadProgramName,
        ReadProgramBlock,
        ReadProgramStr,
        //ReadProgramList,
        //ReadProgramFolder,
        ReadProgramInfo,
        ReadModalInfo,
        ReadCycleTime,
        ReadWorkpartNum,
        ReadSpindleSpeed,
        ReadFeedrate,
    }
}
