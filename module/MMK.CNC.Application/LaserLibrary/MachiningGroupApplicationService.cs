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

    public interface IMachiningGroupApplicationService : IAsyncCrudAppService<MachiningGroupDto, int, MachiningGroupResultRequestDto, CreateMachiningGroupDto, UpdateMachiningGroupDto>
    {

    }

    public class MachiningGroupApplicationService : AsyncCrudAppService<MachiningDataGroup, MachiningGroupDto, int, MachiningGroupResultRequestDto, CreateMachiningGroupDto, UpdateMachiningGroupDto>, IMachiningGroupApplicationService
    {
        IRepository<MachiningDataGroup, int> repository;
        public MachiningGroupApplicationService(IRepository<MachiningDataGroup, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override IQueryable<MachiningDataGroup> CreateFilteredQuery(MachiningGroupResultRequestDto input)
        {
            return repository.GetAllIncluding().WhereIf(input.MaterialId != -1, d => d.MaterialId == input.MaterialId);
        }

    }

}
