using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface ISchoolService : IBaseService<SchoolDTO, School>
    {
        Task<SchoolInfoDTO> GetSchoolById(int id);

        Task<bool> UpdateInfoSchool(UpdateInfoSchoolDTO schoolDTO);
    }
}

