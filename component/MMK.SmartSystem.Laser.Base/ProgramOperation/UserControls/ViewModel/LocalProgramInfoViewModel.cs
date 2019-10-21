using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class LocalProgramInfoViewModel:ViewModelBase{
        private string _PreviewText;
        public string PreviewText
        {
            get { return _PreviewText; }
            set
            {
                if (_PreviewText != value)
                {
                    _PreviewText = value;
                    RaisePropertyChanged(() => PreviewText);
                }
            }
        }
    }
}
