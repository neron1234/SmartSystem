using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MMK.SmartSystem.Roles.Dto;
using MMK.SmartSystem.Users.Dto;

namespace MMK.SmartSystem.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
