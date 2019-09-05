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
            SystemMenuModule systemMenu = new SystemMenuModule()
            {
                ModuleName = "机床功能",
                Pages = new List<SystemPageModel>()
                {
                    new SystemPageModel(){ Title="自动寻边",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AutoFindSidePage",IsAuth=true,Permission="MachineOperation.AutoFindSidePage"},
                    new SystemPageModel(){ Title="割嘴复归",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.CutterResetCheckPage",IsAuth=true,Permission="MachineOperation.CutterResetCheckPage"},
                    new SystemPageModel(){ Title="割嘴清洁",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AutoCutterCleanPage",IsAuth=true,Permission="MachineOperation.AutoCutterCleanPage"},
                    new SystemPageModel(){ Title="割嘴对中",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.CutCenterPage",IsAuth=true,Permission="MachineOperation.CutCenterPage"},
                    new SystemPageModel(){ Title="辅助气体",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.AuxGasCheckPage",IsAuth=true,Permission="MachineOperation.AuxGasCheckPage"},
                    new SystemPageModel(){ Title="手动寻边",FullName="MMK.SmartSystem.Laser.Base.MachineOperation.ManualFindSidePage",IsAuth=true,Permission="MachineOperation.ManualFindSidePage"},

                }
            };
          
            Configuration.GetOrCreate(SmartSystemCommonConsts.ModulePageKey, () => new List<SystemMenuModule>()).AddRange((new List<SystemMenuModule>() { systemMenu}));



        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
