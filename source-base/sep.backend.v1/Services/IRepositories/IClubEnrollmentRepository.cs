using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IClubEnrollmentRepository : IRepository<ClubEnrollment>
    {
        Task<bool> CheckEnrollmentIdsExistAsync(AssignMemberRequest[] assignRequests);
        Task<List<ClubEnrollment>> GetClubEnrollments(int? clubId, int? semesterId,string? keyword);
        Task<List<Teacher>> GetTeachersNotInClub(int clubId, int semesterId, int schoolId);
        Task<List<Pupil>> GetPupilsNotInClubAsync(int clubId, int semesterId, int schoolId);
        Task<List<ClubEnrollment>> GetPupilsRegisterClub(int? clubId, int? semesterId);
        Task<List<ClubEnrollment>> GetMembersInNextSemester(int semesterId);
    }
}
