using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISemesterService : IBaseService<SemesterDTO, Semester>
    {
        Task<IEnumerable<ViewSemesterDTO>> GetAllAsync(int schoolId);
        Task<SemesterDTO> GetNewSemester(int schoolId);
        Task<SemesterDTO> GetCurrentSemester(int schoolId);
        Task<SemesterDTO> GetNextSemester(int schoolId);
        Task<List<ViewSemesterDTO>> GetListSemesters(int schoolId, int schoolYearId);
        Task<SemesterDTO> GetSemester(int id);
        Task<bool> Create(CreateAndUpdateSemesterDTO semesterDTO);
        Task<bool> Update(CreateAndUpdateSemesterDTO semesterDTO, int schoolId);
        Task<bool> Delete(int id);
    }
}
