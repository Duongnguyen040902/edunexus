using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IClubEnrollmentService : IBaseService<ClubEnrollment, ClubEnrollmentDTO>
    {
        //Học sinh tạo ClubEnrollment
        Task<bool> PupilCreateClubEnrollmentAsync(ClubEnrollmentDTO model, int semesterId, int pupilId);
        // update clubEnrollment
        Task<bool> UpdateClubEnrollmentAsync(ClubEnrollmentDTO model, int semesterId, int pupilId);
        // Lấy danh sách ClubEnrollmentDetailDTO theo pupilId
        Task<List<ClubEnrollmentDetailDTO>> GetClubEnrollmentByPupilIdAsync(int pupilId, int semesterId);
        Task<PagedResponse<List<ClubEnrollmentForAdminSchoolDTO>>> GetListClubEnrollments(
            PaginationFilter filters,
            IUriService uriService,
            string route,
            string? keyword,
            int clubId,
            int semesterId);
        Task<bool> AssignMemberToClub(AssignMemberRequest[] request, int schoolId);
        Task<bool> UpdateMemberInClub(UpdateMemberRequest[] request);
        Task<bool> RemoveMemberFromClub(int id);
        Task<List<TeacherAssignClubDTO>> GetTeachersNotInClub(int clubId, int semesterId, int schoolId);
        Task<List<PupilAssignToClubDTO>> GetPupilsNotInClub(int clubId, int semesterId, int schoolId);
        Task<List<ClubEnrollmentForAdminSchoolDTO>> GetPupilsRegisterClub(int clubId, int semesterId);
        Task<List<ClubEnrollmentForAdminSchoolDTO>> GetMemberToCopy(int nextSemesterId);
    }
}
