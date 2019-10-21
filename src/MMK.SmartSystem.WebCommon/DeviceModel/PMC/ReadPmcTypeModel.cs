using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPmcTypeModel : IEquatable<ReadPmcTypeModel>
    {
        public short AdrType { get; set; }

        public ushort StartNum { get; set; }

        public ushort EndNum { get; set; }

        public ushort DwordQuantity { get; set; }


        public bool Equals(ReadPmcTypeModel other)
        {
            return ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return AdrType * 10002 * 2 + StartNum + 12 ^ 2 + DwordQuantity;
        }


        public override string ToString()
        {
            return $"AdrType:{AdrType} StartNum:{StartNum} DwordQuantity:{DwordQuantity}";
        }
    }
}
