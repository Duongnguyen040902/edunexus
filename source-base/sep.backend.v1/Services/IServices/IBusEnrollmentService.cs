using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.DTOs;
using sep.backend.v1.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using sep.backend.v1.Requests.Excel;

namespace sep.backend.v1.Services.IServices
{
    public interface IBusEnrollmentService : IBaseService<BusEnrollmentDTO, BusEnrollment>
    {
        Task<bool> CreateBusEnrollment(CreateBusEnrollmentDTO[] busEnrollmentDto);
        Task<CreateBusEnrollmentDTO?> UpdateBusEnrollment(int id, CreateBusEnrollmentDTO busEnrollmentDto);
        Task<bool> DeleteBusEnrollment(int id);
        Task<BusEnrollmentDTO?> GetBusEnrollmentDetail(int id);      
        Task<List<PupilDetailDTO>> GetListPupilsInBusStop(int semesterId, int busStopId, int schoolId);
        Task<List<PupilDetailDTO>> GetPupilWithoutEnrollments(int semesterId,int schoolId);
        Task<List<BusSupervisorDTO>> GetBusSupervisorWithoutEnrollments(int semesterId, int schoolId);
        Task<PagedResponse<List<BusEnrollmentDTO>>> GetListBusEnrollments(
            PaginationFilter filters,
            IUriService uriService,
            string route,
            int busId,
            int semesterId
        );
        Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateMemberBusAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest);
        Task<List<BusEnrollmentDTO>> GetMemberToCopy(int nextSemesterId);
    }
}
