﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramNameResultModel
    {

        public string Id { get; set; }

        public ReadProgramNameResultItemModel Value { get; set; } = new ReadProgramNameResultItemModel();
    }
}
