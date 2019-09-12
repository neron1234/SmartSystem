using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using MMK.CNC.Application.SystemClient.Dto;
using MMK.CNC.Core.SystemClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMK.CNC.Application.SystemClient.OperationLog
{

    public interface IOperationLogAppService : IAsyncCrudAppService<OperationLogDto, int, PagedOperationResultRequestDto, CreateOperationLogDto, UpdateOperationLogDto>
    { }

    public class OperationLogAppService : AsyncCrudAppService<Core.SystemClient.OperationLog, OperationLogDto, int, PagedOperationResultRequestDto, CreateOperationLogDto, UpdateOperationLogDto>, IOperationLogAppService
    {
        IRepository<Core.SystemClient.OperationLog, int> repository;
        public OperationLogAppService(IRepository<Core.SystemClient.OperationLog, int> repository) : base(repository)
        {
            this.repository = repository;
        }

        protected override IQueryable<Core.SystemClient.OperationLog> CreateFilteredQuery(PagedOperationResultRequestDto input)
        {
            return repository.GetAllIncluding().WhereIf(!string.IsNullOrEmpty(input.Keyword), d => d.ModuleName.Contains(input.Keyword));

        }
    }
}
