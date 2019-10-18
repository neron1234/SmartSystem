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

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class UpLoadLocalProgramViewModel:ViewModelBase
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

        private string _LocalProgramPath;
        public string LocalProgramPath
        {
            get { return _LocalProgramPath; }
            set
            {
                if (_LocalProgramPath != value)
                {
                    _LocalProgramPath = value;
                    RaisePropertyChanged(() => LocalProgramPath);
                }
            }
        }

        private int _SelectedNozzleKindCode;
        public int SelectedNozzleKindCode
        {
            get { return _SelectedNozzleKindCode; }
            set
            {
                if (_SelectedNozzleKindCode != value)
                {
                    _SelectedNozzleKindCode = value;
                    RaisePropertyChanged(() => SelectedNozzleKindCode);
                }
            }
        }

        private ObservableCollection<NozzleKindDto> _NozzleKindList;
        public ObservableCollection<NozzleKindDto> NozzleKindList
        {
            get { return _NozzleKindList; }
            set
            {
                if (_NozzleKindList != value)
                {
                    _NozzleKindList = value;
                    RaisePropertyChanged(() => NozzleKindList);
                }
            }
        }

        public UpLoadLocalProgramViewModel(){

        }
    }

    public class ProgramDetailViewModel
    {
        public string Name { get; set; }

        public string FullPath { get; set; }

        public double Size { get; set; }

        public string Material { get; set; }

        public double Thickness { get; set; }

        public string Gas { get; set; }

        public double FocalPosition { get; set; }

        public string NozzleKind { get; set; }

        public double NozzleDiameter { get; set; }

        public string PlateSize { get; set; }

        public string UsedPlateSize { get; set; }

        public double CuttingDistance { get; set; }

        public int PiercingCount { get; set; }

        public double CuttingTime { get; set; }

        public int ThumbnaiType { get; set; }

        public string ThumbnaiInfo { get; set; }
    }

    public class ReadProgramFolderItemViewModel
    {
        public string Name { get; set; }

        public string Folder { get; set; }

        public List<ReadProgramFolderItemViewModel> Nodes { get; set; } = new List<ReadProgramFolderItemViewModel>();
    }
}
