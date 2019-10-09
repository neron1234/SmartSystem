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

    public interface IMaterialApplicationService : IAsyncCrudAppService<MaterialDto, int, PagedResultRequestDto, CreateMaterialDto, UpdateMaterialDto>
    {

    }

    public class MaterialApplicationService : AsyncCrudAppService<Material, MaterialDto, int, PagedResultRequestDto, CreateMaterialDto, UpdateMaterialDto>, IMaterialApplicationService
    {
        public MaterialApplicationService(IRepository<Material, int> repository) : base(repository)
        {

        }
    }
   
}
