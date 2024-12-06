using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories;

public interface IClubRepository : IRepository<Club>
{
    Task<Club> GetClubDetailAsync(int clubId, int semesterId);
    Task<List<Club>> GetClubsDetailAsync(int schoolId);
    Task<List<Club>> GetAllClubs(int? status, string? keyword, int? schoolId);
}