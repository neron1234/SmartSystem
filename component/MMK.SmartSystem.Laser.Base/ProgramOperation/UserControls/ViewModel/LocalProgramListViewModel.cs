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

        public PagingModel<ProgramViewModel> pagingModel;

        public event Action PagePagingEvent;

        public string Path { get; set; }

        public event Action CheckedProgramEvent;

        public LocalProgramListViewModel()
        {
            ProgramList = new ObservableCollection<ProgramViewModel>();
            this.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
            pagingModel = new PagingModel<ProgramViewModel>();
            pagingModel.PagePagingEvent += PagingModel_PagePagingEvent;
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
        public void DataPaging()
        {
            pagingModel.Init(LocalProgramList, (d) => d.CreateTime, PageNumber);
        }

        public string ConnectId { get; set; }

        public event Action<LocalProgramListViewModel, ProgramViewModel> UploadClickEvent;
        public ICommand UpLoadCommand
        {
            get{
                return new RelayCommand(() =>{
                    if (this.SelectedProgramViewModel == null){
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
                    sc.sVM.SearchEvent += SVM_SearchEvent;
                    new PopupWindow(sc, 680, 240, "搜索本地程序").ShowDialog();
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
                    if (this.SelectedProgramViewModel == null){
                        return;
                    }
                    //System.Diagnostics.Process.Start(@"Notepad.exe", System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
                    var ep = new EditProgramStrControl(System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
                    new PopupWindow(ep, 1000, 600, "编辑程序").ShowDialog();
                });
            }
        }

        public ICommand NextPageCommand{
            get{
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
