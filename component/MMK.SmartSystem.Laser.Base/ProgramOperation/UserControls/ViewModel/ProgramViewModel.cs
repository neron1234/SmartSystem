using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class ProgramViewModel:ViewModelBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = "程序名称:" + value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private string _Size;
        public string Size
        {
            get { return _Size; }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }

        private string _CreateTime;
        public string CreateTime
        {
            get { return _CreateTime; }
            set
            {
                if (_CreateTime != value)
                {
                    _CreateTime = value;
                    RaisePropertyChanged(() => CreateTime);
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }
    }
}
