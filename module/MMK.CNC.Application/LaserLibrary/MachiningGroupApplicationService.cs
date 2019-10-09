using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using MMK.CNC.Application.LaserLibrary.Dto;
using MMK.CNC.Core.LaserLibrary;
using MMK.SmartSystem.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary
{

    public interface IMachiningGroupApplicationService : IAsyncCrudAppService<MachiningGroupDto, int, MachiningGroupResultRequestDto, CreateMachiningGroupDto, UpdateMachiningGroupDto>
    {

    }

    public class MachiningGroupApplicationService : AsyncCrudAppService<MachiningDataGroup, MachiningGroupDto, int, MachiningGroupResultRequestDto, CreateMachiningGroupDto, UpdateMachiningGroupDto>, IMachiningGroupApplicationService
    {
        IRepository<MachiningDataGroup, int> repository;

        public IRepository<CuttingData, int> CuttingRepository { get; set; }

        public IRepository<EdgeCuttingData, int> EdgeCuttingRepository { get; set; }
        public IRepository<PiercingData, int> PiercingRepository { get; set; }
        public IRepository<SlopeControlData, int> SlopeControlRepository { get; set; }

        public MachiningGroupApplicationService(IRepository<MachiningDataGroup, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override IQueryable<MachiningDataGroup> CreateFilteredQuery(MachiningGroupResultRequestDto input)
        {
            return repository.GetAllIncluding().WhereIf(input.MaterialId != -1, d => d.MaterialId == input.MaterialId);
        }

        public override async Task<MachiningGroupDto> Create(CreateMachiningGroupDto input)
        {
            var entity = AutoMapper.Mapper.Map<MachiningDataGroup>(input);
            var groupId = await repository.InsertAndGetIdAsync(entity);

            for (int i = 0; i < MMKSmartSystemWebCommonConsts.LaserLibraryCuttingDataQuantity; i++)
            {
                await CuttingRepository.InsertAsync(new CuttingData(i) { MachiningDataGroupId = groupId });

            }
            for (int i = 0; i < MMKSmartSystemWebCommonConsts.LaserLibraryEdgeCuttingDataQuantity; i++)
            {
                await EdgeCuttingRepository.InsertAsync(new EdgeCuttingData(i) { MachiningDataGroupId = groupId });

            }
            for (int i = 0; i < MMKSmartSystemWebCommonConsts.LaserLibraryPiercingDataQuantity; i++)
            {
                await PiercingRepository.InsertAsync(new PiercingData(i) { MachiningDataGroupId = groupId });

            }

            for (int i = 0; i < MMKSmartSystemWebCommonConsts.LaserLibrarySlopeControlDataQuantity; i++)
            {
                await SlopeControlRepository.InsertAsync(new SlopeControlData(i) { MachiningDataGroupId = groupId }); 
            }




            return AutoMapper.Mapper.Map<MachiningGroupDto>(entity); 
        }

    }

}
