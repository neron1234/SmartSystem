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
    public interface IGasApplicationService : IAsyncCrudAppService<GasDto, int, PagedResultRequestDto, CreateGasDto, UpdateGasDto>
    {

    }

    public class GasApplicationService : AsyncCrudAppService<Gas, GasDto, int, PagedResultRequestDto, CreateGasDto, UpdateGasDto>, IGasApplicationService
    {
        public GasApplicationService(IRepository<Gas, int> repository) : base(repository)
        {

        }
    }
}
