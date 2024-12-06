using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class FeedbackRepository : Repository<PupilFeedback>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationContext context, ILogger<Repository<PupilFeedback>> logger) : base(context, logger)
        {
        }

        public async Task<List<PupilFeedback>> GetAllFeedbacksOfPupil(int pupilId)
        {
            return await _context.PupilFeedbacks
                .Include(x => x.Semester)
                .ThenInclude(x => x.SchoolYear)
                .Where(x => x.PupilId == pupilId)
                .ToListAsync();
        }

    }
}
