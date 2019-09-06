using Abp.Modules;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.Model;
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
            List<SystemMenuModule> menuModules = new List<SystemMenuModule>{
                new SystemMenuModule{
                    ModuleName = "MachineOperation",
                    Pages = new List<SystemPageModel>{
                        new SystemPageModel(){ Title="MachineOperation.AutoFindSide",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AutoFindSidePage",IsAuth=true,Permission="MachineOperation.AutoFindSidePage"},
                        new SystemPageModel(){ Title="MachineOperation.AuxGasCheck",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AuxGasCheckPage",IsAuth=false,Permission="MachineOperation.AuxGasCheckPage"},
                        new SystemPageModel(){ Title="MachineOperation.ManualFindSide",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.ManualFindSidePage",IsAuth=true,Permission="MachineOperation.ManualFindSidePage"},
                    }
                },
                new SystemMenuModule{
                     ModuleName = "ManualFindSide",
                     Pages = new List<SystemPageModel>{
                        new SystemPageModel(){ Title="ManualFindSide.CutterResetCheck",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.CutterResetCheckPage",IsAuth=true,Permission="MachineOperation.CutterResetCheckPage"},
                        new SystemPageModel(){ Title="ManualFindSide.AutoCutterClean",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AutoCutterCleanPage",IsAuth=true,Permission="MachineOperation.AutoCutterCleanPage"},
                        new SystemPageModel(){ Title="ManualFindSide.CutCenter",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.CutCenterPage",IsAuth=true,Permission="MachineOperation.CutCenterPage"}
                     }
                }
            };
            Configuration.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>()).AddRange(menuModules);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
