using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using MMK.CNC.Application.LaserLibrary.Dto;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary
{
    public interface ISlopeControlDataApplicationService : IAsyncCrudAppService<SlopeControlDataDto, int, SlopeControlDataResultRequestDto, CreateCSlopeControlDataDto,UpdateSlopeControlDataDto>
    {

    }
    public class SlopeControlDataApplicationService : AsyncCrudAppService<SlopeControlData, SlopeControlDataDto, int, SlopeControlDataResultRequestDto, CreateCSlopeControlDataDto, UpdateSlopeControlDataDto>, ISlopeControlDataApplicationService
    {
        public IRepository<Gas, int> GasRepository { set; get; }
        public IRepository<Material, int> MaterialRepository { get; set; }
        public IRepository<MachiningDataGroup, int> MachiningDataGroupRepository { get; set; }
        public IRepository<MachiningKind, int> MachiningKindRepository { get; set; }
        public IRepository<NozzleKind, int> NozzleKindRepository { get; set; }

        IRepository<SlopeControlData, int> repository;
        public SlopeControlDataApplicationService(IRepository<SlopeControlData, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override IQueryable<SlopeControlData> CreateFilteredQuery(SlopeControlDataResultRequestDto input)
        {
            return repository.GetAllIncluding().WhereIf(input.MachiningDataGroupId != -1, n => n.MachiningDataGroupId == input.MachiningDataGroupId);
        }

        protected override SlopeControlDataDto MapToEntityDto(SlopeControlData entity)
        {
            var resDto = ObjectMapper.Map<SlopeControlDataDto>(entity);
            resDto.GasName = GasRepository.FirstOrDefault(d => d.Code == resDto.GasCode)?.Name_CN;


            var mGroup = MachiningDataGroupRepository.FirstOrDefault(d => d.Id == resDto.MachiningDataGroupId);

            resDto.MachiningKindName = MachiningKindRepository.FirstOrDefault(d => d.Code == resDto.MachiningKindCode)?.Name_CN;
            resDto.MaterialName = MaterialRepository.FirstOrDefault(d => d.Code == mGroup.MaterialCode)?.Name_CN;
            resDto.NozzleKindName = NozzleKindRepository.FirstOrDefault(d => d.Code == resDto.NozzleKindCode)?.Name_CN;

            resDto.MaterialThickness = (double)mGroup?.MaterialThickness;
            return resDto;
        }
    }
}
