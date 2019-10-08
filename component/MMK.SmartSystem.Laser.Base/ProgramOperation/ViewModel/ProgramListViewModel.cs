using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel
{
    public class ProgramListViewModel:ViewModelBase
    {
        private string _SelectedName;
        public string SelectedName
        {
            get { return _SelectedName; }
            set
            {
                if (_SelectedName != value)
                {
                    _SelectedName = value;
                    RaisePropertyChanged(() => SelectedName);
                }
            }
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {
                if (_Path != value)
                {
                    _Path = value;
                    RaisePropertyChanged(() => Path);
                }
            }
        }

        private ObservableCollection<ProgramInfo> _ProgramList;
        public ObservableCollection<ProgramInfo> ProgramList
        {
            get { return _ProgramList; }
            set
            {
                if (_ProgramList != value)
                {
                    _ProgramList = value;
                    RaisePropertyChanged(() => ProgramList);
                }
            }
        }
        public ProgramListViewModel()
        {
            this.SelectedName = "程序名称:";
        }
    }
    
    public class ProgramInfo
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string CreateTime { get; set; }
    }
}
