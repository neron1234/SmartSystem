using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class SimpleProfileViewModel : ViewModelBase
    {
        private string _LeadH;
        public string LeadH
        {
            get { return _LeadH; }
            set
            {
                if (_LeadH != value)
                {
                    _LeadH = value;
                    RaisePropertyChanged(() => LeadH);
                }
            }
        }


        public ICommand InputCommand{
            get{
                return new RelayCommand<string>((str) => {
                    var windows = new InputWindow(this.LeadH, 0, 100, str);
                    windows.InputWindowFinishEvent += (s) => this.LeadH = s;
                    windows.ShowDialog();
                });
            }
        }
    }
}
