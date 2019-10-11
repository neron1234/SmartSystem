using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class PiercingDataByGroupIdEventData : BaseErrorEventData
    {
        public int machiningDataGroupId { get; set; }
    }
    public class UpdatePiercingDataEventData : BaseErrorEventData
    {
        public UpdatePiercingDataDto UpdatePiercingData { get; set; }
    }
}
