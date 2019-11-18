using Abp.Dependency;
using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceHandlers
{
    public class ProgramNameHandler : BasePollCNCHandler<ReadProgramNameModel, ReadProgramNameResultModel, string, string>
    {
        ReadProgramNameResultItemModel temp;
        private string message;
        public ProgramNameHandler()
        {
            temp = new ReadProgramNameResultItemModel();
        }

        protected override object PollDecompiler(List<ReadProgramNameResultModel> res, string item)
        {
            res.Add(new ReadProgramNameResultModel()
            {
                Id = item,
                Value = temp
            });
            return message;
        }

        protected override Tuple<short, string> PollRead(string item)
        {

            var ret =new ProgramNameHelper().ReadProgramName(flib, ref temp);
            if (ret.Item1 != 0)
            {
                message = ret.Item2;
            }
            return ret;
        }
    }
}
