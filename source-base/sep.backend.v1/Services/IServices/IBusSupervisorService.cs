using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Requests.Excel;

namespace sep.backend.v1.Services.IServices
{
    public interface IBusSupervisorService:IBaseService<ProfileBusSupervisorDTO, BusSupervisor>
    {
        Task<ProfileBusSupervisorDTO> GetBusSupervisorById(int id);
        Task<bool> UpdateBusSupervisor(UpdateProfileBusSupervisorDTO busSupervisorDTO);
        Task<bool> CreateBusSupervisorAccountAsync(int schoolId, CreateBusSupervisorDTO busSupervisorDto);
        Task<PagedResponse<List<BusSupervisorAccountDetailDTO>>> GetListBusSupervisorOfSchoolAsync(PaginationFilter filters, IUriService uriService, string route, int schoolId, int? accountStatus = null, string? searchKey = null);
        Task<bool> UpdateBusSupervisorAsync(int busSupervisorId, UpdateBusSupervisorDTO updateBusSupervisorDto);
        Task<bool> DeleteMultipleBusSupervisorAsync(IEnumerable<int> busSupervisorId);
        Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateBusSupervisorAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest);
        Task<BusSupervisorAccountDetailDTO> GetBusSupervisorDetailAsync(int busSupervisorId);
    }
}
