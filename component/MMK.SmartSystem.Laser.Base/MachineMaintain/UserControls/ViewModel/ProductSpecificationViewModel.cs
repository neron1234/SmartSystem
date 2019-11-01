using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls.ViewModel
{
    public class ProductSpecificationViewModel : ViewModelBase
    {
        public ObservableCollection<ProductSpecificationInfo> SpecificationList { get; set; }
        public ProductSpecificationViewModel(){
            this.SpecificationList = new ObservableCollection<ProductSpecificationInfo>{
                new ProductSpecificationInfo{ Id=0, FileType = FileType.PDF,Name = "LASER- ENGINE系统说明",Specification = "B-64483EN-01", Version="V1.0"},
                new ProductSpecificationInfo{ Id=1, FileType = FileType.PDF,Name = "LASER- ENGINE系统说明",Specification = "B-64483EN-02", Version="V1.2"},
                new ProductSpecificationInfo{ Id=2, FileType = FileType.PDF,Name = "LASER- ENGINE系统说明",Specification = "B-64483EN-03", Version="V1.4"},
                new ProductSpecificationInfo{ Id=3, FileType = FileType.PDF,Name = "LASER- ENGINE系统说明",Specification = "B-64483EN-04", Version="V1.6"},
                new ProductSpecificationInfo{ Id=4, FileType = FileType.PDF,Name = "LASER- ENGINE系统说明",Specification = "B-64483EN-05", Version="V2.0"},
            };
        }
    }

    public class ProductSpecificationInfo:ViewModelBase{
        public int Id { get; set; }

        private string _Name;
        public string Name{
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private FileType _FileType;
        public FileType FileType{
            get { return _FileType; }
            set
            {
                if (_FileType != value)
                {
                    _FileType = value;
                    RaisePropertyChanged(() => FileType);
                }
            }
        }

        private string _Specification;
        public string Specification{
            get { return _Specification; }
            set
            {
                if (_Specification != value)
                {
                    _Specification = value;
                    RaisePropertyChanged(() => Specification);
                }
            }
        }

        private string _Version;
        public string Version{
            get { return _Version; }
            set
            {
                if (_Version != value)
                {
                    _Version = value;
                    RaisePropertyChanged(() => Version);
                }
            }
        }
    }

    public enum FileType{
        PDF,
        Word,
        Excal,
        PPT
    }
}
