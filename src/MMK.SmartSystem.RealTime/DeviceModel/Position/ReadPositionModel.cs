using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.RealTime.DeviceModel
{
    public class ReadPositionModel
    {
        public List<ReadPositionTypeModel> Readers { get; set; } = new List<ReadPositionTypeModel>();

        public List<DecompReadPositionItemModel> Decompilers { get; set; } = new List<DecompReadPositionItemModel>();
    }
}
