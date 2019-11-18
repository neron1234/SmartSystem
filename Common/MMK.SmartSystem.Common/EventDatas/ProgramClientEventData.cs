using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class UpLoadProgramClientEventData : BaseApiEventData<object>
    {
        public FileParameter FileParameter { get; set; }
        public string ConnectId { get; set; }
        public string FileHashCode { get; set; }
    }

    public class UpdateProgramClientEventData : BaseApiEventData<object>
    {
        public UpdateProgramDto Data { get; set; }
    }
}
