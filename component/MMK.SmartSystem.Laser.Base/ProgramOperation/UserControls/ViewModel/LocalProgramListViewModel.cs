using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common.EventDatas;
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
    public class LocalProgramListViewModel:ViewModelBase
    {
        private ProgramViewModel _SelectedProgramViewModel;
        public ProgramViewModel SelectedProgramViewModel{
            get { return _SelectedProgramViewModel; }
            set{
                if (_SelectedProgramViewModel != value){
                    _SelectedProgramViewModel = value;
                    RaisePropertyChanged(() => SelectedProgramViewModel);
                }
            }
        }

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

        public string Path { get; set; }

        public LocalProgramListViewModel()
        {
            if (Directory.Exists(@"C:\Users\wjj-yl\Desktop\测试用DXF"))
            {
                this.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
                GetFileName(this.Path);
                Messenger.Default.Send(new ProgramPath(this.Path));
            }
        }
        public void GetFileName(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);
                this.ProgramList = new ObservableCollection<ProgramViewModel>();
                foreach (FileInfo f in root.GetFiles())
                {
                    this.ProgramList.Add(new ProgramViewModel
                    {
                        Name = f.Name,
                        CreateTime = f.CreationTime.ToString(),
                        Size = (f.Length / 1024).ToString() + "KB"
                    });
                }
            }
        }

        public string ConnectId { get; set; }
        public ICommand UpLoadCommand
        {
            get{
                return new RelayCommand(() => {
                    var stream = FileToStream();
                    var fileHash = FileHashHelper.ComputeMD5(System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name));
                    Task.Factory.StartNew(new Action(() => {
                        EventBus.Default.TriggerAsync(new UpLoadProgramClientEventData
                        {
                            FileParameter = new Common.FileParameter(stream, this.SelectedProgramViewModel.Name),
                            ConnectId = ConnectId,
                            FileHashCode = fileHash
                        });
                    }));
                    new PopupWindow(new UpLoadLocalProgramControl(this.Path), 900, 590, "上传本地程序").ShowDialog();
                });
            }
        }

        public Stream FileToStream()
        {
            using (var fileStream = new FileStream(System.IO.Path.Combine(this.Path, this.SelectedProgramViewModel.Name), FileMode.Open, FileAccess.Read, FileShare.Read)){

                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                Stream stream = new MemoryStream(bytes);
                return stream;
            }
        }
    }

    public class ProgramPath
    {
        public string Path { get; set; }
        public ProgramPath(string path)
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
