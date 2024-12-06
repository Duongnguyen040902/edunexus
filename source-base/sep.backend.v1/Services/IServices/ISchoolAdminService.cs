using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISchoolAdminService : IBaseService<SchoolDTO, School>
    {
        Task<PagedResponse<List<SchoolDTO>>> GetAllAccountSchoolAdmin(PaginationFilter filters, IUriService uriService,
            string route, int? status, string? keyword, int? subscriptionPlanId);
        Task<SchoolDTO> GetSchoolAdminAsync(int schoolId);
        Task<UpdateSchoolDTO> UpdateSchoolAdminAsync(int id, UpdateSchoolDTO schoolEntity);
        Task<CreateSchoolDTO> CreateSchoolAdminAsync(CreateSchoolDTO schoolEntity);
        Task<SchoolDTO> DeleteSchoolAdminAsync(int schoolId);
    }
}
