using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class SlopeControlDataByGroupIdEventData : BaseErrorEventData
    {
        public int machiningDataGroupId { get; set; }
    }
    public class UpdateSlopeControlDataEventData : BaseErrorEventData
    {
        public UpdateSlopeControlDataDto UpdateSlopeControlData { get; set; }
    }
}
