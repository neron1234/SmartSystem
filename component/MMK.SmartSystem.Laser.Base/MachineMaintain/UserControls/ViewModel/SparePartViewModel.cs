using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.MachineMaintain.UserControls.ViewModel
{
    public class SparePartViewModel:ViewModelBase
    {
        public ObservableCollection<SparePartInfo> SparePartList { get; set; }
        public SparePartViewModel()
        {
            this.SparePartList = new ObservableCollection<SparePartInfo>
            {
                new SparePartInfo{ Id=0,Name ="风扇",Specification ="CNC", UseLocation = "B",About="割嘴A",ImgUrl = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Part1Icon.png" },
                new SparePartInfo{ Id=1,Name ="风扇",Specification ="CNC", UseLocation = "B",About="割嘴B",ImgUrl = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Part1Icon.png" },
                new SparePartInfo{ Id=2,Name ="风扇",Specification ="CNC", UseLocation = "B",About="割嘴C",ImgUrl = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Part1Icon.png" },
                new SparePartInfo{ Id=3,Name ="电机",Specification ="CNC", UseLocation = "A",About="割嘴A",ImgUrl = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Part1Icon.png" },
                new SparePartInfo{ Id=4,Name ="电机",Specification ="CNC", UseLocation = "A",About="割嘴B",ImgUrl = "/MMK.SmartSystem.Laser.Base;component/Resources/Images/Part1Icon.png" }
            };
        }
    }

    public class SparePartInfo : ViewModelBase
    {
        public int Id { get; set; }

        private string _UseLocation;
        public string UseLocation
        {
            get { return _UseLocation; }
            set
            {
                if (_UseLocation != value)
                {
                    _UseLocation = value;
                    RaisePropertyChanged(() => UseLocation);
                }
            }
        }

        private string  _ImgUrl;
        public string ImgUrl
        {
            get { return _ImgUrl; }
            set
            {
                if (_ImgUrl != value)
                {
                    _ImgUrl = value;
                    RaisePropertyChanged(() => ImgUrl);
                }
            }
        }

        private string _Name;
        public string Name
        {
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

        private string _Specification;
        public string Specification
        {
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

        private string _About;
        public string About
        {
            get { return _About; }
            set
            {
                if (_About != value)
                {
                    _About = value;
                    RaisePropertyChanged(() => About);
                }
            }
        }
    }
}
