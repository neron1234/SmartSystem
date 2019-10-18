using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using MMK.SmartSystem.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class AddMaterialViewModel:ViewModelBase
    {
        private int _SelectedMaterialId;
        public int SelectedMaterialId
        {
            get { return _SelectedMaterialId; }
            set
            {
                if (_SelectedMaterialId != value)
                {
                    _SelectedMaterialId = value;
                    RaisePropertyChanged(() => SelectedMaterialId);
                }
            }
        }

        private ObservableCollection<MaterialDto> _MaterialTypeList;
        public ObservableCollection<MaterialDto> MaterialTypeList
        {
            get { return _MaterialTypeList; }
            set
            {
                if (_MaterialTypeList != value)
                {
                    _MaterialTypeList = value;
                    RaisePropertyChanged(() => MaterialTypeList);
                }
            }
        }

        private string _MaterialThickness;
        public string MaterialThickness
        {
            get { return _MaterialThickness; }
            set
            {
                if (_MaterialThickness != value)
                {
                    _MaterialThickness = value;
                    RaisePropertyChanged(() => MaterialThickness);
                }
            }
        }

        public AddMaterialViewModel()
        {
            
        }

        public string Error { get; set; }
        public ICommand SaveCommand{
            get{
                return new RelayCommand(() => {
                    Messenger.Default.Register<MainSystemNoticeModel>(this, (ms) => {
                        if (ms.Success)
                        {
                            ms.SuccessAction?.Invoke();
                        }
                        else
                        {
                            Error = ms.Error;
                            ms.ErrorAction?.Invoke();
                        }
                    });

                    EventBus.Default.TriggerAsync(new AddMachiningGroupInfoEventData
                    {
                        CreateMachiningGroup = new CreateMachiningGroupDto
                        {
                            MaterialThickness = Convert.ToDouble(this.MaterialThickness),
                            MaterialCode = this.SelectedMaterialId,
                        },
                        SuccessAction = SaveSuccessAction,
                        ErrorAction = SaveErrorAction
                    });
                });
            }
        }

        public ICommand InputCommand{
            get{
                return new RelayCommand<string>((str) =>{
                    var number = 0;
                    if (int.TryParse(str,out number)){
                        MaterialThickness += number;
                    }else{
                        if (str == "." && !MaterialThickness.Contains(".")){
                            MaterialThickness += str;
                        }else{
                            if (MaterialThickness.Length > 0){
                                MaterialThickness = MaterialThickness.Remove(MaterialThickness.Length - 1, 1);
                            }
                        }
                    }
                });
            }
        }

        private void SaveSuccessAction()
        {
            Messenger.Default.Unregister<MainSystemNoticeModel>(this);
            Messenger.Default.Unregister<PagedResultDtoOfMaterialDto>(this);
            Messenger.Default.Send(new PopupMsg("保存成功", true));
        }
        private void SaveErrorAction()
        {
            Messenger.Default.Unregister<MainSystemNoticeModel>(this);
            System.Windows.MessageBox.Show(Error);
        }
    }
}
