using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base
{
    public class SmartSystemLaserModule : AbpModule
    {
        public override void PreInitialize()
        {
            List<string> meun = new List<string>()
            {
                $"自动寻边:MMK.SmartSystem.Laser.Base.MachineOperation.AutoFindSidePage",
                $"割嘴复归:MMK.SmartSystem.Laser.Base.MachineOperation.CutterResetCheckPage",
                $"割嘴清洁:MMK.SmartSystem.Laser.Base.MachineOperation.AutoCutterCleanPage",
                $"割嘴对中:MMK.SmartSystem.Laser.Base.MachineOperation.CutCenterPage",
                $"辅助气体:MMK.SmartSystem.Laser.Base.MachineOperation.AuxGasCheckPage",
                $"手动寻边:MMK.SmartSystem.Laser.Base.MachineOperation.ManualFindSidePage"
            };
            Configuration.GetOrCreate("WPF.Page", () => new List<string>()).AddRange(meun);

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
