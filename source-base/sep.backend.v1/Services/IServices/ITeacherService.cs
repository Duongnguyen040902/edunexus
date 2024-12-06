using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Requests.Excel;
using System.Threading.Tasks;

namespace sep.backend.v1.Services.IServices
{
    public interface ITeacherService : IBaseService<TeacherDTO, Teacher>
    {
        Task<bool> CreateTeacherAccountAsync(int schoolId, CreateTeacherDTO teacherDto);

        Task<PagedResponse<List<TeacherDetailDTO>>> GetListTeacherOfSchoolAsync(PaginationFilter filters, IUriService uriService, string route,
                                                                           int schoolId, int? subjectId = null, int? accountStatus = null, string? searchKey = null);
        Task<List<TeacherDetailDTO>> GetTeacherAssignToClass(int semesterId, int schoolId);


        Task<TeacherDetailDTO> GetTeacherDetailAsync(int teacherId);
        Task<bool> UpdateTeacherAsync(int teacherId, UpdateTeacherDTO updateTeacherDto);
        Task<ProfileTeacherDTO> GetProfileTeacherAsync(int id);
        Task<bool> UpdateProfileTeacherAsync(UpdateProfileTeacherDTO profileTeacher);
        Task<bool> DeleteMultipleTeacherAsync(IEnumerable<int> teacherId);
        Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateTeacherAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest);
    }
}
