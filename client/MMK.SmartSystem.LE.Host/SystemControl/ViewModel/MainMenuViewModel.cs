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
        public string Id { get; set; }
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
        public int Sort { get; set; }
        public string Page { get; set; }

        public string PageKey { get; set; }
        public bool Auth { get; set; }

        public bool WebPage { get; set; }

        public string Url { get; set; }
        public string Permission { get; set; }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    RaisePropertyChanged(() => Icon);
                }
            }
        }
        private bool IsActive = false;
        public string BackColor { get; set; }

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
                    if (IsActive)
                    {
                        return;
                    }
                    if (s.WebPage)
                    {
                        Messenger.Default.Send(new PageChangeModel() { Url = s.Url, Page = PageEnum.WebPage });
                        return;
                    }
                    Messenger.Default.Send(new PageChangeModel() { FullType = s.PageType, Page = PageEnum.WPFPage });
                    MenuClickEvent?.Invoke(this);
                });
            }
        }


        public event Action<MainMenuViewModel> MenuClickEvent;
        public void MenuActive()
        {
            Icon = _icon + "-active";
            IsActive = true;
        }
        public void MenuClearActive()
        {
            IsActive = false;
            Icon = _icon.Replace("-active", "");

        }
        public MainMenuViewModel()
        {
            _Show = Visibility.Collapsed;
        }
    }
}
