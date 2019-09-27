using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadProgramNameModel:CncReadDecoplilersModel<string,string>
    {
    }

    public class ReadProgramNameResultItemModel
    {
        public string Name { get; set; }

        public string FullName { get; set; }
    }
    public class ReadProgramNameResultModel:BaseCncResultModel
    {

        public string Id { get; set; }

        public ReadProgramNameResultItemModel Value { get; set; } = new ReadProgramNameResultItemModel();
    }
}
