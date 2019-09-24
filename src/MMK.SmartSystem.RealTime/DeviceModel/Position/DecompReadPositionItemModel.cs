using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceModel
{
    public class DecompReadPositionItemModel
    {
        public string Id { get; set; }

        public CncPositionTypeEnum PositionType { get; set; }

        public int AxisNum { get; set; }
    }
}
