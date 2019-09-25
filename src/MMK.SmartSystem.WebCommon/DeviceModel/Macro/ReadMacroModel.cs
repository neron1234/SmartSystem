using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadMacroModel
    {
        public ushort StartNum { get; set; }

        public int Quantity { get; set; }

        public List<DecompReadMacroItemModel> Decompilers { get; set; } = new List<DecompReadMacroItemModel>();
    }
}
