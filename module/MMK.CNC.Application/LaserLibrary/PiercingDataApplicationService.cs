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
    public interface IPiercingDataApplicationService: IAsyncCrudAppService<PiercingDataDto,int, PagedResultRequestDto,CreatePiercingDataDto,UpdatePiercingDataDto>
    {
    }

    public class PiercingDataApplicationService:AsyncCrudAppService<PiercingData,PiercingDataDto,int, PagedResultRequestDto, CreatePiercingDataDto,UpdatePiercingDataDto>, IPiercingDataApplicationService
    {
        public PiercingDataApplicationService(IRepository<PiercingData, int> repository) : base(repository)
        {

        }
    }
}
