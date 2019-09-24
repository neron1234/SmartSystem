using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public enum CncEventEnum
    {
        ReadMacro,
        ReadPmc,
        Position,
        LampStatus,
        AlarmMessage,
        NoticeMessage,
        ReadProgramName,
        ReadProgramBlock,
        ReadProgramStr,
        ReadProgramList,
        ReadProgramFolder,
        ReadProgramInfo,
        ModelInfo,
        CycleTime,
        WorkpartNum,
    }
}
