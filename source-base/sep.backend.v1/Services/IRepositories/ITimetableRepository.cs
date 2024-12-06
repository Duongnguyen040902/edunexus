using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ITimetableRepository : IRepository<TimeTable>
    {
        Task<List<TimeTable>> GetTimeTableDetail(int classId, int semesterId);
    }
}
