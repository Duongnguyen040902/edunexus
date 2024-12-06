using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Requests.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace sep.backend.v1.Services.IServices
{
    public interface IAuthService : IBaseService<UserDTO, User>
    {
        Task<Object> LoginByModeAsync(LoginRequest loginRequest);

        Task<Object> RegisterAdminSchool(RegisterDTO registerDto);

        Task<Object> FindUserByMode(int mode, string email);

        Task<Object> ResetPasswordByMode(int mode, string email, string password);
        Task<bool> LogoutAsync(string accessToken);
        Task<Object> UpdateFirstLoginAsync(VerifyFirstLoginRequest verifyFirstLoginRequest);
        Task<bool> UpdateStatusCompleteVerifyTask(int mode, int id);

        Task<bool> ChangePassword(ChangePasswordDTO model, int mode,int userId);
    }
}