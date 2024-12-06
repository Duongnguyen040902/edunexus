using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;

namespace sep.backend.v1.Services.IServices
{
    public interface IBusService : IBaseService<BusDTO,Bus>
    {
        Task<BusDTO> GetAssignedBus(int supervisorId);
        Task<BusDTO> GetEnrollmentBus(int pupilId);
        Task<ViewBusDetailDTO> GetBusDetail(int busId, int semesterId);
        Task<PagedResponse<List<BusDTO>>> GetListBus(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int schoolId, int busRouteId);
        Task<BusDetailDto?> GetBusDetail(int id);
        Task<BusDTO> CreateBus(CreateBusDto busDto);
        Task<BusDTO?> UpdateBus(int id, CreateBusDto busDto);
        Task<bool> DeleteBus(int id);
        Task<List<ViewBusEnrollDetailDTO>> GetViewBusEnrolls(int pupilId);
    }
}
