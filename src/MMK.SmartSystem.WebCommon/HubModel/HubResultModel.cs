using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.HubModel
{
    public class HubReadWriterModel
    {
        public string Id { get; set; }
        public string ConnectId { get; set; }

        public string ProxyName { get; set; }

        public string Action { get; set; }
        public object[] Data { get; set; }

        public string SuccessTip { get; set; }

        public string ErrorTip { get; set; }
    }

    public class HubReadWriterResultModel
    {
        public string Id { get; set; }
        public string ConnectId { get; set; }

        public string ProxyName { get; set; }

        public string Action { get; set; }

        public object Result { set; get; }

        public string Error { get; set; }
        public string SuccessTip { get; set; }

        public string ErrorTip { get; set; }
        public bool Success { get; set; }
    }
    public class HubResultModel
    {
        public object Data { get; set; }

        public string Time { get; set; }
    }
}
