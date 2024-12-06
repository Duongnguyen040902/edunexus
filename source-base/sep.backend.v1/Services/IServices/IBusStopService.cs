using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Common.Responses;

namespace sep.backend.v1.Services.IServices
{
    public interface IBusStopService : IBaseService<BusStopDTO, BusStop>
    {
        Task<BusStopDTO> CreateBusStop(CreateBusStopDTO busStopDto);
        Task<BusStopDTO?> UpdateBusStop(int id, CreateBusStopDTO busStopDto);
        Task<bool> DeleteBusStop(int id);
        Task<BusStopDTO?> GetBusStopDetail(int id);
        Task<PagedResponse<List<BusStopDTO>>> GetListBusStop(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int busRouteId);
    }
}
