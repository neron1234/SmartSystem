using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ViewModel
{
    public class SwitchBtnViewModel : ViewModelBase
    {
        public SwitchBtnViewModel()
        {
            this.HorizontalAlignment = "Right";
            this.Text = "OFF";
            this.FontColor = "#000000";
        }

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    RaisePropertyChanged(() => Text);
                }
            }
        }

        private string _HorizontalAlignment;
        public string HorizontalAlignment
        {
            get { return _HorizontalAlignment; }
            set
            {
                if (_HorizontalAlignment != value)
                {
                    _HorizontalAlignment = value;
                    RaisePropertyChanged(() => HorizontalAlignment);
                }
            }
        }

        private string _FontColor;
        public string FontColor
        {
            get { return _FontColor; }
            set
            {
                if (_FontColor != value)
                {
                    _FontColor = value;
                    RaisePropertyChanged(() => FontColor);
                }
            }
        }

        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(() => {
                    if (this.HorizontalAlignment == "Left")
                    {
                        this.HorizontalAlignment = "Right";
                        this.Text = "OFF";
                        this.FontColor = "#000000";
                    }
                    else
                    {
                        this.HorizontalAlignment = "Left";
                        this.Text = "ON";
                        this.FontColor = "#fdcd00";
                    }
                });
            }
        }
    }
}
