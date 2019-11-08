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
            this.ProgramList = new ObservableCollection<ProgramViewModel>(LocalProgramList.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList());
        }

        public CNCProgramListViewModel()
        {
            LocalProgramList = new List<ProgramViewModel>();
            Messenger.Default.Register<CNCProgramPath>(this, (cncPath) => {
                this.CNCPath = cncPath.Path;
            });
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
            Messenger.Default.Register<ReadProgramFolderItemViewModel>(this, (pfs) => {
                this.ProgramFolderList = pfs;
            });
            Messenger.Default.Register<List<ProgramViewModel>>(this, (pvList) => {
                this.LocalProgramList = pvList;
                this.DataPaging();
            });

            this.ProgramList = new ObservableCollection<ProgramViewModel> { 
                new ProgramViewModel { Name = "0005", CreateTime = "2019-10-26 14:28:02", Size = "865KB" },
                new ProgramViewModel { Name = "0004", CreateTime = "2019-10-26 14:32:16", Size = "610KB" },
                new ProgramViewModel { Name = "0003", CreateTime = "2019-10-26 14:50:25", Size = "1298KB" },
                new ProgramViewModel { Name = "0002", CreateTime = "2019-10-27 10:05:41", Size = "511KB" },
                new ProgramViewModel { Name = "0001", CreateTime = "2019-10-27 18:28:54", Size = "715KB" },
                new ProgramViewModel { Name = "0011", CreateTime = "2019-10-28 08:28:34", Size = "228KB" },
                new ProgramViewModel { Name = "0012", CreateTime = "2019-10-28 20:12:21", Size = "1101KB" },
                new ProgramViewModel { Name = "0021", CreateTime = "2019-10-29 10:05:10", Size = "489KB" }
            };
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
