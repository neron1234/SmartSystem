using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    [AutoMap(typeof(NozzleKind))]

    public class NozzleKindDto : EntityDto<int>
    {
        public short Code { get; set; }

        public string Name_EN { get; set; }


        public string Name_CN { get; set; }

        public DateTime CreationTime { get; set; }
    }

    [AutoMap(typeof(NozzleKind))]

    public class UpdateNozzleKindDto : EntityDto<int>
    {
        public string Name_EN { get; set; }


        public string Name_CN { get; set; }
    }


    [AutoMap(typeof(NozzleKind))]

    public class CreateNozzleKindDto
    {
        public string Name_EN { get; set; }


        public string Name_CN { get; set; }
    }
}
