using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IClassEnrollmentRepository : IRepository<ClassEnrollment>
    {
        Task<ClassEnrollment> GetEnrollmentByClassAndTeacherAsync(int classId, int teacherId, int semesterId, int schoolId);

        Task<ClassEnrollment> GetEnrollmentByClassAndPupilAsync(int pupilId, int classId, int semesterId, int schoolId);
        Task<List<ClassEnrollment>> GetMembersInClass(int semesterId, int classId, string? keyword);
        Task<List<ClassEnrollment>> GetMembersInNextSemester(int semesterId);
        Task<bool> ValidateUpdateAssignTeacherAsync(UpdateAssignTeacherRequest request);

        Task<bool> CheckEnrollmentIdsExistAsync(AssignMemberToClass[] assignRequests);
    }
}
