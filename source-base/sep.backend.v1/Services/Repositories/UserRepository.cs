using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context, ILogger<Repository<User>> logger) : base(context, logger)
        {
        }

        public async Task<User> DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> LoginAsync(User user)
        {
            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(user.Password, userEntity.Password))
            {
                return null;
            }

            return userEntity;
        }

        public async Task<User> LoginByEmailAsync(string email)
        {
            var userEntity = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            return userEntity;
        }

        public async Task<bool> UpdateRefreshTokenAsync(User user, string refreshToken, DateTime? refreshTokenExpiration)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (existingUser == null)
                {
                    return false;
                }

                existingUser.RefreshToken = refreshToken;
                existingUser.RefreshTokenExpiryDate = refreshTokenExpiration;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}