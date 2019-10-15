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
            CreateMap<CuttingData, CuttingDataDto>().ForMember(x => x.GasName, opt => opt.Ignore())
                .ForMember(x => x.MaterialThickness, opt => opt.Ignore())
                .ForMember(x => x.MachiningKindName, opt => opt.Ignore())
                .ForMember(x => x.MaterialName, opt => opt.Ignore())
                .ForMember(x => x.NozzleKindName, opt => opt.Ignore());

            CreateMap<EdgeCuttingData, EdgeCuttingDataDto>().ForMember(x => x.GasName, opt => opt.Ignore())
                .ForMember(x => x.MaterialThickness, opt => opt.Ignore())
                .ForMember(x => x.MachiningKindName, opt => opt.Ignore())
                .ForMember(x => x.MaterialName, opt => opt.Ignore())
                .ForMember(x => x.NozzleKindName, opt => opt.Ignore());

            CreateMap<PiercingData, PiercingDataDto>().ForMember(x => x.GasName, opt => opt.Ignore())
                .ForMember(x => x.MaterialThickness, opt => opt.Ignore())
                .ForMember(x => x.MachiningKindName, opt => opt.Ignore())
                .ForMember(x => x.MaterialName, opt => opt.Ignore())
                .ForMember(x => x.NozzleKindName, opt => opt.Ignore());

            CreateMap<SlopeControlData, SlopeControlDataDto>()
                .ForMember(x => x.MaterialThickness, opt => opt.Ignore())
                .ForMember(x => x.MachiningKindName, opt => opt.Ignore())
                .ForMember(x => x.MaterialName, opt => opt.Ignore())
                .ForMember(x => x.NozzleKindName, opt => opt.Ignore());
        }
    }
}
