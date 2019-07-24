using System.Threading.Tasks;
using Abp.Application.Services;
using MMK.SmartSystem.Authorization.Accounts.Dto;

namespace MMK.SmartSystem.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
