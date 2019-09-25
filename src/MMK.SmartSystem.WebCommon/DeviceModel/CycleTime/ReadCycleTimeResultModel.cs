using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class ReadCycleTimeResultModel
    {
        public double TotalSeconds { get; set; }

        public string CycleTimeStr
        {
            get
            {
                var hour = Math.Floor(TotalSeconds / 3600);
                var min = Math.Floor(TotalSeconds % 3600 / 60);
                var sec = Math.Floor(TotalSeconds % 60);

                return (hour == 0 ? "" : (hour.ToString("00") + "H:")) + ((hour == 0 && min == 0) ? "" : (min.ToString("00") + "M: ")) + sec.ToString("00") + "S";
                
            }
        }
    }
}
