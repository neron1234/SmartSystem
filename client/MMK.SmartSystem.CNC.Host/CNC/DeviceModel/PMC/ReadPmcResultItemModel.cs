using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.CNC.Host.DeviceModel
{
    public class ReadPmcResultItemModel
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"【{Id}】:{Value}";
        }
    }
}
