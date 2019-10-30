using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class RemainCutViewModel:ViewModelBase
    {
        public ObservableCollection<RemainInfo> RemainList { get; set; }

        public RemainCutViewModel()
        {
            RemainList = new ObservableCollection<RemainInfo> {
                new RemainInfo{ Name = "801",X = "41.13",Y ="358.92"},
                new RemainInfo{ Name = "802",X = "781.25",Y ="25.8"},
                new RemainInfo{ Name = "803",X = "656.47",Y ="581.5"},
                new RemainInfo{ Name = "804",X = "466.56",Y ="893.9"},
                new RemainInfo{ Name = "805",X = "289.4",Y ="983.7"},
                new RemainInfo{ Name = "806",X = "1257.2",Y ="651"},
                new RemainInfo{ Name = "807",X = "82.8",Y ="2516.5"},
            };
        }
    }

    public class RemainInfo
    {
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }
}
