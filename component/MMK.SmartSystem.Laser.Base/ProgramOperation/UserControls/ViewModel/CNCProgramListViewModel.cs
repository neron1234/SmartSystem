using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramListViewModel:ViewModelBase{
        private ObservableCollection<ProgramViewModel> _ProgramList;
        public ObservableCollection<ProgramViewModel> ProgramList{
            get { return _ProgramList; }
            set{
                if (_ProgramList != value){
                    _ProgramList = value;
                    RaisePropertyChanged(() => ProgramList);
                }
            }
        }

        public ICommand UpLoadCommand{
            get{
                return new RelayCommand(() =>{
                    new PopupWindow(new EditProgramControl(), 800, 200, "上传CNC程序").ShowDialog();
                });
            }
        }
    }
}
