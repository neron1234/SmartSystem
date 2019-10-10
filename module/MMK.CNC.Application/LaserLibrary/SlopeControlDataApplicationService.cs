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
    public interface ISlopeControlDataApplicationService : IAsyncCrudAppService<SlopeControlDataDto, int, PagedResultRequestDto, CreateCSlopeControlDataDto,UpdateSlopeControlDataDto>
    {

    }
    public class SlopeControlDataApplicationService : AsyncCrudAppService<SlopeControlData, SlopeControlDataDto, int, PagedResultRequestDto, CreateCSlopeControlDataDto, UpdateSlopeControlDataDto>, ISlopeControlDataApplicationService
    {
        public SlopeControlDataApplicationService(IRepository<SlopeControlData, int> repository) : base(repository)
        {

        }
    }
}
