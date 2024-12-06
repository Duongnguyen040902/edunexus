using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IPupilScoreService : IBaseService<PupilScoreDTO, PupilScore>
    {
        Task<List<PupilScoreViewDTO>> GetPupilForCreate(int entityId, int semesterId, int subjectId);
        Task<bool> CreatePupilScores(List<PupilScoreDTO> pupilScores);
        Task<List<PupilScoreViewDTO>> GetPupilScores(int entityId, int semesterId, int subjectId);
        Task<bool> UpdatePupilScores(List<PupilScoreDTO> updatedScores);
        Task<ClassScoreListDTO> GetPupilScoreListDTOs(int classId, int semesterId);
        Task<List<PupilIndividualScoreDTO>> GetIndividualPupilScores(int pupilId, int semesterId);
    }
}
