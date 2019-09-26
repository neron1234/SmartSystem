using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.WebCommon.DeviceModel
{
    public class BaseCNCResultModel<T> where T :new()
    {
        public string Id { get; set; }

        public string FullNamespace { get; private set; }

        public BaseCNCResultModel()
        {
            FullNamespace = typeof(T).FullName;
        }

        public string ErrorMessage { get; set; }
        public List<T> Value { get; set; } = new List<T>();
    }
}
