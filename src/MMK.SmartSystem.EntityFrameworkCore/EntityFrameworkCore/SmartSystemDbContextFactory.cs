using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MMK.SmartSystem.Configuration;
using MMK.SmartSystem.Web;

namespace MMK.SmartSystem.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class SmartSystemDbContextFactory : IDesignTimeDbContextFactory<SmartSystemDbContext>
    {
        public SmartSystemDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SmartSystemDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            SmartSystemDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SmartSystemConsts.ConnectionStringName));

            return new SmartSystemDbContext(builder.Options);
        }
    }
}
