namespace MMK.SmartSystem.EntityFrameworkCore.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly SmartSystemDbContext _context;

        public InitialHostDbBuilder(SmartSystemDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefulatCNCCreator(_context).Create();
            _context.SaveChanges();
        }
    }
}
