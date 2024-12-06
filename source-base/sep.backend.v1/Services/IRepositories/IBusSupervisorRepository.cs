using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IBusSupervisorRepository : IRepository<BusSupervisor>
    {
        Task<Object> LoginAsyncByModeBusSupervisor(string email, string password);
        Task<List<BusSupervisor>> GetListBusSupervisorOfSchoolAsync(
        int schoolId,
        int? accountStatus = null,
        string? searchKey = null);
    }
}
