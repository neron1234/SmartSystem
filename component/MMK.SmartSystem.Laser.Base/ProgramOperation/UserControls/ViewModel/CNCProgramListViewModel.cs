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
        public ObservableCollection<ProgramViewModel> ProgramList { get; set; }

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

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageNumber = 7;
        public void DataPaging(bool next = false)
        {
            if (LocalProgramList == null){
                return;
            }
            int count = LocalProgramList.Count;
            int pageSize = 0;
            if (count % PageNumber == 0){
                pageSize = count / PageNumber;
            }else{
                pageSize = count / PageNumber + 1;
            }
            TotalPage = pageSize;
            if (next && CurrentPage >= 1 && CurrentPage < TotalPage){
                CurrentPage++;
            }else{
                CurrentPage = 1;
            }

            this.ProgramList.Clear();
            foreach (var item in LocalProgramList.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList())
            {
                this.ProgramList.Add(item);
            }
            //this.ProgramList = new ObservableCollection<ProgramViewModel>(LocalProgramList.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList());
        }

        public event Action SetCNCProgramPath;
        public CNCProgramListViewModel()
        {
            this.ProgramList = new ObservableCollection<ProgramViewModel>();
            this.LocalProgramList = new List<ProgramViewModel>();

            Messenger.Default.Register<SearchInfo>(this, (sInfo) => {
                this.ProgramList.Clear();
                if (string.IsNullOrEmpty(sInfo.Search)) {
                    DataPaging();
                    return;
                }
                foreach (var item in this.LocalProgramList.Where(n => n.Name.Contains(sInfo.Search))) {
                    this.ProgramList.Add(item);
                }
            });
        }

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
                    SetCNCProgramPath?.Invoke();
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

        public ICommand NextPageCommand
        {
            get
            {
                return new RelayCommand(() => {
                    DataPaging(true);
                });
            }
        }
    }
}
