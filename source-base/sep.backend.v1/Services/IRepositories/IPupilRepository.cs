using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IPupilRepository : IRepository<Pupil>
    {
        Task<Object> LoginAsyncByModePupil(string email, string password);

        Task<List<Pupil>> GetPupilWithoutClassesAsync(int semesterId, int schoolId);
        Task<Pupil> GetPupilProfile(int id);
        Task<List<Pupil>> GetListPupilOfSchoolAsync( int schoolId,
                                                     int? accountStatus = null,
                                                     string? searchKey = null);
        Task<Pupil?> GetPupilDetailAsync(int pupilId);
    }
}
