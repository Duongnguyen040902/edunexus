using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISchoolYearService : IBaseService<SchoolYearDTO, SchoolYear>
    {
        Task<PagedResponse<List<SchoolYearDTO>>> GetSchoolYearsBySchoolId(PaginationFilter paginationFilter, IUriService uriService, string route, int schoolId);
        Task<SchoolYearDTO> GetSchoolYear(int schoolYearId);
        Task<bool> CreateSchoolYear(CreateAndUpdateSchoolYearDTO createSchoolYearDTO, int schoolId);
        Task<bool> UpdateSchoolYear(CreateAndUpdateSchoolYearDTO updateSchoolYearDTO, int schoolId);
        Task<bool> DeleteSchoolYear(int schoolYearId);
    }
}
