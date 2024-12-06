using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services.Repositories
{
    public class AttendanceRecordRepository : Repository<AttendanceRecordRepository>, IAttendanceRecordRepository
    {
        public AttendanceRecordRepository(ApplicationContext context, ILogger<Repository<AttendanceRecordRepository>> logger) : base(context, logger)
        {
        }
    }
}
