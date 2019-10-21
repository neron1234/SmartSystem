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

        public string Path { get; set; }


        private string _LocalPath;
        public string LocalPath
        {
            get { return _LocalPath; }
            set
            {
                if (_LocalPath != value)
                {
                    _LocalPath = value;
                    RaisePropertyChanged(() => LocalPath);
                }
            }
        }

        public LocalProgramListViewModel()
        {
            this.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";

            GetFileName();
        }

        public void GetFileName()
        {
            if (Directory.Exists(this.Path))
            {
                DirectoryInfo root = new DirectoryInfo(this.Path);
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
                this.LocalPath = this.Path;
                Messenger.Default.Send(new LocalProgramPath(this.Path));
            }
        }

        public string ConnectId { get; set; }
        public ICommand UpLoadCommand
        {
            get{
                return new RelayCommand(() => {
                    if (this.SelectedProgramViewModel == null)
                    {
                        return;
                    }
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
                    new PopupWindow(new UpLoadLocalProgramControl(this.Path, this.ProgramFolderInfo), 900, 590, "上传本地程序").ShowDialog();
                });
            }
        }

        public ICommand LocalPathCommand{
            get{
                return new RelayCommand(() => {
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK){
                        this.Path = folderDialog.SelectedPath.Trim();
                        GetFileName();
                    }
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
}
