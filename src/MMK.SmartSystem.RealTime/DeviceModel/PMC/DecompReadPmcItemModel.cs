using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.RealTime.DeviceModel
{
    public class DecompReadPmcItemModel
    {
        public string Id { get; set; }

        public short AdrType { get; set; }

        public Type DataType { get; set; }

        public short RelStartAdr { get; set; }

        public ushort? Bit { get; set; }


    }
}
