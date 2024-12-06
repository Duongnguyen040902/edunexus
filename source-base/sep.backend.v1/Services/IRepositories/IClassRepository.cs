using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<Class> GetClassDetail(int id, int semesterId);
        public Task<Class> GetClassByNameAsync(string name, int schoolId);

        public Task<List<Class>> getAllClass(int schoolId, string? keyword, int? status);
    }
}
