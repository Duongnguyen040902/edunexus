using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Helpers;

public static class LoginHelper
{
    public static async Task<object> LoginAsync<TEntity>(this DbSet<TEntity> dbSet,
        ApplicationContext context,
        ILogger logger,
        string email,
        string password) where TEntity : BaseUserEntity
    {
        return await LoginAsync(dbSet, context, null, logger, email, password);
    }

    public static async Task<object> LoginAsync<TEntity>(this DbSet<TEntity> dbSet,
        ApplicationContext context,
        IConfiguration? configuration,
        ILogger logger,
        string email,
        string password) where TEntity : BaseUserEntity
    {
        var entity = await dbSet.FirstOrDefaultAsync(e => e.Email == email || e.Username == email);

        if (entity == null || !BCrypt.Net.BCrypt.Verify(password, entity.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, entity.Email ?? entity.Username ?? ""),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, entity.ShortRoleName)
        };

        AddSchoolClaimsIfApplicable(entity, authClaims, logger);

        var token = configuration != null ? JWTHelper.CreateToken(authClaims, configuration) : null;
        var refreshToken = JWTHelper.GenerateRefreshToken();
        entity.RefreshToken = refreshToken;
        entity.RefreshTokenExpiryDate = DateTime.Now.AddDays(7);

        context.Update(entity);
        await context.SaveChangesAsync();

        return new
        {
            AuthClaims = authClaims,
            entity.Email,
            Role = entity.ShortRoleName,
            Token = token,
            RefreshToken = refreshToken,
            User = entity
        };
    }

    public static async Task<T?> FindByEmailAsync<T>(IRepository<T> repository, string email) where T : BaseUserEntity
    {
        return await repository.Where(x => x.Email == email).FirstOrDefaultAsync();
    }

    private static void AddSchoolClaimsIfApplicable<TEntity>(TEntity entity, List<Claim> authClaims, ILogger logger)
        where TEntity : BaseUserEntity
    {
        switch (entity)
        {
            case School schoolAdmin:
                if (schoolAdmin.Id != null)
                {
                    authClaims.Add(new Claim("SchoolId", schoolAdmin.Id.ToString()));
                    authClaims.Add(new Claim("SchoolAdminId", schoolAdmin.Id.ToString()));
                }

                break;
            case Pupil pupilUser when pupilUser.SchoolId != null:
                if (pupilUser.SchoolId != null)
                {
                    authClaims.Add(new Claim("SchoolId", pupilUser.SchoolId.ToString()));
                    authClaims.Add(new Claim("PupilId", pupilUser.Id.ToString()));
                }

                break;
            case Teacher teacherUser when teacherUser.SchoolId != null:
                authClaims.Add(new Claim("SchoolId", teacherUser.SchoolId.ToString()));
                authClaims.Add(new Claim("TeacherId", teacherUser.Id.ToString()));
                break;
            case BusSupervisor busSupervisorUser when busSupervisorUser.SchoolId != null:
                if (busSupervisorUser.SchoolId != null)
                {
                    authClaims.Add(new Claim("SchoolId", busSupervisorUser.SchoolId.ToString()));
                    authClaims.Add(new Claim("BusSupervisorId", busSupervisorUser.Id.ToString()));
                }

                break;
            default:
                logger.LogWarning($"Entity does not have a valid SchoolId: {entity.Email}");
                break;
        }
    }
}