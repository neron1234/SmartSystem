using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.EventDatas
{
    public class UserConfigEventData : EventData
    {
        public string UserName { get; set; }

        public string Pwd { get; set; }
        public bool IsChangeUser { get; set; }
        public bool IsChangeLanguage { get; set; }
        public string Culture { get; set; }
    }
}
