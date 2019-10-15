using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MMK.SmartSystem.Authorization.Roles;
using MMK.SmartSystem.Authorization.Users;
using MMK.SmartSystem.MultiTenancy;
using MMK.CNC.Core.Managements;
using MMK.CNC.Core.SystemClient;
using MMK.CNC.Core.LaserLibrary;
using MMK.CNC.Core.LaserProgram;

namespace MMK.SmartSystem.EntityFrameworkCore
{
    public class SmartSystemDbContext : AbpZeroDbContext<Tenant, Role, User, SmartSystemDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<Department> Departments { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }

        public DbSet<CuttingData> CuttingDatas { set; get; }

        public DbSet<EdgeCuttingData> EdgeCuttingDatas { set; get; }

        public DbSet<Gas> Gass { set; get; }

        public DbSet<MachiningDataGroup> MachiningDataGroups { set; get; }

        public DbSet<MachiningKind> MachiningKinds { set; get; }

        public DbSet<Material> Materials { set; get; }

        public DbSet<NozzleKind> NozzleKinds { set; get; }

        public DbSet<PiercingData> PiercingDatas { set; get; }

        public DbSet<SlopeControlData> SlopeControlDatas { set; get; }

        public DbSet<ProgramComment> ProgramComments { set; get; }
        public SmartSystemDbContext(DbContextOptions<SmartSystemDbContext> options)
            : base(options)
        {
        }
    }
}
