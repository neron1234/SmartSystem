using Abp.Events.Bus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Common;
using MMK.SmartSystem.Common.EventDatas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMK.SmartSystem.Laser.Base.MachineProcess.UserControls.ViewModel
{
    public class ProcessOptionsViewModel:ViewModelBase
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

        private int _SelectedMaterialTypeId;
        public int SelectedMaterialTypeId
        {
            get { return _SelectedMaterialTypeId; }
            set
            {
                if (_SelectedMaterialTypeId != value)
                {
                    _SelectedMaterialTypeId = value;
                    RaisePropertyChanged(() => SelectedMaterialTypeId);
                }
            }
        }

        private ObservableCollection<MachiningGroupDto> _MaterialThicknessList;
        public ObservableCollection<MachiningGroupDto> MaterialThicknessList
        {
            get { return _MaterialThicknessList; }
            set
            {
                if (_MaterialThicknessList != value)
                {
                    _MaterialThicknessList = value;
                    RaisePropertyChanged(() => MaterialThicknessList);
                }
            }
        }

        public ProcessOptionsViewModel()
        {
            this.MaterialTypeList = new ObservableCollection<MaterialDto>();
            this.MaterialThicknessList = new ObservableCollection<MachiningGroupDto>();
        }
        
        public ICommand MTypeSelectionCommand{
            get{
                return  new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new MachiningGroupInfoEventData() { MaterialId = this.SelectedMaterialId });
                });
            }
        }
    }
}
