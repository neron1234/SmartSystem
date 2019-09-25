using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadModalResultModel
    {
        public string Id { get; set; }
        public List<string> Modals { get; set; } = new List<string>();
    }
}
