using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class SearchViewModel:ViewModelBase
    {
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    RaisePropertyChanged(() => SearchText);
                }
            }
        }

        public ICommand SearchCommand
        {
            get{
                return new RelayCommand(() =>{
                    Messenger.Default.Send(new SearchInfo(this.SearchText));
                });
            }
        }

        public ICommand CancelCommand{
            get{
                return new RelayCommand(() => {
                    Messenger.Default.Send(new PopupMsg("", true));
                });
            }
        }
    }
}
