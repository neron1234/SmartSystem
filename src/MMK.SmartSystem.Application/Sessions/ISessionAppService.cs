using System.Threading.Tasks;
using Abp.Application.Services;
using MMK.SmartSystem.Sessions.Dto;

namespace MMK.SmartSystem.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
