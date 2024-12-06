using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IClassService : IBaseService<ClassDTO, Class>
    {
        Task<ClassDTO> GetAssignClassAsync(int teacherId);
        Task<ClassDetailDTO> GetClassAsync(int classId, int semesterId);
        Task<ViewClassAdminDTO> GetClassDetailByID(int classId);
        Task<ClassDTO> UpdateClassAsync(ClassDTO classEntity);
        Task<TeacherAccountDTO> GetCurrentTeacherOfClassAsync(int classId, int semesterId);
        
        Task<PagedResponse<List<ViewClassAdminDTO>>> GetAllClass(PaginationFilter filters, IUriService uriService, string route, int schoolId, string? keyword, int? status);
        Task<bool> UpdateClass(UpdateClassDTO updateClassDTO, int schoolId);
        
        Task<bool> DeleteClass(int id);
        Task<bool> AddNewClass (AddClassDTO addClassDTO,int schoolId);
        Task<ClassDTO> GetEnrollmentClassAsync(int pupilId);

        Task<List<ViewClassEnrollDTO>> GetClassesByPupilIdAsync(int pupilId);
    }
}