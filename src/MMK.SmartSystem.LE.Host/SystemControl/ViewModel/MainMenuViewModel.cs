using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MMK.SmartSystem.LE.Host.SystemControl.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {

        public bool IsLoad { get; set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        public string Page { get; set; }

        public string PageKey { get; set; }
        public bool Auth { get; set; }

        public string Permission { get; set; }


        private Visibility _Show;
        public Visibility Show
        {
            get { return _Show; }
            set
            {
                if (_Show != value)
                {
                    _Show = value;
                    RaisePropertyChanged(() => Show);
                }
            }
        }
        public Type PageType { get; set; }

        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<MainMenuViewModel>((s) =>
                {
                    Messenger.Default.Send(s.PageType);
                });
            }
        }

        public MainMenuViewModel()
        {
            _Show = Visibility.Collapsed;
        }
    }
}
