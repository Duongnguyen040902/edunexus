using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IBusEnrollmentRepository : IRepository<BusEnrollment>
    {
        Task<List<BusEnrollment>> GetBusEnrollments(int? busId, int? semesterId);
        Task<bool> CheckEnrollmentIdsExistAsync(CreateBusEnrollmentDTO[] busEnrollmentDto);
        Task<List<BusEnrollment>> GetMembersInNextSemester(int semesterId);
    }
}
