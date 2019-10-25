using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class SlopeControlDataByGroupIdEventData : BaseApiEventData<List<SlopeControlDataDto>>
    {
        public int machiningDataGroupId { get; set; }
    }
    public class UpdateSlopeControlDataEventData : BaseApiEventData<SlopeControlDataDto>
    {
        public UpdateSlopeControlDataDto UpdateSlopeControlData { get; set; }
    }
}
