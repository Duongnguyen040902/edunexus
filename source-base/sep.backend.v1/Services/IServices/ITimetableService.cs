using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ITimetableService
    {
        Task<bool> CreateTimeTableAsync(TimeTableDTO model);
        Task<bool> DeleteTimeTableAsync(TimeTableDTO model);
        Task<bool> UpdateTimeTableAsync(TimeTableDTO model);
        Task<List<TimeTableDetailDTO>> GetTimeTableDetailAsync(int classId, int semesterId);
    }
}
