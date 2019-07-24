using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MMK.SmartSystem.EntityFrameworkCore
{
    public static class SmartSystemDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SmartSystemDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SmartSystemDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
