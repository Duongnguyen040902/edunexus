using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IClassApplicationService : IBaseService<ClassAppicationDTO, ClassApplication>
    {
        Task<List<GetClassAppicationDetailDTO>> GetClassApplicationByClassId(int classId, int semesterId, int? categoryId);
        Task<GetClassAppicationDetailDTO> GetClassApplicationDetailById(int id);
        Task<List<GetClassAppicationDetailDTO>> GetClassApplicationByPupilId(int pupilId, int semesterId);
        Task<bool> UpdateClassApplication(CreateAndUpdateClassApplicationDTO model);
        Task<bool> CreateClassApplication(CreateAndUpdateClassApplicationDTO model, int schoolId);
        Task<bool> ResponeClassApplication(ResponeClassApplicationDTO model);
        Task<bool> DeleteClassApplication(int id);
        Task<List<ClassApplicationCategoryDTO>> GetAllClassApplicationCategory();

    }
}
