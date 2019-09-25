﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceModel.ProgramList
{
    public class ReadProgramListItemResultModel
    {

        public string Name { get; set; }

        public int Size { get; set; }

        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
