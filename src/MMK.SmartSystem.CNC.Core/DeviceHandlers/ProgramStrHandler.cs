using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class ProgramStrHandler : BasePollCNCHandler<ReadProgramStrModel, ReadProgramStrResultModel, string, string>
    {
        ReadProgramStrResultModel temp;
        private string message;
        public ProgramStrHandler()
        {
            temp = new ReadProgramStrResultModel();
        }

        protected override object PollDecompiler(List<ReadProgramStrResultModel> res, string item)
        {
            res.Add(new ReadProgramStrResultModel()
            {
                Id = item,
                Value = temp.Value
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {
            var ret = ProgramStrHelper.ReadProgramStr(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
