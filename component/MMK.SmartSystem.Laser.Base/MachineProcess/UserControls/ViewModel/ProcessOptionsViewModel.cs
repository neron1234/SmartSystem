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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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

        private ObservableCollection<MeterialGroupThicknessDto> _MaterialTypeList;
        public ObservableCollection<MeterialGroupThicknessDto> MaterialTypeList
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

        private int _SelectedMaterialGroupId;
        public int SelectedMaterialGroupId
        {
            get { return _SelectedMaterialGroupId; }
            set
            {
                if (_SelectedMaterialGroupId != value)
                {
                    _SelectedMaterialGroupId = value;
                    RaisePropertyChanged(() => SelectedMaterialGroupId);
                }
            }
        }

        private ObservableCollection<ThicknessItem> _MaterialThicknessList;
        public ObservableCollection<ThicknessItem> MaterialThicknessList
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
            this.MaterialTypeList = new ObservableCollection<MeterialGroupThicknessDto>();
            this.MaterialThicknessList = new ObservableCollection<ThicknessItem>();
            RegisterMaterial();
        }

        public void RegisterMaterial()
        {
            Task.Factory.StartNew(() => { 
                Messenger.Default.Register<PagedResultDtoOfMeterialGroupThicknessDto>(this, (results) =>{
                    this.MaterialTypeList.Clear();
                    foreach (var item in results.Items){
                        this.MaterialTypeList.Add(item);
                    }
                    if (this.MaterialTypeList.Count > 0){
                        var fistMtList = this.MaterialTypeList.First();
                        this.MaterialThicknessList = new ObservableCollection<ThicknessItem>(fistMtList.ThicknessNodes.ToList());
                        this.SelectedMaterialId = (int)fistMtList.MaterialCode;
                        this.SelectedMaterialGroupId = (int)fistMtList.ThicknessNodes.First().Id;

                        Messenger.Default.Send(this.SelectedMaterialGroupId);
                    }
                });
            });
        }

        public void UnRegisterMaterial()
        {
            Messenger.Default.Unregister<PagedResultDtoOfMeterialGroupThicknessDto>(this);
        }

        public ICommand MTypeSelectionCommand{
            get{
                return new RelayCommand(() => {
                    if (this.MaterialTypeList.Count > 0)
                    {
                        var tNodes = this.MaterialTypeList.FirstOrDefault(n => n.MaterialCode == this.SelectedMaterialId).ThicknessNodes.ToList();
                        this.MaterialThicknessList = new ObservableCollection<ThicknessItem>(tNodes);
                        this.SelectedMaterialGroupId = (int)tNodes.First().Id;
                    }
                });
            }
        }

        public ICommand SearchCommand
        {
            get{
                return new RelayCommand(() =>{
                    Messenger.Default.Send(this.SelectedMaterialGroupId);
                });
            }
        }
        public ICommand LastColumnsCommand
        {
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send("LastColumns");
                });
            }
        }
        public ICommand NestColumnsCommand
        {
            get
            {
                return new RelayCommand(() => {
                    Messenger.Default.Send("NextColumns");
                });
            }
        }
    }
}
