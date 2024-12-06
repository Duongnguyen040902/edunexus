using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories;

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(ApplicationContext context, ILogger<Repository<Subject>> logger) : base(context, logger)
    {
    }
}