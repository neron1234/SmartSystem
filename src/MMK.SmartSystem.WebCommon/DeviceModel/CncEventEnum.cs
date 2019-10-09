using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class CncCustomEventAttribute : Attribute
    {
        public string HandlerName { get; }
        public CncCustomEventAttribute(string handlerName)
        {
            HandlerName = $"MMK.SmartSystem.CNC.Core.DeviceHandlers.{handlerName}";
        }
    }
    public enum CncEventEnum
    {
        [CncCustomEvent("MacroHandler")]
        ReadMacro,

        [CncCustomEvent("PmcHandler")]
        ReadPmc,

        [CncCustomEvent("PositionHandler")]
        ReadPosition,
        [CncCustomEvent("AlarmHandler")]
        ReadAlarm,
        ReadNotice,

        [CncCustomEvent("ProgramNameHandler")]
        ReadProgramName,
        ReadProgramBlock,
        [CncCustomEvent("ProgramStrHandler")]
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
