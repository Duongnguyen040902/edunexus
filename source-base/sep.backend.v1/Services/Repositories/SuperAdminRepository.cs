using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class SuperAdminRepository : Repository<SuperAdmin>, ISuperAdminRepository
    {
        public SuperAdminRepository(ApplicationContext context, ILogger<Repository<SuperAdmin>> logger) : base(context, logger)
        {
        }
    }
}
