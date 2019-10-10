using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MMK.SmartSystem.Common;
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
            this.MaterialTypeList = new ObservableCollection<MaterialDto>();
        }

        public ICommand InputCommand
        {
            get
            {
                return new RelayCommand<string>((str) =>
                {
                    var number = 0;
                    if (int.TryParse(str,out number))
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
                            MaterialThickness = MaterialThickness.Remove(MaterialThickness.Length - 2, 1);
                        }
                    }
                });
            }
        }
    }
}
