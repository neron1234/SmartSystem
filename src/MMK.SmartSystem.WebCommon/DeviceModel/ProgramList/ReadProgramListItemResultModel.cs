using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramListItemResultModel: BaseCncResultModel
    {

        public string Name { get; set; }

        public int Size { get; set; }

        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
