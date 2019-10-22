using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class ProgramBlockHandler : BasePollCNCHandler<ReadProgramBlockModel, ReadProgramBlockResultModel, string, string>
    {
        ReadProgramBlockResultModel temp;
        private string message { get; set; }
        public ProgramBlockHandler()
        {
            temp = new ReadProgramBlockResultModel();
        }

        protected override object PollDecompiler(List<ReadProgramBlockResultModel> res, string item)
        {
            res.Add(new ReadProgramBlockResultModel()
            {
                Id = item,
                Value = temp.Value
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret = ProgramBlockHelper.ReadProgramBlock(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
