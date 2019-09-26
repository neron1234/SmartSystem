using GalaSoft.MvvmLight;
using MMK.SmartSystem.WebCommon.DeviceModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.ViewModel
{
    public class CncResultViewModel<T> : ViewModelBase where T : BaseCncResultModel
    {
        public List<T> GetCncResult(JObject json)
        {
            var name = json["fullNamespace"]?.ToString();
            if (string.IsNullOrEmpty(name))
            {
                return new List<T>();
            }
            var str = json["value"].ToString();
            try
            {
                if (name == typeof(T).FullName)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(str);

                }
                return new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();

            }
        }
    }
}
