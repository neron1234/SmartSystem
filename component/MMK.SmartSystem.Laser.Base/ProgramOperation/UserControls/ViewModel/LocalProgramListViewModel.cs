using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class LocalProgramListViewModel : ViewModelBase
    {
        private ProgramViewModel _SelectedProgramViewModel;
        public ProgramViewModel SelectedProgramViewModel
        {
            get { return _SelectedProgramViewModel; }
            set
            {
                if (_SelectedProgramViewModel != value)
                {
                    _SelectedProgramViewModel = value;
                    RaisePropertyChanged(() => SelectedProgramViewModel);
                }
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        public ObservableCollection<ProgramViewModel> LocalProgramList { get; set; } = new ObservableCollection<ProgramViewModel>();

        /// <summary>
        /// 原数据
        /// </summary>
        public ObservableCollection<ProgramViewModel> ProgramList { get; set; }

        public string Path { get; set; }

        public event Action CheckedProgramEvent;

        public LocalProgramListViewModel()
        {
            ProgramList = new ObservableCollection<ProgramViewModel>();
            this.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
            GetFileName();
        }

        public void GetFileName(){
            if (Directory.Exists(this.Path)){
                DirectoryInfo root = new DirectoryInfo(this.Path);
                LocalProgramList = new ObservableCollection<ProgramViewModel>();

                var files = root.GetFiles("*.ng").Union(root.GetFiles("*.txt")).Union(root.GetFiles("*."));

                foreach (FileInfo f in files){
                    var program = new ProgramViewModel{
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

        public int PageNumber = 7;
        public void DataPaging(bool next = false)
        {
            if (LocalProgramList == null)
            {
                return;
            }
            int count = LocalProgramList.Count;
            this.TotalPage = 0;
            if (count % PageNumber == 0)
            {
                this.TotalPage = count / PageNumber;
            }
            else
            {
                this.TotalPage = count / PageNumber + 1;
            }

            if (next && CurrentPage >= 1 && CurrentPage < TotalPage)
            {
                CurrentPage++;
            }
            else
            {
                CurrentPage = 1;
            }
            this.ProgramList.Clear();
            foreach (var item in LocalProgramList.Take(PageNumber * CurrentPage).Skip(PageNumber * (CurrentPage - 1)).ToList())
            {
                this.ProgramList.Add(item);
            }
        }

        public string ConnectId { get; set; }

        public event Action<LocalProgramListViewModel, ProgramViewModel> UploadClickEvent;
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

        public ICommand LocalPathCommand{
            get{
                return new RelayCommand(() =>{
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.Path = folderDialog.SelectedPath.Trim();
                        GetFileName();
                        CheckedProgramEvent.Invoke();
                        Messenger.Default.Send(new Common.ViewModel.NotifiactionModel()
                        {
                            Title = "操作成功",
                            Content = $"切换路径成功!" + DateTime.Now,
                            NotifiactionType = Common.ViewModel.EnumPromptType.Success
                        });
                    }
                });
            }
        }

        public ICommand DeleteFileCommand{
            get{
                return new RelayCommand(() =>{
                    if (File.Exists(System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name))){
                        File.Delete(System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
                        GetFileName();
                    }
                });
            }
        }

        public ICommand SearchCommand{
            get{
                return new RelayCommand(() =>{
                    var sc = new SearchControl();
                    new PopupWindow(sc, 680, 240, "搜索本地程序").ShowDialog();
                    sc.sVM.SearchEvent += SVM_SearchEvent;
                });
            }
        }

        public void SVM_SearchEvent(string str){
            this.ProgramList.Clear();
            if (string.IsNullOrEmpty(str)){
                DataPaging();
                return;
            }
            foreach (var item in this.LocalProgramList.Where(n => n.Name.Contains(str))){
                this.ProgramList.Add(item);
            }
        }

        public ICommand OpenFileCommand{
            get{
                return new RelayCommand(() =>{
                    System.Diagnostics.Process.Start(@"Notepad.exe", System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
                });
            }
        }

        public ICommand NextPageCommand{
            get{
                return new RelayCommand(() =>
                {
                    DataPaging(true);
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
