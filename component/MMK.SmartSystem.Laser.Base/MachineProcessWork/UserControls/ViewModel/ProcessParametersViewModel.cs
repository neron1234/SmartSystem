using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcessWork.UserControls.ViewModel
{
    public class ProcessParametersViewModel: ViewModelBase
    {
        private System.Windows.Visibility _Visibility1;
        public System.Windows.Visibility Visibility1
        {
            get { return _Visibility1; }
            set
            {
                if (_Visibility1 != value)
                {
                    _Visibility1 = value;
                    RaisePropertyChanged(() => Visibility1);
                }
            }
        }

        private System.Windows.Visibility _Visibility2;
        public System.Windows.Visibility Visibility2
        {
            get { return _Visibility2; }
            set
            {
                if (_Visibility2 != value)
                {
                    _Visibility2 = value;
                    RaisePropertyChanged(() => Visibility2);
                }
            }
        }

        public ICommand LoadPageCommand{
            get{
                return new RelayCommand(() => {
                    if (this.Visibility1 == System.Windows.Visibility.Visible){
                        this.Visibility1 = System.Windows.Visibility.Collapsed;
                        this.Visibility2 = System.Windows.Visibility.Visible;
                    }else{
                        this.Visibility1 = System.Windows.Visibility.Visible;
                        this.Visibility2 = System.Windows.Visibility.Collapsed;
                    }
                });
            }
        }
        public ProcessParametersViewModel()
        {
            this.Visibility1 = System.Windows.Visibility.Visible;
            this.Visibility2 = System.Windows.Visibility.Collapsed;
        }
    }
}
