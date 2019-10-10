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
    public interface IEdgeCuttingDataApplicationService: IAsyncCrudAppService<EdgeCuttingDataDto,int, PagedResultRequestDto,CreateEdgeCuttingDataDto,UpdateEdgeCuttingDataDto>
    {
    }
    public class EdgeCuttingDataApplicationService: AsyncCrudAppService<EdgeCuttingData, EdgeCuttingDataDto,int, PagedResultRequestDto,CreateEdgeCuttingDataDto,UpdateEdgeCuttingDataDto>,IEdgeCuttingDataApplicationService
    {
        public EdgeCuttingDataApplicationService(IRepository<EdgeCuttingData, int> repository) : base(repository)
        {
        
        }
    }
}
