using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel
{
    public class ProgramListViewModel:ViewModelBase
    {
        private string _SelectedName;
        public string SelectedName
        {
            get { return _SelectedName; }
            set
            {
                if (_SelectedName != value)
                {
                    _SelectedName = value;
                    RaisePropertyChanged(() => SelectedName);
                }
            }
        }

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

        private ObservableCollection<ProgramInfo> _ProgramList;
        public ObservableCollection<ProgramInfo> ProgramList
        {
            get { return _ProgramList; }
            set
            {
                if (_ProgramList != value)
                {
                    _ProgramList = value;
                    RaisePropertyChanged(() => ProgramList);
                }
            }
        }
        public ProgramListViewModel()
        {
            this.SelectedName = "程序名称:";

            if (Directory.Exists(@"C:\Users\wjj-yl\Desktop\测试用DXF"))
            {
                this.Path = @"C:\Users\wjj-yl\Desktop\测试用DXF";
                GetFileName(this.Path);
            }
        }

        public void GetFileName(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo root = new DirectoryInfo(path);
                this.ProgramList = new ObservableCollection<ProgramInfo>();
                foreach (FileInfo f in root.GetFiles())
                {
                    this.ProgramList.Add(new ProgramInfo
                    {
                        Name = f.Name,
                        CreateTime = f.CreationTime.ToString(),
                        Size = (f.Length / 1024).ToString() + "KB"
                    });
                }
            }
        }

        public ICommand LoadFileCommand{
            get{
                return new RelayCommand(() => {
                    GetFileName(this.Path);
                });
            }
        }
                
    }
    
    public class ProgramInfo
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string CreateTime { get; set; }
    }
}
