using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.Model
{
    public class SingalrResultMapModel<T> where T : BaseCncResultModel
    {
        public CncResultViewModel<T> ViewModels { get; set; }

        public SignalrMapModelEnum MapType { get; set; }

        public Func<List<T>, string, object> AutoPropMapAction { set; get; }
        public Action<List<T>> MapAction { get; set; }

    }

    public enum SignalrMapModelEnum
    {
        AutoPropMap,
        CustomAction
    }
}
