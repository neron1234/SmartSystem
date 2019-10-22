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
        public List<ProgramViewModel> CNCProgramViews { get; set; }

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

        public ReadProgramFolderItemViewModel ProgramFolder { get; set; }

        private CNCProgramPath _CNCPath;
        public CNCProgramPath CNCPath
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
            
        }

        public ICommand LoadFileCommand{
            get{
                return new RelayCommand(() => {
                    
                });
            }
        }

        public ICommand LocalListCommand{
            get{
                return new RelayCommand(() => {
                    this.ListControl = new LocalProgramListControl(this.ConnectId,this.ProgramFolder);
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
        public string Page { get; set; }
        public string Path { get; set; }
        public CNCProgramPath(string path,string page)
        {
            Path = path;
            Page = page;
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
