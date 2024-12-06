using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class PupilRepository : Repository<Pupil>, IPupilRepository
    {
        public PupilRepository(ApplicationContext context, ILogger<Repository<Pupil>> logger) : base(context, logger)
        {
        }

        public async Task<List<Pupil>> GetPupilWithoutClassesAsync(int semesterId, int schoolId)
        {
            var result = await _context.Pupils
                .Where(p => p.SchoolId == schoolId && (p.AccountStatus == (int)Statuses.Active || p.AccountStatus == (int)Statuses.Inactive) &&
                            !_context.ClassEnrollments
                                .Any(ce => ce.PupilId == p.Id && ce.SemesterId == semesterId))
                .ToListAsync();
            return result;
        }


        public async Task<object> LoginAsyncByModePupil(string email, string password)
        {
            return await _context.Pupils.LoginAsync(_context, _logger, email, password);
        }
        public async Task<Pupil> GetPupilProfile(int id)
        {
            return await _context.Pupils.Include(x => x.PupilClasses).ThenInclude(x => x.Class).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Pupil>> GetListPupilOfSchoolAsync(
        int schoolId,
        int? accountStatus = null,
        string? searchKey = null)
        {
            var query = _context.Pupils
                .Where(t => t.SchoolId == schoolId)
                .AsQueryable();


            if (accountStatus.HasValue)
            {
                query = query.Where(t => t.AccountStatus == accountStatus);
            }

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var lowerKeyword = searchKey.ToLower();
                query = query.Where(t => (t.LastName.ToLower() + " " + t.FirstName.ToLower()).Contains(lowerKeyword) ||
                        t.FirstName.ToLower().Contains(lowerKeyword) ||
                        t.LastName.ToLower().Contains(lowerKeyword) ||
                        t.Username.ToLower().Contains(lowerKeyword) ||
                        t.Email.ToLower().Contains(lowerKeyword));
            }

            query = query.OrderByDescending(t => t.Id);

            return await query.ToListAsync();
        }
        public async Task<Pupil?> GetPupilDetailAsync(int pupilId)
        {

            return await _context.Pupils.FirstOrDefaultAsync(t => t.Id == pupilId);

        }
    }
   
}
