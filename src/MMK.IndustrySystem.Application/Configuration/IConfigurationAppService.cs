using System.Threading.Tasks;
using MMK.SmartSystem.Configuration.Dto;

namespace MMK.SmartSystem.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
