using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using MMK.CNC.Application.LaserLibrary.Dto;
using MMK.CNC.Core.LaserLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMK.CNC.Application.LaserLibrary
{
    public interface ICuttingDataApplicationService : IAsyncCrudAppService<CuttingDataDto, int, CuttingDataResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>
    {

    }
    public class CuttingDataApplicationService : AsyncCrudAppService<CuttingData, CuttingDataDto, int, CuttingDataResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>, ICuttingDataApplicationService
    {
        public IRepository<Gas, int> GasRepository { set; get; }
        public IRepository<Material, int> MaterialRepository { get; set; }
        public IRepository<MachiningDataGroup, int> MachiningDataGroupRepository { get; set; }
        public IRepository<MachiningKind, int> MachiningKindRepository { get; set; }
        public IRepository<NozzleKind, int> NozzleKindRepository { get; set; }

        IRepository<CuttingData, int> repository;
        public CuttingDataApplicationService(IRepository<CuttingData, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        //protected override IQueryable<CuttingData> CreateFilteredQuery(CuttingDataResultRequestDto input)
        //{
        //    return repository.GetAllIncluding().WhereIf(input.MachiningDataGroupId != -1, n => n.MachiningDataGroupId == input.MachiningDataGroupId);
        //}

        //protected override CuttingDataDto MapToEntityDto(CuttingData entity)
        //{
        //    var resDto = ObjectMapper.Map<CuttingDataDto>(entity);

        //    resDto.GasName = GasRepository.FirstOrDefault(d => d.Code == resDto.GasCode)?.Name_CN;

        //    var mGroup = MachiningDataGroupRepository.FirstOrDefault(d => d.Id == resDto.MachiningDataGroupId);

        //    resDto.MachiningKindName = MachiningKindRepository.FirstOrDefault(d => d.Code == resDto.MachiningKindCode)?.Name_CN;

        //    resDto.MaterialName = MaterialRepository.FirstOrDefault(d => d.Code == mGroup.MaterialCode)?.Name_CN;

        //    resDto.NozzleKindName = NozzleKindRepository.FirstOrDefault(d => d.Code == resDto.NozzleKindCode)?.Name_CN;

        //    resDto.MaterialThickness = (double)mGroup?.MaterialThickness;

        //    return resDto;
        //}

        public override async Task<PagedResultDto<CuttingDataDto>> GetAll(CuttingDataResultRequestDto input)
        {
            var list = repository.GetAllIncluding().WhereIf(input.MachiningDataGroupId != -1, n => n.MachiningDataGroupId == input.MachiningDataGroupId);
            int count = list.Count();
            var listRes = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var resultDto = ObjectMapper.Map<List<CuttingDataDto>>(listRes);
            var gasNodes = GasRepository.GetAllIncluding().ToDictionary(d => d.Code);

            var machining = MachiningDataGroupRepository.GetAllIncluding().ToDictionary(d => d.Id);

            var machiningKind = MachiningKindRepository.GetAllIncluding().ToDictionary(d => d.Code);

            var material = MaterialRepository.GetAllIncluding().ToDictionary(d => d.Code);

            var nozzle = NozzleKindRepository.GetAllIncluding().ToDictionary(d => d.Code);

            foreach (var item in resultDto)
            {
                if (gasNodes.ContainsKey(item.GasCode))
                {
                    item.GasName = gasNodes[item.GasCode].Name_CN;
                }

                if (machining.ContainsKey(item.MachiningDataGroupId))
                {
                    var ma = machining[item.MachiningDataGroupId];
                    item.MaterialThickness = machining[item.MachiningDataGroupId].MaterialThickness;
                    if (material.ContainsKey(ma.MaterialCode))
                    {
                        item.MaterialName = material[ma.MaterialCode].Name_CN;
                    }
                }
                if (machiningKind.ContainsKey(item.MachiningKindCode))
                {
                    item.MachiningKindName = machiningKind[item.NozzleKindCode].Name_CN;
                }

                if (nozzle.ContainsKey(item.NozzleKindCode))
                {
                    item.NozzleKindName = nozzle[item.NozzleKindCode].Name_CN;
                }
            }

            await Task.CompletedTask;
            return new PagedResultDto<CuttingDataDto>(count, resultDto);
        }
    }
}
