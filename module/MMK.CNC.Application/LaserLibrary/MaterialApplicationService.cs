﻿using Abp.Application.Services;
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

    public interface IMaterialApplicationService : IAsyncCrudAppService<MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>
    {

    }

    public class MaterialApplicationService : AsyncCrudAppService<Material, MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>, IMaterialApplicationService
    {
        public IRepository<MachiningDataGroup, int> machiningGroupRepository { set; get; }
        public MaterialApplicationService(IRepository<Material, int> repository) : base(repository)
        {
        }

        public override async Task<PagedResultDto<MaterialDto>> GetAll(MaterialResultRequestDto input)
        {
            List<Material> materials = new List<Material>();
            if (input.IsCheckSon){
                var list1 = machiningGroupRepository.GetAllIncluding().GroupJoin(Repository.GetAllIncluding(), d => d.MaterialCode, f => f.Code, (p, v) => v).ToList();
                foreach (var item in list1){
                    foreach (var d in item){
                        if (materials.FindIndex(g => g.Id == d.Id) == -1){
                            materials.Add(d);
                        }

                    }
                }
            }else{
                materials = Repository.GetAllIncluding().ToList();
            }
            int total = materials.Count;
            var listRes = materials.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var listDto = ObjectMapper.Map<List<MaterialDto>>(listRes);
            await Task.CompletedTask;
            return new PagedResultDto<MaterialDto>(total, listDto);   
        }
    }

}
