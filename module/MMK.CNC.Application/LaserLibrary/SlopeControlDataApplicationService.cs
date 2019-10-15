using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
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
        private readonly ICacheManager _cacheManager;
        public BaseDataService DataService { set; get; }
        public SlopeControlDataApplicationService(IRepository<SlopeControlData, int> repository, ICacheManager cacheManager) : base(repository)
        {
            _cacheManager = cacheManager;
        }

        public override Task<PagedResultDto<SlopeControlDataDto>> GetAll(SlopeControlDataResultRequestDto input)
        {
            var list = _cacheManager.GetCache("SlopeControlDataGetAll").Get(input.ToString(), () => GetDataFromDb(input));
            return list;
        }

        private async Task<PagedResultDto<SlopeControlDataDto>> GetDataFromDb(SlopeControlDataResultRequestDto input)
        {
            var list = Repository.GetAllIncluding().WhereIf(input.MachiningDataGroupId != -1, n => n.MachiningDataGroupId == input.MachiningDataGroupId);
            int count = list.Count();
            var listRes = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var resultDto = ObjectMapper.Map<List<SlopeControlDataDto>>(listRes);

            foreach (var item in resultDto)
            {
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
            return new PagedResultDto<SlopeControlDataDto>(count, resultDto);
        }
    }
}
