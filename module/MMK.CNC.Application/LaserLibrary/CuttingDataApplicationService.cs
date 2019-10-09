using Abp.Application.Services;
using Abp.Application.Services.Dto;
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
    public interface ICuttingDataApplicationService : IAsyncCrudAppService<CuttingDataDto, int, PagedResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>
    {

    }
    public class CuttingDataApplicationService : AsyncCrudAppService<CuttingData, CuttingDataDto, int, PagedResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>, ICuttingDataApplicationService
    {
        public IRepository<Gas, int> GasRepository { set; get; }
        IRepository<CuttingData, int> repository;
        public CuttingDataApplicationService(IRepository<CuttingData, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override CuttingDataDto MapToEntityDto(CuttingData entity)
        {
            var resDto = AutoMapper.Mapper.Map<CuttingDataDto>(entity);

            resDto.GasName = GasRepository.FirstOrDefault(d => d.Id == resDto.GasId)?.Name_CN;
            return resDto;
        }
    }
}
