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
        [System.Runtime.Serialization.EnumMember(Value = @"ReadMacro")]
        [CncCustomEvent("MacroHandler")]
        ReadMacro,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadPmc")]
        [CncCustomEvent("PmcHandler")]
        ReadPmc,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadPosition")]
        [CncCustomEvent("PositionHandler")]
        ReadPosition,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadAlarm")]
        [CncCustomEvent("AlarmHandler")]
        ReadAlarm,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadNotice")]
        ReadNotice,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadProgramName")]
        [CncCustomEvent("ProgramNameHandler")]
        ReadProgramName,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadProgramBlock")]
        [CncCustomEvent("ProgramBlockHandler")]
        ReadProgramBlock,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadProgramStr")]
        [CncCustomEvent("ProgramStrHandler")]
        ReadProgramStr,
        //ReadProgramList,
        //ReadProgramFolder,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadProgramInfo")]
        ReadProgramInfo,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadModalInfo")]
        ReadModalInfo,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadCycleTime")]
        [CncCustomEvent("CycleTimeHandler")]
        ReadCycleTime,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadWorkpartNum")]
        ReadWorkpartNum,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadSpindleSpeed")]
        ReadSpindleSpeed,

        [System.Runtime.Serialization.EnumMember(Value = @"ReadFeedrate")]
        [CncCustomEvent("FeedrateHandler")]
        ReadFeedrate,
    }
}
