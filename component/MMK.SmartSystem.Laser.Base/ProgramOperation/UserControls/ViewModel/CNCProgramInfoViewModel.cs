using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MMK.SmartSystem.Laser.Base.ProgramOperation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class CNCProgramInfoViewModel : ViewModelBase
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


        private string _Size;
        public string Size
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
        private string _CuttingDistance;
        public string CuttingDistance
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

        private string _Thickness;
        public string Thickness
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

        private string _PiercingCount;
        public string PiercingCount
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

        private string _PlateSize;
        public string PlateSize
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

        private string _CuttingTime;
        public string CuttingTime
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

        public void InitData(Common.ProgramCommentFromCncDto programComment)
        {
            Name = programComment.Name;
            if (programComment.UsedPlateSize_W != null && programComment.UsedPlateSize_H != null)
            {
                Size = $"{programComment.UsedPlateSize_W?.ToString("0")}mm * {programComment.UsedPlateSize_H?.ToString("0")}mm";

            }
            else
            {
                Size = "信息缺失";
            }
            Material = programComment.Material;
            CuttingDistance = (programComment.CuttingDistance / 1000).ToString("0") + "米";
            Thickness = programComment.Thickness.ToString("0.0") + "mm";
            PiercingCount = programComment.PiercingCount.ToString();

            if (programComment.PlateSize_W != null && programComment.PlateSize_H != null)
            {
                PlateSize = $"{programComment.PlateSize_W?.ToString("0")}mm * {programComment.PlateSize_H?.ToString("0")}mm";

            }
            else
            {
                PlateSize = "信息缺失";
            }
            var dateTime = TimeSpan.FromSeconds(programComment.CuttingTime);
            CuttingTime = "";
            if (dateTime.Hours >= 1)
            {
                CuttingTime = $"{dateTime.Hours.ToString("0")}小时";
            }

            if (dateTime.Minutes >= 1 || dateTime.Hours >= 1)
            {
                CuttingTime += $"{dateTime.Minutes.ToString("0")}分钟";

            }
            CuttingTime += $"{dateTime.Seconds.ToString("0")}秒";

        }


    }
}
