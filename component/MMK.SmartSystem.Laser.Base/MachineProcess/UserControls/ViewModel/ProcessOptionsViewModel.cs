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
    public class ProcessOptionsViewModel : ViewModelBase
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
                    var node = MaterialTypeList.FirstOrDefault(d => d.MaterialCode == _SelectedMaterialId);
                    MaterialThicknessList.Clear();
                    if (node != null && node.ThicknessNodes.Count > 0)
                    {
                        node.ThicknessNodes.ToList().ForEach(d => MaterialThicknessList.Add(d));
                        if (MaterialThicknessList.Count > 0)
                        {
                            SelectedMaterialGroupId = (int)MaterialThicknessList.First().Id;
                        }
                    }
                    RaisePropertyChanged(() => SelectedMaterialId);
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
                    MaterialGroupChangeEvent?.Invoke(SelectedMaterialGroupId);
                }
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MaterialGroupChangeEvent?.Invoke(SelectedMaterialGroupId);
                });
            }
        }

        public ICommand LastColumnsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MoveGridHeadEvent?.Invoke(false);
                });
            }
        }
        public ICommand NestColumnsCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MoveGridHeadEvent?.Invoke(true);
                });
            }
        }

        public event Action<bool> MoveGridHeadEvent;
        public event Action<int> MaterialGroupChangeEvent;
        public ObservableCollection<MeterialGroupThicknessDto> MaterialTypeList { get; set; }
        public ObservableCollection<ThicknessItem> MaterialThicknessList { get; set; }
        public ProcessOptionsViewModel()
        {
            MaterialTypeList = new ObservableCollection<MeterialGroupThicknessDto>();
            MaterialThicknessList = new ObservableCollection<ThicknessItem>();
        }

        public void InitMaterialData(List<MeterialGroupThicknessDto> list)
        {
            MaterialTypeList.Clear();
            list.ForEach(d => MaterialTypeList.Add(d));
            MaterialThicknessList.Clear();
            if (list.Count > 0)
            {
                SelectedMaterialId = (int)list[0].MaterialCode;
             
            }
        }
    }


}
