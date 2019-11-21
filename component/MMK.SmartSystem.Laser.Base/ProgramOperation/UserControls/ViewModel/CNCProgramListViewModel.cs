using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramListViewModel : ViewModelBase
    {
        public ObservableCollection<ProgramViewModel> ProgramList { get; set; }

        public List<ProgramViewModel> LocalProgramList { get; set; }

        public event Action MainCommandEvent;
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

        private int _CurrentPage;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set
            {
                if (_CurrentPage != value)
                {
                    _CurrentPage = value;
                    RaisePropertyChanged(() => CurrentPage);
                }
            }
        }

        private int _TotalPage;
        public int TotalPage
        {
            get { return _TotalPage; }
            set
            {
                if (_TotalPage != value)
                {
                    _TotalPage = value;
                    RaisePropertyChanged(() => TotalPage);
                }
            }
        }

        public int PageNumber = 7;
        public void DataPaging(bool next = false)
        {
            pagingModel.Init(LocalProgramList, (d) => d.CreateTime, 7);
        }

        private PagingModel<ProgramViewModel> pagingModel;
        public event Action SetCNCProgramPath;

        public event Action DeleteProgramEvent;

        public event Action DownProgramEvent;
        public CNCProgramListViewModel()
        {
            this.ProgramList = new ObservableCollection<ProgramViewModel>();
            this.LocalProgramList = new List<ProgramViewModel>();
            pagingModel = new PagingModel<ProgramViewModel>();
            pagingModel.PagePagingEvent += PagingModel_PagePagingEvent;
        }

        private void PagingModel_PagePagingEvent(IEnumerable<ProgramViewModel> arg1, int arg2, int arg3)
        {
            this.ProgramList.Clear();
            arg1.ToList().ForEach(d => ProgramList.Add(d));
            CurrentPage = arg2;
            TotalPage = arg3;
        }

        public ICommand MainProgramCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MainCommandEvent?.Invoke();
                });
            }
        }

        public ICommand CNCPathCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetCNCProgramPath?.Invoke();
                });
            }
        }

        public ICommand DowloadCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DownProgramEvent?.Invoke();
                });
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var sc = new SearchControl();
                    new PopupWindow(sc, 680, 240, "搜索CNC程序").ShowDialog();
                    sc.sVM.SearchEvent += SVM_SearchEvent;
                });
            }
        }

        private void SVM_SearchEvent(string obj)
        {
            this.ProgramList.Clear();
            if (string.IsNullOrEmpty(obj))
            {
                DataPaging();
                return;
            }
            foreach (var item in this.LocalProgramList.Where(n => n.Name.Contains(obj)))
            {
                this.ProgramList.Add(item);
            }
        }

        public ICommand DeleteFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DeleteProgramEvent?.Invoke();
                });
            }
        }

        public ICommand NextPageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    pagingModel.CyclePage();
                });
            }
        }
    }
}
