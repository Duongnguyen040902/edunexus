using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IPupilFeedbackService : IBaseService<PupilFeedbackDTO, PupilFeedback>
    {
        Task<List<ListPupilFeedbackDTO>> GetPupilFeedbackOfClass(int classId, int semesterId);
        Task<bool> CreatePupilFeedback(PupilFeedbackDTO models);
        Task<bool> UpdatePupilFeedback(PupilFeedbackDTO models);
        Task<bool> DeletePupilFeedback(int pupilId, int semesterId);
        Task<List<PupilFeedbackDetailDTO>> GetPupilFeedbacks(int pupilId);
       
    }
}
