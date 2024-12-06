using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IBusStopRepository : IRepository<BusStop>
    {
        Task<List<BusStop>> GetBusStops(int? status, string? keyword, int busRouteId);
    }
}
