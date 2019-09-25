using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public enum CncEventEnum
    {
        ReadMacro,
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
