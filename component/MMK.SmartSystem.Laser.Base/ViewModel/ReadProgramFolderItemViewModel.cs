using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base
{
    public class ReadProgramFolderItemViewModel:ViewModelBase
    {
        private int _RegNum;
        public int RegNum
        {
            get { return _RegNum; }
            set
            {
                if (_RegNum != value)
                {
                    _RegNum = value;
                    RaisePropertyChanged(() => RegNum);
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private string _Folder;
        public string Folder
        {
            get { return _Folder; }
            set
            {
                if (_Folder != value)
                {
                    _Folder = value;
                    RaisePropertyChanged(() => Folder);
                }
            }
        }

        private ObservableCollection<ReadProgramFolderItemViewModel> _Nodes;
        public ObservableCollection<ReadProgramFolderItemViewModel> Nodes
        {
            get { return _Nodes; }
            set
            {
                if (_Nodes != value)
                {
                    _Nodes = value;
                    RaisePropertyChanged(() => Nodes);
                }
            }
        }
    }
}
