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
    public class AddMaterialViewModel : ViewModelBase
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

        public ObservableCollection<MeterialGroupThicknessDto> MaterialTypeList { get; set; } = new ObservableCollection<MeterialGroupThicknessDto>();

        private string _MaterialThickness;
        public string MaterialThickness
        {
            get { return _MaterialThickness; }
            set
            {
                if (_MaterialThickness != value)
                {
                    _MaterialThickness = value;
                    CanSave = _MaterialThickness.Length > 0;
                    RaisePropertyChanged(() => MaterialThickness);
                }
            }
        }
        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                if (_canSave != value)
                {
                    _canSave = value;
                    RaisePropertyChanged(() => CanSave);
                }
            }
        }

        private string _saveText;
        public string SaveText
        {
            get { return _saveText; }
            set
            {
                if (_saveText != value)
                {
                    _saveText = value;
                    RaisePropertyChanged(() => SaveText);
                }
            }
        }
        public AddMaterialViewModel()
        {
            SaveText = "保存";
        }

        public string Error { get; set; }

        public event Action<CreateMachiningGroupDto> SaveMachiningEvent;
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveText = "保存中...";
                    CanSave = false;
                    SaveMachiningEvent?.Invoke(new CreateMachiningGroupDto
                    {
                        MaterialThickness = Convert.ToDouble(this.MaterialThickness),
                        MaterialCode = this.SelectedMaterialId,
                    }
                );
                });

            }
        }

        public ICommand InputCommand
        {
            get
            {
                return new RelayCommand<string>((str) =>
                {
                    var number = 0;
                    if (int.TryParse(str, out number))
                    {
                        MaterialThickness += number;
                    }
                    else
                    {
                        if (str == "." && !MaterialThickness.Contains("."))
                        {
                            MaterialThickness += str;
                        }
                        else
                        {
                            if (MaterialThickness.Length > 0)
                            {
                                MaterialThickness = MaterialThickness.Remove(MaterialThickness.Length - 1, 1);
                            }
                        }
                    }
                });
            }
        }


    }
}
