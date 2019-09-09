using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MMK.SmartSystem.MultiTenancy.Dto;

namespace MMK.SmartSystem.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

