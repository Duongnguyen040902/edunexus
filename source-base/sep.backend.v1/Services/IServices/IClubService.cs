using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IClubService : IBaseService<ClubDTO, Club>
    {
        Task<List<ClubDTO>> GetAssignClubAsync(int teacherId);
        Task<ClubDetailDTO> GetClubAsync(int clubId, int semesterId);      
        Task<ClubDTO> GetClubDetail(int clubId);
        Task<List<ClubDTO>> GetEnrolledClubAsync(int pupilId);
        Task<PagedResponse<List<ClubDetailDTO>>> GetClubsBySchoolId(PaginationFilter filters, IUriService uriService,
            string route,int schoolId);
        Task<PagedResponse<List<ClubDTO>>> GetListClubs(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int? schoolId);
        Task<ClubDTO> CreateClub(CreateClubDTO clubDto, int schoolId);
        Task<ClubDTO?> UpdateClub(int id, CreateClubDTO clubDto);
        Task<bool> DeleteClub(int id);
    }
}
