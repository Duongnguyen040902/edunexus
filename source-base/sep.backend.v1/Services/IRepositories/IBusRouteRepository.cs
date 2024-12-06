using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IBusRouteRepository : IRepository<BusRoute>
    {
        Task<List<BusRoute>> getAllBusRoute(int? status, string? keyword, int? schoolId);
    }
}
