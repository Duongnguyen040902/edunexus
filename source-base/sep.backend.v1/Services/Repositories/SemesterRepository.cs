using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class SemesterRepository : Repository<Semester>, ISemesterRepository
    {
        public SemesterRepository(ApplicationContext context, ILogger<Repository<Semester>> logger) : base(context, logger)
        {
        }
    }
}
