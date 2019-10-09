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

    public interface IMachiningKindApplicationService : IAsyncCrudAppService<MachiningKindDto, int, PagedResultRequestDto, CreateMachiningKindDto, UpdateMachiningKindDto>
    {

    }

    public class MachiningKindApplicationService : AsyncCrudAppService<MachiningKind, MachiningKindDto, int, PagedResultRequestDto, CreateMachiningKindDto, UpdateMachiningKindDto>, IMachiningKindApplicationService
    {
        public MachiningKindApplicationService(IRepository<MachiningKind, int> repository) : base(repository)
        {

        }
    }
  
}
