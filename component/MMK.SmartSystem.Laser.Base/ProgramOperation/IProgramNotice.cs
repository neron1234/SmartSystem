using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation
{
    public static class ProgramConfigConsts
    {
        public static string CNCPath = "//CNC_MEM/USER/PATH1/";

        public static string LocalPath = "";

        public static ReadProgramFolderItemViewModel CurrentReadProgramFolder = new ReadProgramFolderItemViewModel();
    }
    public interface IProgramNotice
    {
        event Action<HubReadWriterModel> RealReadWriterEvent;

        bool CanWork(HubReadWriterResultModel resultModel);

        void DoWork(HubReadWriterResultModel resultModel);
    }
}
