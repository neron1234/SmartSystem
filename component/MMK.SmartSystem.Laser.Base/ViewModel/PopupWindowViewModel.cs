﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MMK.SmartSystem.Laser.Base
{
    public class PopupWindowViewModel:ViewModelBase
    {
        private ContentControl _PopupContent;
        public ContentControl PopupContent
        {
            get { return _PopupContent; }
            set
            {
                if (_PopupContent != value)
                {
                    _PopupContent = value;
                    RaisePropertyChanged(() => PopupContent);
                }
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title != value)
                {
                    _Title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }
    }

    public class PopupMsg
    {
        public string Msg { get; set; }
        public bool IsClose { get; set; }
        public PopupMsg(string msg,bool close)
        {
            Msg = msg;
            IsClose = close;
        }
        public PopupMsg()
        {

        }
    }
}
