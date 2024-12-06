using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Object> LoginAsyncByModeTeacher(string email, string password);
        Task<List<Teacher>> GetListTeacherOfSchoolAsync(
            int schoolId,
            int? subjectId = null,
            int? accountStatus = null,
            string? searchKey = null);
        Task<Teacher?> GetTeacherDetailAsync(int schoolId);
        Task<List<Teacher>> GetTeachersWithoutClassesAsync(int semesterId,int schoolId);
       
       
        Task<Teacher> GetTeacherProfile(int id);
    }
}
