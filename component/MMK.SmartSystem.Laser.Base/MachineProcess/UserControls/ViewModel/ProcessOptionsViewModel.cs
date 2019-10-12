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

            Messenger.Default.Register<PagedResultDtoOfMachiningGroupDto>(this, (result) =>
            {
                this.MaterialThicknessList.Clear();
                foreach (var item in result.Items)
                {
                    this.MaterialThicknessList.Add(item);
                }
                if (this.MaterialThicknessList.Count > 0)
                {
                    this.SelectedMaterialTypeId = (int)this.MaterialThicknessList.First()?.Id;
                }
            });
            RegisterMaterial();
        }

        private void RegisterMaterial()
        {
            Messenger.Default.Register<List<MaterialDto>>(this, (results) =>
            {
                this.MaterialTypeList.Clear();
                foreach (var item in results)
                {
                    this.MaterialTypeList.Add(item);
                }
                if (this.MaterialTypeList.Count > 0)
                {
                    this.SelectedMaterialId = (int)this.MaterialTypeList.First()?.Id;
                    this.MTypeSelectionCommand.Execute("");
                }
            });
            EventBus.Default.TriggerAsync(new MaterialInfoEventData { IsAll = false });
        }

        public ICommand MTypeSelectionCommand{
            get{
                return  new RelayCommand(() => {
                    EventBus.Default.TriggerAsync(new MachiningGroupInfoEventData() { MaterialId = this.SelectedMaterialId });
                });
            }
        }

        public ICommand AddCommand{
            get{
                return new RelayCommand(() =>{
                    Messenger.Default.Unregister<List<MaterialDto>>(this);

                    new PopupWindow(new AddMaterialControl(), 650, 260, "添加工艺材料").ShowDialog();

                    RegisterMaterial();
                });
            }
        }
        public ICommand Searchommand
        {
            get{
                return new RelayCommand(() =>{
                    System.Windows.MessageBox.Show(this.SelectedMaterialId + "|" + this.SelectedMaterialTypeId);
                });
            }
        }
    }
}
