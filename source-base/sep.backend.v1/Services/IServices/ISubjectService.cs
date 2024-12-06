using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISubjectService : IBaseService<SubjectDTO, Subject>
    {
        Task<List<SubjectDTO>> GetAll(int schoolId);
        Task<SubjectDTO> GetSubjectById(int id);
        Task<SubjectDTO> CreateSubject(SubjectDTO subjectDTO, int schoolId);
        Task<SubjectDTO> UpdateSubject(int id, SubjectDTO subjectDTO);
        Task<bool> DeleteSubject(int id, int schoolId);
    }
}
