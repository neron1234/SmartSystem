using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using MMK.SmartSystem.Laser.Base.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class LocalProgramListViewModel : ViewModelBase
    {
        private const int PageNumber = 7;

        private ProgramViewModel _SelectedProgramViewModel;
        public ProgramViewModel SelectedProgramViewModel
        {
            get { return _SelectedProgramViewModel; }
            set
            {
                _SelectedProgramViewModel = value;

                if (_SelectedProgramViewModel == null)
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// 本地数据副本
        /// </summary>
        public List<ProgramViewModel> LocalProgramList { get; set; } = new List<ProgramViewModel>();

        /// <summary>
        /// 原数据
        /// </summary>
        public ObservableCollection<ProgramViewModel> ProgramList { get; set; }

        public PagingModel<ProgramViewModel> pagingModel;

        public event Action PagePagingEvent;


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

        public event Action CheckedProgramEvent;
        public event Action EditProgramEvent;

        public LocalProgramListViewModel()
        {
            ProgramList = new ObservableCollection<ProgramViewModel>();
            this.Path = ProgramConfigConsts.LocalPath;
            pagingModel = new PagingModel<ProgramViewModel>();
            pagingModel.PagePagingEvent += PagingModel_PagePagingEvent;
        }

        public void Init()
        {
            GetFileName();
        }
        private void PagingModel_PagePagingEvent(IEnumerable<ProgramViewModel> arg1, int arg2, int arg3)
        {
            this.ProgramList.Clear();
            arg1.ToList().ForEach(d => ProgramList.Add(d));

            CurrentPage = arg2;
            TotalPage = arg3;
            PagePagingEvent?.Invoke();
        }

        private void GetFileName()
        {
            if (Directory.Exists(this.Path))
            {
                DirectoryInfo root = new DirectoryInfo(this.Path);
                LocalProgramList.Clear();

                var files = root.GetFiles("*.ng").Union(root.GetFiles("*.txt")).Union(root.GetFiles("*."));

                foreach (FileInfo f in files)
                {
                    var program = new ProgramViewModel
                    {
                        FileHash = FileHashHelper.ComputeMD5(f.FullName),
                        Name = f.Name,
                        FillName = f.FullName,
                        CreateTime = f.CreationTime.ToString("MM-dd HH:mm"),
                        Size = GetFileSize(f.Length),
                        StatusImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Blue.png"
                    };
                    this.LocalProgramList.Add(program);
                }
                DataPaging();
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
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    RaisePropertyChanged(() => IsEnabled);
                }
            }
        }
        public void DataPaging()
        {
            pagingModel.Init(LocalProgramList, (d) => d.CreateTime, 1, PageNumber);
        }
        public void RefreshPage()
        {
            pagingModel.Init(LocalProgramList, (d) => d.CreateTime, CurrentPage, PageNumber);

        }
        public string ConnectId { get; set; }

        public event Action<LocalProgramListViewModel, ProgramViewModel> UploadClickEvent;

        public event Action<string, ProgramViewModel> DeleteProgramEvent;
        public ICommand UpLoadCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (this.SelectedProgramViewModel == null)
                    {
                        return;
                    }
                    UploadClickEvent?.Invoke(this, SelectedProgramViewModel);
                });
            }
        }

        public ICommand LocalPathCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedProgramViewModel = null;
                        ProgramList.Clear();
                        CurrentPage = 0;
                        TotalPage = 0;
                        LocalProgramList.Clear();
                        this.Path = folderDialog.SelectedPath.Trim();
                        ProgramConfigConsts.LocalPath = Path;
                        GetFileName();
                        CheckedProgramEvent.Invoke();
                        Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                        {
                            Title = "操作成功",
                            Content = $"切换路径成功!" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"),
                            NotifiactionType = Common.ViewModel.EnumPromptType.Success
                        });
                    }
                });
            }
        }

        public ICommand DeleteFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DeleteProgramEvent?.Invoke(Path, SelectedProgramViewModel);

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
                    sc.sVM.SearchEvent += SVM_SearchEvent;
                    new PopupWindow(sc, 680, 240, "搜索本地程序").ShowDialog();
                });
            }
        }

        public void SVM_SearchEvent(string str)
        {
            this.ProgramList.Clear();
            if (string.IsNullOrEmpty(str))
            {
                DataPaging();
                return;
            }
            foreach (var item in this.LocalProgramList.Where(n => n.Name.Contains(str)))
            {
                this.ProgramList.Add(item);
            }
        }

        public ICommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EditProgramEvent.Invoke();
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


        public static string GetFileSize(long size)
        {
            var num = 1024.0;
            if (size < num)
                return size + "B";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f1") + "K";
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f1") + "M";
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f1") + "G";

            return (size / Math.Pow(num, 4)).ToString("f1") + "T";
        }
    }
}
