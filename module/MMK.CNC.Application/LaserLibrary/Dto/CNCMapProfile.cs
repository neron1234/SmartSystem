using AutoMapper;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary.Dto
{
    public class CNCMapProfile : Profile
    {
        public CNCMapProfile()
        {
            CreateMap<CuttingData, CuttingDataDto>().ForMember(x => x.GasName, opt => opt.Ignore());
        }
    }
}
