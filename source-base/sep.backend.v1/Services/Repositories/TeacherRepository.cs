using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Linq;

namespace sep.backend.v1.Services.Repositories
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationContext context, ILogger<Repository<Teacher>> logger) : base(context, logger)
        {
        }

        public async Task<Teacher?> GetTeacherDetailAsync(int teacherId)
        {

            return await _context.Teachers
                                 .Include(t => t.TeacherSubjects)
                                 .ThenInclude(ts => ts.Subject)
                                 .FirstOrDefaultAsync(t => t.Id == teacherId);

        }

        public async Task<List<Teacher>> GetListTeacherOfSchoolAsync(
        int schoolId,
        int? subjectId = null,
        int? accountStatus = null,
        string? searchKey = null)
        {
            var query = _context.Teachers
                .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
                .Where(t => t.SchoolId == schoolId)
                .AsQueryable();

            if (subjectId.HasValue)
            {
                query = query.Where(t => t.TeacherSubjects.Any(ts => ts.SubjectId == subjectId.Value));
            }

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

        public async Task<List<Teacher>> GetTeachersWithoutClassesAsync(int semesterId, int schoolId)
        {
            return await _context.Teachers
                    .Where(t => t.SchoolId == schoolId && (t.AccountStatus == (int)Statuses.Active || t.AccountStatus == (int)Statuses.Inactive) &&
                     !_context.ClassEnrollments
                     .Any(ce => ce.TeacherId == t.Id && ce.SemesterId == semesterId))
                    .ToListAsync();
        }
        public async Task<object> LoginAsyncByModeTeacher(string email, string password)
        {
            return await _context.Teachers.LoginAsync(_context, _logger, email, password);
        }

        public async Task<List<Teacher>> GetTeachersInClassesAsync(int semesterId, int schoolId)
        {
            return await _context.Teachers
        .Where(t => t.SchoolId == schoolId &&
                    _context.ClassEnrollments
                    .Any(ce => ce.TeacherId == t.Id && ce.SemesterId == semesterId))
                    .ToListAsync();
        }

        public async Task<Teacher> GetTeacherProfile(int id)
        {
            return await _context.Teachers.Include(x => x.TeacherSubjects).ThenInclude(x => x.Subject).FirstOrDefaultAsync(x => x.Id == id);
        }
    }

}
