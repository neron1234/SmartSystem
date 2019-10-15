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
    public class CNCProgramListViewModel:ViewModelBase
    {
        public ICommand UpLoadCommand
        {
            get{
                return new RelayCommand(() =>{
                    new PopupWindow(new EditProgramControl(), 800, 200, "上传CNC程序").ShowDialog();
                });
            }
        }
    }
}
