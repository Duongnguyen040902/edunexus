using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ITimeSlotService : IBaseService<TimeSlotDTO, TimeSlot>
    {
        Task<List<TimeSlotDTO>> GetAll(int schoolId);
        Task<PagedResponse<List<TimeSlotDTO>>> GetAllTimeSlots(PaginationFilter filters, IUriService uriService, string route, bool? isActive, string? keyword, int schoolId);
        Task<TimeSlotDTO> GetTimeSlotDetail(int id);
        Task<TimeSlotDTO> CreateTimeSlot(int schoolId,CreateTimeSlotDTO timeSlotDto);
        Task<TimeSlotDTO> UpdateTimeSlot(int id, CreateTimeSlotDTO timeSlotDto, int schoolId);
        Task<bool> DeleteTimeSlot(int id);
    }
}
