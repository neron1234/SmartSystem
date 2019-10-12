using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
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

    public interface IMaterialApplicationService : IAsyncCrudAppService<MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>
    {

    }

    public class MaterialApplicationService : AsyncCrudAppService<Material, MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>, IMaterialApplicationService
    {
        IRepository<MachiningDataGroup, int> machiningGroupRepository;
        IRepository<Material, int> repository;
        public MaterialApplicationService(IRepository<Material, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 创建分页查询的筛选条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Material> CreateFilteredQuery(MaterialResultRequestDto input)
        {
            //var mIds = machiningGroupRepository.GetAll().Select(n => n.MaterialId);
            //return repository.GetAllIncluding().WhereIf(input.IsCheckSon,n => mIds.Contains(n.Id)).ToList().AsQueryable();
            return repository.GetAll();
        }
    }
   
}
