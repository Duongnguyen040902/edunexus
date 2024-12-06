using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IBusRepository : IRepository<Bus>
    {
        Task<Bus> GetBusDetailAsync(int busId, int semesterId);
        Task<List<Bus>> GetBuses(int? status, string? keyword, int? schoolId,int busRouteId);
    }
}
