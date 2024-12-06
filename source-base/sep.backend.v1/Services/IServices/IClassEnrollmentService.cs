using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IClassEnrollmentService : IBaseService<ClassEnrollmentDTO, ClassEnrollment>
    {
        public Task<bool> AssignPupilToClass(AssignPupilRequest[] request,int schoolId);
        public Task<bool> AssignTeacherToClass(AssignTeacherRequest request,int schoolId);
            
        Task<bool> RemoveTeacherFromClass(int classId, int semesterId, int teacherId);
        Task<bool> RemovePupilFromClass(int classId, int semesterId, int pupilId);
        Task<bool> RemoveMemberClass(int id);
        Task<bool> AssignMemberToClass(AssignMemberToClass[] request);
        Task<bool> PupilsToGraduate(int[] pupilIds);
        Task<bool> SwapTeachersInClass(int ceTeacherId1, int ceTeacherId2);
        Task<List<TeacherSwapDTO>> GetTeacherInClass(int semesterId, int schoolId, int ceTeacherId1);
        public Task<bool> UpdateAssignTeacherToClass(UpdateAssignTeacherRequest request);
        public Task<int> GetTeacherId(int classId, int semesterId);
        Task<PagedResponse<List<MemberInClassDTO>>> GetMemberInClass(PaginationFilter filters,
            IUriService uriService,
            string route,
            string? keyword, int classId, int semesterId);
       
       Task<List<MemberInClassDTO>> GetMemberToCopy(int nextSemesterId);
    }
}
