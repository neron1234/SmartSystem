using Abp.Application.Services;
using Abp.Domain.Repositories;
using MMK.CNC.Application.SystemClient.Dto;
using MMK.CNC.Core.SystemClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMK.CNC.Application.SystemClient.OperationLog
{
    public interface IOperationLogAppService : IAsyncCrudAppService<OperationLogDto, int, CreateOperationLogDto>
    {}

    public class OperationLogAppService:AsyncCrudAppService<Core.SystemClient.OperationLog, OperationLogDto,int,CreateOperationLogDto>,IOperationLogAppService
    {
        public OperationLogAppService(IRepository<Core.SystemClient.OperationLog,int> repository):base(repository)
        {

        }
    }
}
