using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class SchoolYearRepository : Repository<SchoolYear>, ISchoolYearRepository
    {
        public SchoolYearRepository(ApplicationContext context, ILogger<Repository<SchoolYear>> logger) : base(context, logger)
        {
        }
    }
}
