using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class CuttingDataByGroupIdEventData : BaseErrorEventData
    {
        public int machiningDataGroupId { get; set; }
    }

    public class UpdateCuttingDataEventData : BaseErrorEventData
    {
       public UpdateCuttingDataDto UpdateCuttingData { get; set; }
    }
}
