using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MMK.SmartSystem.Configuration.Dto;

namespace MMK.SmartSystem.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : SmartSystemAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
