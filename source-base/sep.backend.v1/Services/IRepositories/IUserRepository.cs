using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Services.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> LoginAsync(User user);

        Task<User> LoginByEmailAsync(string email);

        Task<bool> UpdateRefreshTokenAsync(User user, string refreshToken, DateTime? refreshTokenExpiration);
    }
}