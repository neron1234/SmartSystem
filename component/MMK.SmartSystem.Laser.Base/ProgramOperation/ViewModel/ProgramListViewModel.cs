using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls;
using MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel
{
    public class ProgramListViewModel:ViewModelBase
    {
        private UserControl _ListControl;
        public UserControl ListControl
        {
            get { return _ListControl; }
            set
            {
                if (_ListControl != value)
                {
                    _ListControl = value;
                    RaisePropertyChanged(() => ListControl);
                }
            }
        }

        private UserControl _InfoControl;
        public UserControl InfoControl
        {
            get { return _InfoControl; }
            set
            {
                if (_InfoControl != value)
                {
                    _InfoControl = value;
                    RaisePropertyChanged(() => InfoControl);
                }
            }
        }

        private string _CNCPath;
        public string CNCPath
        {
            get { return _CNCPath; }
            set
            {
                if (_CNCPath != value)
                {
                    _CNCPath = value;
                    RaisePropertyChanged(() => CNCPath);
                }
            }
        }


        public string ConnectId { get; set; }
        public ProgramListViewModel()
        {
            this.ListControl = new CNCProgramListControl(this.ProgramFolderInfo);
            this.InfoControl = new CNCProgramInfoControl();
            this.CNCPath = "//CNC_MEM/USER/PATH1/";
        }

        public ICommand LoadFileCommand{
            get{
                return new RelayCommand(() => {
                    
                });
            }
        }

        public ICommand CNCListCommand{
            get{
                return new RelayCommand(() => {
                    this.ListControl = new CNCProgramListControl(this.ProgramFolderInfo);
                    this.InfoControl = new CNCProgramInfoControl();
                });
            }
        }

        public ICommand LocalListCommand{
            get{
                return new RelayCommand(() => {
                    this.ListControl = new LocalProgramListControl(this.ConnectId, this.ProgramFolderInfo);
                    this.InfoControl = new LocalProgramInfoControl();
                });
            }
        }

        public ICommand CNCInfoCommand{
            get{
                return new RelayCommand(() => {
                    this.ListControl = new CNCInfoControl();
                    this.InfoControl = new UserControl();
                });
            }
        }

        private ReadProgramFolderItemViewModel _ProgramFolderInfo;
        public ReadProgramFolderItemViewModel ProgramFolderInfo
        {
            get { return _ProgramFolderInfo; }
            set
            {
                if (_ProgramFolderInfo != value)
                {
                    _ProgramFolderInfo = value;
                    RaisePropertyChanged(() => ProgramFolderInfo);
                }
            }
        }
    }
    public class LocalProgramPath
    {
        public string Path { get; set; }
        public LocalProgramPath(string path)
        {
            Path = path;
        }
    }

    public class CNCProgramPath
    {
        public string Path { get; set; }
        public CNCProgramPath(string path)
        {
            Path = path;
        }
    }

    public class PageConnect
    {
        public string ConnectId { get; set; }
        public PageConnect(string cId)
        {
            ConnectId = cId;
        }
    }
}
