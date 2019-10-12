using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessListViewModel:ViewModelBase
    {
        private Abp.Application.Services.Dto.PagedResultRequestDto _ProcessDataList;
        public Abp.Application.Services.Dto.PagedResultRequestDto ProcessDataList
        {
            get { return _ProcessDataList; }
            set     
            {
                if (_ProcessDataList != value)
                {
                    _ProcessDataList = value;
                    RaisePropertyChanged(() => ProcessDataList);
                }
            }
        }
    }
}
