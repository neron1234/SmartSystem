using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadPmcModel
    {
        public List<ReadPmcTypeModel> Readers { get; set; } = new List<ReadPmcTypeModel>();

        public List<DecompReadPmcItemModel> Decompilers { get; set; } = new List<DecompReadPmcItemModel>();
    }
}
