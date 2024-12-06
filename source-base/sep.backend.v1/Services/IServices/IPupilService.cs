using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Requests.Excel;

namespace sep.backend.v1.Services.IServices
{
    public interface IPupilService : IBaseService<PupilDTO, Pupil>
    {
        Task<bool> CreatePupilAccountAsync(int schoolId, CreatePupilDTO pupilDto);
        Task<PagedResponse<List<PupilDetailDTO>>> GetListPupilOfSchoolAsync(PaginationFilter filters, IUriService uriService, string route,
                                                                           int schoolId, int? accountStatus = null, string? searchKey = null);
        Task<List<PupilDetailDTO>> GetPupilAssignToClass(int semesterId,int schoolId);
        Task<PupilDetailDTO> GetPupilDetailAsync(int pupilId);
        Task<bool> UpdatePupilAsync(int pupilId, UpdatePupilDTO updatePupilDto);

        Task<ProfilePupilDTO> GetProfilePupilAsync(int id);
        Task<bool> UpdateProfilePupilAsync(UpdateProfilePupilDTO profilePupil);
        Task<bool> DeleteMultiplePupilAsync(IEnumerable<int> pupilId);
        Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreatePupilAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest);
    }
}
