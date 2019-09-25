using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMonitor.ViewModel
{
    public class CoordinateViewModel : ViewModelBase
    {
        private string _X;
        public string X
        {
            get { return _X; }
            set
            {
                if (_X != value)
                {
                    _X = value;
                    RaisePropertyChanged(() => X);
                }
            }
        }

        private string _Y;
        public string Y
        {
            get { return _Y; }
            set
            {
                if (_Y != value)
                {
                    _Y = value;
                    RaisePropertyChanged(() => Y);
                }
            }
        }

        private string _Z;
        public string Z
        {
            get { return _Z; }
            set
            {
                if (_Z != value)
                {
                    _Z = value;
                    RaisePropertyChanged(() => Z);
                }
            }
        }
    }
}
