using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IFeedbackRepository : IRepository<PupilFeedback>
    {
        Task<List<PupilFeedback>> GetAllFeedbacksOfPupil(int pupilId);
    }
}
