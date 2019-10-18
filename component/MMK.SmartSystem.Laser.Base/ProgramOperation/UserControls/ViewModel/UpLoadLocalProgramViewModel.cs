using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

        private ProgramDetailViewModel _ProgramDetail;
        public ProgramDetailViewModel ProgramDetail
        {
            get { return _ProgramDetail; }
            set
            {
                if (_ProgramDetail != value)
                {
                    _ProgramDetail = value;
                    RaisePropertyChanged(() => ProgramDetail);
                }
            }
        }

        private ReadProgramFolderItemViewModel _ProgramFolders;
        public ReadProgramFolderItemViewModel ProgramFolders
        {
            get { return _ProgramFolders; }
            set
            {
                if (_ProgramFolders != value)
                {
                    _ProgramFolders = value;
                    RaisePropertyChanged(() => ProgramFolders);
                }
            }
        }

        private ReadProgramFolderItemViewModel _SelectedProgramFolders;
        public ReadProgramFolderItemViewModel SelectedProgramFolders
        {
            get { return _SelectedProgramFolders; }
            set{
                if (_SelectedProgramFolders != value){
                    _SelectedProgramFolders = value;
                    RaisePropertyChanged(() => SelectedProgramFolders);
                }
            }
        }

        public ICommand InputCommand{
            get{
                return new RelayCommand<string>((str) => {
                    Messenger.Default.Send(new KeyCode { Code = str });
                });
            }
        }

        public void GetTreeViewData(System.IO.DirectoryInfo dir, ReadProgramFolderItemViewModel node)
        {
            System.IO.DirectoryInfo[] allDs = dir.GetDirectories();
            node.Nodes = new ObservableCollection<ReadProgramFolderItemViewModel>();
            for (int i = 0; i < allDs.Length; i++)
            {
                ReadProgramFolderItemViewModel child = new ReadProgramFolderItemViewModel();
                child.Name = allDs[i].Name;
                child.Folder = allDs[i].FullName;
                node.Nodes.Add(child);
                GetTreeViewData(allDs[i], child);
            }
        }

        public UpLoadLocalProgramViewModel(){
            ProgramFolders = new ReadProgramFolderItemViewModel();
            SelectedProgramFolders = new ReadProgramFolderItemViewModel();

            GetTreeViewData(new System.IO.DirectoryInfo(@"C:\Users\wjj-yl\Desktop\测试用DXF"), ProgramFolders);

            if (ProgramFolders.Nodes.Count > 0)
            {
                SelectedProgramFolders = ProgramFolders.Nodes[0];
            }
        }
    }

    public class ProgramDetailViewModel : ViewModelBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private string _FullPath;
        public string FullPath
        {
            get { return _FullPath; }
            set
            {
                if (_FullPath != value)
                {
                    _FullPath = value;
                    RaisePropertyChanged(() => FullPath);
                }
            }
        }

        private double _Size;
        public double Size
        {
            get { return _Size; }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }

        private string _Material;
        public string Material
        {
            get { return _Material; }
            set
            {
                if (_Material != value)
                {
                    _Material = value;
                    RaisePropertyChanged(() => Material);
                }
            }
        }

        private double _Thickness;
        public double Thickness
        {
            get { return _Thickness; }
            set
            {
                if (_Thickness != value)
                {
                    _Thickness = value;
                    RaisePropertyChanged(() => Thickness);
                }
            }
        }

        private string _Gas;
        public string Gas
        {
            get { return _Gas; }
            set
            {
                if (_Gas != value)
                {
                    _Gas = value;
                    RaisePropertyChanged(() => Gas);
                }
            }
        }

        private double _FocalPosition;
        public double FocalPosition
        {
            get { return _FocalPosition; }
            set
            {
                if (_FocalPosition != value)
                {
                    _FocalPosition = value;
                    RaisePropertyChanged(() => FocalPosition);
                }
            }
        }

        private string _NozzleKind;
        public string NozzleKind
        {
            get { return _NozzleKind; }
            set
            {
                if (_NozzleKind != value)
                {
                    _NozzleKind = value;
                    RaisePropertyChanged(() => NozzleKind);
                }
            }
        }

        private double _NozzleDiameter;
        public double NozzleDiameter
        {
            get { return _NozzleDiameter; }
            set
            {
                if (_NozzleDiameter != value)
                {
                    _NozzleDiameter = value;
                    RaisePropertyChanged(() => NozzleDiameter);
                }
            }
        }

        private string  _PlateSize;
        public string  PlateSize
        {
            get { return _PlateSize; }
            set
            {
                if (_PlateSize != value)
                {
                    _PlateSize = value;
                    RaisePropertyChanged(() => PlateSize);
                }
            }
        }

        private string _UsedPlateSize;
        public string UsedPlateSize
        {
            get { return _UsedPlateSize; }
            set
            {
                if (_UsedPlateSize != value)
                {
                    _UsedPlateSize = value;
                    RaisePropertyChanged(() => UsedPlateSize);
                }
            }
        }

        private double _CuttingDistance;
        public double CuttingDistance
        {
            get { return _CuttingDistance; }
            set
            {
                if (_CuttingDistance != value)
                {
                    _CuttingDistance = value;
                    RaisePropertyChanged(() => CuttingDistance);
                }
            }
        }

        private int _PiercingCount;
        public int PiercingCount
        {
            get { return _PiercingCount; }
            set
            {
                if (_PiercingCount != value)
                {
                    _PiercingCount = value;
                    RaisePropertyChanged(() => PiercingCount);
                }
            }
        }

        private double _CuttingTime;
        public double CuttingTime
        {
            get { return _CuttingTime; }
            set
            {
                if (_CuttingTime != value)
                {
                    _CuttingTime = value;
                    RaisePropertyChanged(() => CuttingTime);
                }
            }
        }

        private int _ThumbnaiType;
        public int ThumbnaiType
        {
            get { return _ThumbnaiType; }
            set
            {
                if (_ThumbnaiType != value)
                {
                    _ThumbnaiType = value;
                    RaisePropertyChanged(() => ThumbnaiType);
                }
            }
        }

        private string _ThumbnaiInfo;
        public string ThumbnaiInfo
        {
            get { return _ThumbnaiInfo; }
            set
            {
                if (_ThumbnaiInfo != value)
                {
                    _ThumbnaiInfo = value;
                    RaisePropertyChanged(() => ThumbnaiInfo);
                }
            }
        }
    }

    public class ReadProgramFolderItemViewModel:ViewModelBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private string _Folder;
        public string Folder
        {
            get { return _Folder; }
            set
            {
                if (_Folder != value)
                {
                    _Folder = value;
                    RaisePropertyChanged(() => Folder);
                }
            }
        }

        private ObservableCollection<ReadProgramFolderItemViewModel> _Nodes;
        public ObservableCollection<ReadProgramFolderItemViewModel> Nodes
        {
            get { return _Nodes; }
            set
            {
                if (_Nodes != value)
                {
                    _Nodes = value;
                    RaisePropertyChanged(() => Nodes);
                }
            }
        }
    }

    public class KeyCode
    {
        public string Code { get; set; }
    }
}
