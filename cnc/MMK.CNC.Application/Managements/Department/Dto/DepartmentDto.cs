using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MMK.CNC.Core.Managements;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.Managements.Dto
{
    [AutoMap(typeof(Department))]
    public class DepartmentDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string Level { get; set; }
        public string Code { get; set; }

        public string Icon { get; set; }

        public int Sort { get; set; }
        public int ParentId { get; set; }
        public DateTime CreationTime { get; set; }
    }
    [AutoMap(typeof(Department))]

    public class CreateDeppartmentDto
    {
        public string Name { get; set; }

        public string Level { get; set; }
        public string Code { get; set; }

        public string Icon { get; set; }

        public int Sort { get; set; }
        public int ParentId { get; set; }
    }

    [AutoMap(typeof(Department))]

    public class UpdateDeppartmentDto : EntityDto<int>
    {
        public string Name { get; set; }

        public string Level { get; set; }
        public string Code { get; set; }

        public string Icon { get; set; }

        public int Sort { get; set; }
        public int ParentId { get; set; }
    }

    public class PagedDepartmentResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public string Code { get; set; }


    }
}
