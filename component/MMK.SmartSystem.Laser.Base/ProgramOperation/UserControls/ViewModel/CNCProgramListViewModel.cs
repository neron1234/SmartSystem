using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramListViewModel:ViewModelBase{
        private ObservableCollection<ProgramViewModel> _ProgramList;
        public ObservableCollection<ProgramViewModel> ProgramList{
            get { return _ProgramList; }
            set{
                if (_ProgramList != value){
                    _ProgramList = value;
                    RaisePropertyChanged(() => ProgramList);
                }
            }
        }

        public List<ProgramViewModel> LocalProgramList { get; set; }

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

        private bool _NextIsEnable;
        public bool NextIsEnable
        {
            get { return _NextIsEnable; }
            set
            {
                if (_NextIsEnable != value)
                {
                    _NextIsEnable = value;
                    RaisePropertyChanged(() => NextIsEnable);
                }
            }
        }

        private bool _LastIsEnable;
        public bool LastIsEnable
        {
            get { return _LastIsEnable; }
            set
            {
                if (_LastIsEnable != value)
                {
                    _LastIsEnable = value;
                    RaisePropertyChanged(() => LastIsEnable);
                }
            }
        }

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageNumber = 10;
        public void DataPaging(int page)
        {
            if (LocalProgramList == null){
                return;
            }
            int count = LocalProgramList.Count;
            int pageSize = 0;
            if (count % PageNumber == 0)
            {
                pageSize = count / PageNumber;
            }
            else
            {
                pageSize = count / PageNumber + 1;
            }
            TotalPage = pageSize;
            CurrentPage = page;

            LastIsEnable = CurrentPage > 1;
            NextIsEnable = CurrentPage < TotalPage;

            this.ProgramList = new ObservableCollection<ProgramViewModel>(LocalProgramList.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList());
        }

        public CNCProgramListViewModel(ReadProgramFolderItemViewModel readProgramFolder)
        {
            this.ProgramFolderList = readProgramFolder;
            LocalProgramList = new List<ProgramViewModel>();
            Messenger.Default.Register<CNCProgramPath>(this, (cncPath) => {
                this.CNCPath = cncPath.Path;
            });
            Messenger.Default.Register<SearchInfo>(this, (sInfo) => {
                this.ProgramList.Clear();
                if (string.IsNullOrEmpty(sInfo.Search)){
                    DataPaging(1);
                    return;
                }
                foreach (var item in this.LocalProgramList.Where(n => n.Name.Contains(sInfo.Search))){
                    this.ProgramList.Add(item);
                }
            });
        }
        
        public ReadProgramFolderItemViewModel ProgramFolderList { get; set; }

        public ICommand MainProgramCommand
        {
            get
            {
                return new RelayCommand(() => {

                });
            }
        }

        public ICommand CNCPathCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new PopupWindow(new CNCPathControl(this.ProgramFolderList), 680, 240, "修改CNC路径").ShowDialog();
                });
            }
        }
        public ICommand DowloadCommand
        {
            get
            {
                return new RelayCommand(() => {

                });
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new PopupWindow(new SearchControl(), 680, 240, "搜索本地程序").ShowDialog();
                });
            }
        }

        public ICommand DeleteFileCommand
        {
            get
            {
                return new RelayCommand(() => {
                    
                });
            }
        }

        public ICommand LastPageCommand
        {
            get
            {
                return new RelayCommand(() => {
                    if (CurrentPage > 1)
                    {
                        DataPaging(--CurrentPage);
                    }
                });
            }
        }

        public ICommand NextPageCommand
        {
            get
            {
                return new RelayCommand(() => {
                    if (CurrentPage < TotalPage)
                    {
                        DataPaging(++CurrentPage);
                    }
                });
            }
        }
    }
}
