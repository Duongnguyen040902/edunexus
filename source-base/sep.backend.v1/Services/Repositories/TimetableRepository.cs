using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class TimetableRepository : Repository<TimeTable>, ITimetableRepository 
    {
        public TimetableRepository(ApplicationContext context, ILogger<Repository<TimeTable>> logger) : base(context, logger)
        {
        }

        public Task<List<TimeTable>> GetTimeTableDetail(int classId, int semesterId)
        {
           var timetable = _context.TimeTables
                .Include(x=>x.Subject)
                .Include(x=>x.Class)
                .Include(x => x.Semester)
                .Include(x => x.TimeSlot)
                .Where(x => x.ClassId == classId && x.SemesterId == semesterId).ToListAsync(); 

            return timetable;
        }
    }
    
}
