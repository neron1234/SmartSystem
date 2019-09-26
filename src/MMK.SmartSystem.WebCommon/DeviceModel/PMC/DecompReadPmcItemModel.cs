using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class DecompReadPmcItemModel
    {
        public string Id { get; set; }

        public short AdrType { get; set; }

        public DataTypeEnum DataType { get; set; }

        public short StartAdr { get; set; }

        public short RelStartAdr { get; set; }

        public ushort? Bit { get; set; }


    }

    public enum DataTypeEnum
    {
        Boolean,
        Byte,
        Int16,
        Int32
    }
}
