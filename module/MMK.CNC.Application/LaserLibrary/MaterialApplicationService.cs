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

    public interface IMaterialApplicationService : IAsyncCrudAppService<MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>
    {
        PagedResultDto<MeterialGroupThicknessDto> GetMaterialAll(MaterialResultRequestDto input);
    }

    public class MaterialApplicationService : AsyncCrudAppService<Material, MaterialDto, int, MaterialResultRequestDto, CreateMaterialDto, UpdateMaterialDto>, IMaterialApplicationService
    {
        public IRepository<MachiningDataGroup, int> machiningGroupRepository { set; get; }
        public MaterialApplicationService(IRepository<Material, int> repository) : base(repository)
        {
        }

        public PagedResultDto<MeterialGroupThicknessDto> GetMaterialAll(MaterialResultRequestDto input)
        {
            if (input.IsCheckSon)
            {
                var list1 = machiningGroupRepository.GetAllIncluding().Join(Repository.GetAllIncluding(), d => d.MaterialCode, f => f.Code, (p, v) => new MeterialThicknessDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    MaterialCode = p.MaterialCode,
                    MaterialThickness = p.MaterialThickness,
                    Name_CN = v.Name_CN,
                    Name_EN = v.Name_EN

                }).Distinct();
                int total = list1.Count();
                var listRes = list1.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                var groupList = listRes.GroupBy(d => new { d.MaterialCode, d.Name_CN, d.Name_EN }, (key, value) => new MeterialGroupThicknessDto()
                {
                    Code = key.MaterialCode,
                    MaterialCode = key.MaterialCode,
                    Name_CN = key.Name_CN,
                    Name_EN = key.Name_EN,
                    ThicknessNodes = value.Select(d => new ThicknessItem() { Id = d.Id, Thickness = d.MaterialThickness })


                }).ToList();
                return new PagedResultDto<MeterialGroupThicknessDto>(total, groupList);

            }
            else
            {
                int total = Repository.GetAllIncluding().Count();
                var listRes = Repository.GetAllIncluding().Select(d => new MeterialGroupThicknessDto()
                {

                    Name_CN = d.Name_CN,
                    Name_EN = d.Name_EN,
                    Code = d.Code

                }).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                return new PagedResultDto<MeterialGroupThicknessDto>(total, listRes);
            }
        }
    }

}
