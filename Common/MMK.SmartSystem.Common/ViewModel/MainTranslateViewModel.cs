using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Common.ViewModel
{
    public class MainTranslateViewModel : ViewModelBase
    {
        public SystemTranslate Translate { get; set; }

        private string authkey = "";

        private bool _EditAuth;
        public bool EditAuth
        {
            get { return _EditAuth; }
            set
            {
                if (_EditAuth != value)
                {
                    _EditAuth = value;
                    RaisePropertyChanged(() => EditAuth);
                }
            }
        }
        public void RefreshAuth()
        {
            EditAuth = authkey.IsGrantedPermission();

        }
        public MainTranslateViewModel()
        {
            Translate = SmartSystemCommonConsts.SystemTranslateModel;

        }

        public MainTranslateViewModel(string key)
        {
            Translate = SmartSystemCommonConsts.SystemTranslateModel;
            this.authkey = key + ".Edit";
            EditAuth = authkey.IsGrantedPermission();

        }
    }
}
