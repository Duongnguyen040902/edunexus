using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ITimeSlotRepository : IRepository<TimeSlot>
    {
        Task<IEnumerable<TimeSlot>> GetAllTimeSlots(bool? isActive, string? keyword, int schoolId);
        Task<int> GetTotalRecords(bool? isActive, string? keyword);
    }
}
