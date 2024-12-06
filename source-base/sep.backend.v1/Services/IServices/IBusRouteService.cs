using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IBusRouteService  :IBaseService<BusRouteDTO, BusRoute>
    {
        Task<PagedResponse<List<BusRouteDTO>>> GetListBusRoute(PaginationFilter filters, IUriService uriService,
           string route, int? status, string? keyword, int? schoolId);
        Task<List<BusRouteDetailDTO>> GetBusRoute(int id);
        Task<BusRouteDTO> CreateBusRoute(CreateBusRouteDto busRouteDto, int schoolId);
        Task<BusRouteDTO?> UpdateBusRoute(int id, CreateBusRouteDto busRouteDto, int schoolId);
        Task<bool> DeleteBusRoute(int id);
    }
}
