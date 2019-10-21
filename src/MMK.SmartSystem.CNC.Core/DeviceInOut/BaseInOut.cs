using MMK.SmartSystem.CNC.Core.DeviceHelpers;
using MMK.SmartSystem.WebCommon.HubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.CNC.Core.DeviceInOut
{
    public class BaseInOut : Abp.Dependency.ITransientDependency
    {
        protected ushort flib = 0;

        protected bool ConnectSuccess = true;
        bool IsConnect = false;

        public event Action<string> ConnectFailEvent;
        public BaseInOut()
        {
            if (!IsConnect)
            {
                IsConnect = Connect() == 0;
                if (!IsConnect)
                {
                    ConnectFailEvent?.Invoke("设备连接失败");
                    ConnectSuccess = false;
                }
            }

        }

        public bool BuildConnect()
        {
            return ConnectSuccess;
        }
        private short Connect()
        {
            return ConnectHelper.BuildConnect(ref flib, SmartSystemCNCCoreConsts.CncIP,
                SmartSystemCNCCoreConsts.CncPort, SmartSystemCNCCoreConsts.CncTimeout);


        }
    }
}
