using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;
using System.Net.WebSockets;

namespace sep.backend.v1.Services.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(ApplicationContext context, ILogger<Repository<Class>> logger) : base(context, logger)
        {

        }

        public async Task<Class> GetClassDetail(int id, int semesterId)
        {
            var classEntity = await _context.Classes
                .Include(x => x.School)
                .Include(x => x.ClassEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.Semester).ThenInclude(x => x.SchoolYear)
                .Include(c => c.ClassEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.Teacher)
                .Include(c => c.ClassEnrollments.Where(ce => ce.SemesterId == semesterId))
                .ThenInclude(ce => ce.Pupil)
                .FirstOrDefaultAsync(c => c.Id == id && c.ClassEnrollments.Any(x=>x.SemesterId == semesterId));

            return classEntity;
        }

        public async Task<Class> GetClassByNameAsync(string name, int schoolId)
        {
            return await _context.Classes.FirstOrDefaultAsync(c => c.Name == name && c.SchoolId == schoolId);
        }

        public async Task<List<Class>> getAllClass(int schoolId, string? keyword, int? status)
        {
            var query = _context.Classes.AsQueryable();

            query = query.Where(c => c.SchoolId == schoolId);

            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(c => c.Name.ToLower().Contains(keyword.ToLower()));
            query = query.OrderBy(c => c.Block).ThenBy(x => x.Name);
            return await query.ToListAsync();
        }
    }
}