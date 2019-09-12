using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.LE.Host.AccountControl.ViewModel
{
    public class UserInfoControlViewModel:ViewModelBase
    {
        private string _Account;
        public string Account
        {
            get { return _Account; }
            set
            {
                if (_Account != value)
                {
                    _Account = value;
                    RaisePropertyChanged(() => Account);
                }
            }
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    RaisePropertyChanged(() => Email);
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


    }
}
