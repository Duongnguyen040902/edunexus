using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISchoolDashboardService : IBaseService<School, SchoolDTO>
    {
        Task<SchoolDashboardDTO> GetSchoolDashboard(int schoolId);
    }
}
