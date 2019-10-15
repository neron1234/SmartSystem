using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class LocalProgramListViewModel:ViewModelBase
    {
        private ObservableCollection<ProgramViewModel> _ProgramList;
        public ObservableCollection<ProgramViewModel> ProgramList
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

        public string Path { get; set; }

        public LocalProgramListViewModel()
        {
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
    }
}
