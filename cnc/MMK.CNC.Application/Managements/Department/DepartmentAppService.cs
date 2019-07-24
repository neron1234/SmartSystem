using Abp.Application.Services;
using Abp.Domain.Repositories;
using MMK.CNC.Application.Managements.Dto;
using MMK.CNC.Core.Managements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.Managements
{
    public interface IDepartmentAppService : IAsyncCrudAppService<DepartmentDto, int, PagedDepartmentResultRequestDto, CreateDeppartmentDto, UpdateDeppartmentDto>
    {
    }

    public class DepartmentAppService : AsyncCrudAppService<Department, DepartmentDto, int, PagedDepartmentResultRequestDto, CreateDeppartmentDto, UpdateDeppartmentDto>, IDepartmentAppService
    {
        public DepartmentAppService(IRepository<Department, int> repository) : base(repository)
        {

        }
    }
}
