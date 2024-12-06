using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ISchoolRepository : IRepository<School>
    {
        Task<Object> LoginAsyncByModeSchool(string email, string password);
        Task<List<School>> getAllAccountSchoolAdmin(int? status, string? keyword, int? subscriptionPlanId);
        Task<School> getAccountSchoolAdminBySchoolId(int schoolId);
        Task<List<School>> getAllSchoolExpired();
    }
}
