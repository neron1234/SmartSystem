﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Host.DeviceModel
{
    public class ReadPmcTypeModel
    {
        public short AdrType { get; set; }

        public ushort StartNum { get; set; }

        public ushort DwordQuantity { get; set; }
    }
}