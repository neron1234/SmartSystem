using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common.ViewModel;
using MMK.SmartSystem.WebCommon.DeviceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Concurrent;
namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class SimpleProfileViewModel : ViewModelBase
    {

        public List<SimpleProfileItemViewModel> SimpleItems { get; set; } = new List<SimpleProfileItemViewModel>();

        public event Action<SimpleProfileItemViewModel> InputClickEvent;



        public SimpleProfileViewModel()
        {
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_h", Title = "引线H" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_i", Title = "长度I" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_j", Title = "宽度J" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_d", Title = "直径D" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_r", Title = "半径R" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_e", Title = "切割E" });
            SimpleItems.Add(new SimpleProfileItemViewModel() { Id = "simpleprofile_perice", Title = "穿孔E" });

            SimpleItems.ForEach(d => d.InputClickEvent += D_InputClickEvent);

        }

        private void D_InputClickEvent(SimpleProfileItemViewModel obj)
        {
            InputClickEvent?.Invoke(obj);
        }
    }

    public class SimpleProfileItemViewModel : CncResultViewModel<ReadMacroResultItemModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int MinValue { get; set; } = 0;

        public int MaxValue { get; set; } = 100;


        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged(() => Value);
                }
            }
        }
        public event Action<SimpleProfileItemViewModel> InputClickEvent;
        public ICommand InputCommand
        {
            get
            {
                return new RelayCommand(() => InputClickEvent?.Invoke(this));
            }
        }
        public SimpleProfileItemViewModel()
        {
            Value = "0";
        }
    }
}
