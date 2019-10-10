using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.Laser.Base.CustomControl.ViewModel
{
    public class PopupWindowViewModel:ViewModelBase
    {
        private ContentControl _PopupContent;
        public ContentControl PopupContent
        {
            get { return _PopupContent; }
            set
            {
                if (_PopupContent != value)
                {
                    _PopupContent = value;
                    RaisePropertyChanged(() => PopupContent);
                }
            }
        }
    }
}
