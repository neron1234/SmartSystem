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
using Abp.Runtime.Caching;

namespace MMK.CNC.Application.LaserLibrary
{
    public interface ICuttingDataApplicationService : IAsyncCrudAppService<CuttingDataDto, int, CuttingDataResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>
    {

    }
    public class CuttingDataApplicationService : AsyncCrudAppService<CuttingData, CuttingDataDto, int, CuttingDataResultRequestDto, CreateCuttingDataDto, UpdateCuttingDataDto>, ICuttingDataApplicationService
    {
        private readonly ICacheManager _cacheManager;

        public BaseDataService DataService { set; get; }

        public CuttingDataApplicationService(IRepository<CuttingData, int> repository, ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }

      
        public override Task<PagedResultDto<CuttingDataDto>> GetAllAsync(CuttingDataResultRequestDto input)
        {
            var list = _cacheManager.GetCache("CuttingDataGetAll").Get(input.ToString(), () => GetDataFromDb(input));
            return list;
        }

        private async Task<PagedResultDto<CuttingDataDto>> GetDataFromDb(CuttingDataResultRequestDto input)
        {
            var list = Repository.GetAllIncluding().WhereIf(input.MachiningDataGroupId != -1, n => n.MachiningDataGroupId == input.MachiningDataGroupId);
            int count = list.Count();
            var listRes = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var resultDto = ObjectMapper.Map<List<CuttingDataDto>>(listRes);

            foreach (var item in resultDto)
            {
                item.GasName = DataService.GetGas(item.GasCode).Name_CN;
                item.MachiningKindName = DataService.GetMachiningKind(item.MachiningKindCode).Name_CN;
                item.NozzleKindName = DataService.GetNozzleKind(item.NozzleKindCode).Name_CN;
                var groupMachining = DataService.GetMachiningData(item.MachiningDataGroupId);
                if (groupMachining.Id != 0)
                {
                    item.MaterialThickness = groupMachining.MaterialThickness;
                    item.MaterialName = DataService.GetMaterial(groupMachining.MaterialCode).Name_CN;

                }
            }
            await Task.CompletedTask;
            return new PagedResultDto<CuttingDataDto>(count, resultDto);
        }
    }
}
