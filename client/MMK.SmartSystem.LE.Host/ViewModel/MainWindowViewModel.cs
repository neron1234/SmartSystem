using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _mainFrame;
        public object MainFrame
        {
            get { return _mainFrame; }
            set
            {
                if (_mainFrame != value)
                {
                    _mainFrame = value;
                    RaisePropertyChanged(() => MainFrame);
                }
            }
        }


    }

}
