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
    public interface INozzleKindApplicationService : IAsyncCrudAppService<NozzleKindDto, int, PagedResultRequestDto, CreateNozzleKindDto, UpdateNozzleKindDto>
    {

    }

    public class NozzleKindApplicationService : AsyncCrudAppService<NozzleKind, NozzleKindDto, int, PagedResultRequestDto, CreateNozzleKindDto, UpdateNozzleKindDto>, INozzleKindApplicationService
    {
        public NozzleKindApplicationService(IRepository<NozzleKind, int> repository) : base(repository)
        {

        }
    }
    
}
