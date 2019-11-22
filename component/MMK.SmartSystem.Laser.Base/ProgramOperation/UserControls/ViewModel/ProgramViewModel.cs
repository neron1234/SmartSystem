using GalaSoft.MvvmLight;
using MMK.SmartSystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.SmartSystem.Laser.Base.ProgramOperation.UserControls.ViewModel
{
    public class ProgramViewModel : ViewModelBase
    {

        public Common.ProgramCommentFromCncDto CommentDto { get; set; } = new ProgramCommentFromCncDto();
        private string _FileHash;
        public string FileHash
        {
            get { return _FileHash; }
            set
            {
                if (_FileHash != value)
                {
                    _FileHash = value;
                    RaisePropertyChanged(() => FileHash);
                }
            }
        }

        private string _FillName;
        public string FillName
        {
            get { return _FillName; }
            set
            {
                if (_FillName != value)
                {
                    _FillName = value;
                    RaisePropertyChanged(() => FillName);
                }
            }
        }

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
            get { return GetSizeString(Convert.ToInt32(_Size)); }
            set
            {
                if (_Size != value)
                {
                    _Size = value;
                    RaisePropertyChanged(() => Size);
                }
            }
        }

        private string _CreateTime;
        public string CreateTime
        {
            get { return _CreateTime; }
            set
            {
                if (_CreateTime != value)
                {
                    _CreateTime = value;
                    RaisePropertyChanged(() => CreateTime);
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    RaisePropertyChanged(() => Description);
                }
            }
        }

        private bool _IsUpLoad;
        public bool IsUpLoad
        {
            get { return _IsUpLoad; }
            set
            {
                if (_IsUpLoad != value)
                {
                    _IsUpLoad = value;
                    if (value)
                    {
                        this.StatusImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Green.png";
                    }
                    else
                    {
                        this.StatusImg = "/MMK.SmartSystem.LE.Host;component/Resources/Images/Status_Blue.png";
                    }
                    RaisePropertyChanged(() => IsUpLoad);
                }
            }
        }

        private string _ProgramName;
        public string ProgramName
        {
            get { return _ProgramName; }
            set
            {
                if (_ProgramName != value)
                {
                    _ProgramName = value;
                    RaisePropertyChanged(() => ProgramName);
                }
            }
        }

        private string _StatusImg;
        public string StatusImg
        {
            get { return _StatusImg; }
            set
            {
                if (_StatusImg != value)
                {
                    _StatusImg = value;
                    RaisePropertyChanged(() => StatusImg);
                }
            }
        }

        public void SetCommentDto(Func<ProgramCommentFromCncDto, bool> filter)
        {
            var obj = ProgramConfigConsts.CurrentProgramCommentFromCncDtos.FirstOrDefault(filter);
            if (obj != null)
            {
                CommentDto = obj;
                this.IsUpLoad = true;
                this.ProgramName = obj.Name;
            }
            else
            {
                this.IsUpLoad = false;
                this.ProgramName = "未上传";
            }
        }
        public void SetProgramLoad(string name)
        {
            this.IsUpLoad = true;
            this.ProgramName = name;
        }
        private string GetSizeString(long size)
        {
            var num = 1024.0;
            if (size < num)
                return size + "B";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f1") + "K";
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f1") + "M";
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f1") + "G";

            return (size / Math.Pow(num, 4)).ToString("f1") + "T";
        }
    }
}
