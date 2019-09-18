using MMK.SmartSystem.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineOperation.ViewModel
{
    public class AutoFindSidePageViewModel: MainTranslateViewModel
    {
        private bool _IsEdit;
        public bool IsEdit
        {
            get { return _IsEdit; }
            set
            {
                if (_IsEdit != value)
                {
                    _IsEdit = value;
                    RaisePropertyChanged(() => IsEdit);
                }
            }
        }
    }
}
