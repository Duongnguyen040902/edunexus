using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Helpers;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Auth;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services;

public class AuthService : BaseService<UserDTO, User>, IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;
    private readonly IEmailConfirmCode _emailConfirmCode;

    public AuthService(IUnitOfWork unitOfWork, IAutoMapper mapper, IConfiguration configuration,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
        : base(unitOfWork, mapper)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }

    public async Task<object> LoginByModeAsync(LoginRequest loginRequest)
    {
        var mode = GetByMode(loginRequest.Mode);

        dynamic loginResult = mode switch
        {
            ModeLogin.SuperAdmin => await HandleLoginAsync<SuperAdmin>(loginRequest),
            ModeLogin.SchoolAdmin => await _unitOfWork.SchoolRepository.LoginAsyncByModeSchool(loginRequest.Email,
                loginRequest.Password),
            ModeLogin.Donnor => await _unitOfWork.PupilRepository.LoginAsyncByModePupil(loginRequest.Email,
                loginRequest.Password),
            ModeLogin.Teacher => await _unitOfWork.TeacherRepository.LoginAsyncByModeTeacher(loginRequest.Email,
                loginRequest.Password),
            ModeLogin.BusSuperVisor => await _unitOfWork.BusSupervisorRepository.LoginAsyncByModeBusSupervisor(
                loginRequest.Email, loginRequest.Password),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Invalid mode value")
        };

        var authClaims = loginResult.AuthClaims;
        var token = JWTHelper.CreateToken(authClaims, _configuration);
        var refreshToken = JWTHelper.GenerateRefreshToken();
        SetRefreshTokenCookie(refreshToken);
        var userDto = new
        {
            loginResult.Email,
            Token = token,
            RefreshToken = refreshToken,
            loginResult.Role,
            User = loginResult.User
        };

        return userDto;
    }


    public async Task<object> RegisterAdminSchool(RegisterDTO registerDto)
    {
        var newSchoolAdmin = _mapper.Map<RegisterDTO, School>(registerDto);
        newSchoolAdmin.AccountStatus = (int)StatusAccount.Active;
        if (newSchoolAdmin is null) throw new ArgumentNullException(nameof(newSchoolAdmin), "School admin is null");

        var subscriptionPlan = await _unitOfWork.GetRepository<SubscriptionPlan>().GetById((int)Subscription.Trial);

        if (subscriptionPlan is null) throw new InvalidOperationException("Subscription plan not found.");

        newSchoolAdmin.SchoolSubscriptionPlans.Add(new SchoolSubscriptionPlan
        {
            Status = (int)Statuses.Active,
            SubscriptionPlan = subscriptionPlan,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(1)
        });

        await _unitOfWork.GetRepository<School>().Add(newSchoolAdmin);
        await _unitOfWork.CompleteAsync();

        return new { Message = "School admin registered successfully", SchoolAdmin = newSchoolAdmin };
    }

    public async Task<object> FindUserByMode(int mode, string email)
    {
        var modeFind = GetByMode(mode);

        var userRepositoryMapping = new Dictionary<ModeLogin, Func<string, Task<object?>>>
        {
            {
                ModeLogin.SuperAdmin,
                async email => await _unitOfWork.GetRepository<SuperAdmin>().Where(x => x.Email == email)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.SchoolAdmin,
                async email => await _unitOfWork.GetRepository<School>().Where(x => x.Email == email)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.Donnor,
                async email => await _unitOfWork.GetRepository<Pupil>().Where(x => x.Email == email)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.Teacher,
                async email => await _unitOfWork.GetRepository<Teacher>().Where(x => x.Email == email)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.BusSuperVisor,
                async email => await _unitOfWork.GetRepository<BusSupervisor>().Where(x => x.Email == email)
                    .FirstOrDefaultAsync()
            }
        };

        if (userRepositoryMapping.TryGetValue(modeFind, out var findUserFunc))
        {
            var user = await findUserFunc(email);

            if (user == null)
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "attribute", "người dùng" }
                };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate,
                    placeholders));
            }

            return user;
        }

        throw new ArgumentOutOfRangeException(nameof(modeFind), "Invalid mode value");
    }

    public async Task<object> ResetPasswordByMode(int mode, string email, string password)
    {
        var user = FindUserByMode(mode, email).Result as BaseUserEntity;
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        switch (user)
        {
            case SuperAdmin superAdmin:
                superAdmin.Password = hashedPassword;
                _unitOfWork.GetRepository<SuperAdmin>().Update(superAdmin);
                break;
            case School schoolAdmin:
                schoolAdmin.Password = hashedPassword;
                _unitOfWork.GetRepository<School>().Update(schoolAdmin);
                break;
            case Pupil pupil:
                pupil.Password = hashedPassword;
                _unitOfWork.GetRepository<Pupil>().Update(pupil);
                break;
            case Teacher teacher:
                teacher.Password = hashedPassword;
                _unitOfWork.GetRepository<Teacher>().Update(teacher);
                break;
            case BusSupervisor busSupervisor:
                busSupervisor.Password = hashedPassword;
                _unitOfWork.GetRepository<BusSupervisor>().Update(busSupervisor);
                break;
            default:
                throw new InvalidOperationException("Unsupported user type");
        }

        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<object> UpdateFirstLoginAsync(VerifyFirstLoginRequest verifyFirstLoginRequest)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var user = FindUserById(verifyFirstLoginRequest.Mode, verifyFirstLoginRequest.id).Result as BaseUserEntity;
            switch (user)
            {
                case Teacher teacher:
                    teacher.Email = verifyFirstLoginRequest.Email;
                    _unitOfWork.GetRepository<Teacher>().Update(teacher);
                    break;
                case Pupil pupil:
                    pupil.Email = verifyFirstLoginRequest.Email;
                    _unitOfWork.GetRepository<Pupil>().Update(pupil);
                    break;
                case BusSupervisor busSupervisor:
                    busSupervisor.Email = verifyFirstLoginRequest.Email;
                    _unitOfWork.GetRepository<BusSupervisor>().Update(busSupervisor);
                    break;
            }

            var verificationCode = RandomCodeHelper.GenerateVerificationCode();
            var claimsForForgotPassword = new Dictionary<string, string>
            {
                { "Mode", verifyFirstLoginRequest.Mode.ToString() },
                { "Id", verifyFirstLoginRequest.id.ToString() },
                { "VerificationCode", verificationCode }
            };

            var claims = JWTHelper.GenerateClaims(claimsForForgotPassword);
            var token = JWTHelper.CreateToken(claims, _configuration, tokenValidityInSeconds: 1000);
            var body = Helpers.TemplateHelper.GetEmailTemplate("verify.html");
            body = body.Replace("{{username}}", verifyFirstLoginRequest.Email)
                .Replace("{{title_header}}", "Xác Thực Tài Khoản")
                .Replace("{{code}}", verificationCode)
                .Replace("{{title}}", "Xác Thực Tài Khoản")
                .Replace("{{content}}", "Để đăng nhập vào tài khoản của bạn, vui lòng xác thực tài khoản")
                .Replace("{{content_footer}}", "Vui lòng nhập mã này vào trang đăng nhập để truy cập tài khoản của bạn. Nếu bạn không yêu cầu mã này hoặc không cố gắng truy cập tài khoản, hãy liên hệ với chúng tôi ngay để được hỗ trợ.");
            await _unitOfWork.CompleteAsync();
            await _emailService.SendEmailAsync(verifyFirstLoginRequest.Email, "Xác thực tài khoản", body);

            await _unitOfWork.CommitTransactionAsync();

            return new { Token = token };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> UpdateStatusCompleteVerifyTask(int mode, int id)
    {
        var user = FindUserById(mode, id).Result as BaseUserEntity;
        switch (user)
        {
            case Teacher teacher:
                teacher.IsCompleteVerify = true;
                teacher.AccountStatus = (int)StatusAccount.Active;
                _unitOfWork.GetRepository<Teacher>().Update(teacher);
                break;
            case Pupil pupil:
                pupil.IsCompleteVerify = true;
                pupil.AccountStatus = (int)StatusAccount.Active;
                _unitOfWork.GetRepository<Pupil>().Update(pupil);
                break;
            case BusSupervisor busSupervisor:
                busSupervisor.IsCompleteVerify = true;
                busSupervisor.AccountStatus = (int)StatusAccount.Active;
                _unitOfWork.GetRepository<BusSupervisor>().Update(busSupervisor);
                break;
        }

        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<object> FindUserById(int mode, int id)
    {
        var modeFind = GetByMode(mode);

        var userRepositoryMapping = new Dictionary<ModeLogin, Func<int, Task<object?>>>
        {
            {
                ModeLogin.SuperAdmin,
                async id => await _unitOfWork.GetRepository<SuperAdmin>().Where(x => x.Id == id)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.SchoolAdmin,
                async id => await _unitOfWork.GetRepository<School>().Where(x => x.Id == id)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.Donnor,
                async id => await _unitOfWork.GetRepository<Pupil>().Where(x => x.Id == id)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.Teacher,
                async id => await _unitOfWork.GetRepository<Teacher>().Where(x => x.Id == id)
                    .FirstOrDefaultAsync()
            },
            {
                ModeLogin.BusSuperVisor,
                async id => await _unitOfWork.GetRepository<BusSupervisor>().Where(x => x.Id == id)
                    .FirstOrDefaultAsync()
            }
        };

        if (userRepositoryMapping.TryGetValue(modeFind, out var findUserFunc))
        {
            var user = await findUserFunc(id);

            if (user == null)
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "attribute", "người dùng" }
                };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate,
                    placeholders));
            }

            return user;
        }

        throw new ArgumentOutOfRangeException(nameof(modeFind), "Invalid mode value");
    }

    public async Task<bool> LogoutAsync(string accessToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var refreshToken = httpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException("Tokens are missing");
        }

        var newBlacklist = new Blacklist
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpireDate = DateTime.Now.AddDays(7)
        };

        await _unitOfWork.GetRepository<Blacklist>().Add(newBlacklist);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public Task<UserDTO> UpdateAsync(UserDTO userDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> DeleteAsync(UserDTO userDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    private ModeLogin GetByMode(int mode)
    {
        return mode switch
        {
            0 => ModeLogin.SuperAdmin,
            1 => ModeLogin.SchoolAdmin,
            2 => ModeLogin.Donnor,
            3 => ModeLogin.Teacher,
            4 => ModeLogin.BusSuperVisor,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Invalid mode value")
        };
    }

    private async Task<object> HandleLoginAsync<T>(LoginRequest loginRequest) where T : BaseUserEntity
    {
        var user = await _unitOfWork.GetRepository<T>()
            .Where(x => x.Email == loginRequest.Email || x.Username == loginRequest.Email)
            .FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.ShortRoleName)
        };

        var token = JWTHelper.CreateToken(authClaims, _configuration);
        var refreshToken = JWTHelper.GenerateRefreshToken();
        SetRefreshTokenCookie(refreshToken);
        var userDto = new
        {
            user.Email,
            Token = token,
            RefreshToken = refreshToken,
            AuthClaims = authClaims,
            Role = user.ShortRoleName,
            User = user
        };

        return userDto;
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        httpContext.Response.Cookies.Append("refresh_token", refreshToken, cookieOptions);
    }

    public async Task<bool> ChangePassword(ChangePasswordDTO model, int mode, int userId)
    {
        var user = await GetUserByModeAndIdAsync(mode, userId);
        if (user == null)
        {
            throw new NotFoundException("Không tìm thấy người dùng");
        }

        if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
        {
            return false;
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        await _unitOfWork.BeginTransactionAsync();

        switch (user)
        {
            case SuperAdmin:
                await _unitOfWork.GetRepository<SuperAdmin>().Update((SuperAdmin)user);
                break;
            case School:
                await _unitOfWork.GetRepository<School>().Update((School)user);
                break;
            case Pupil:
                await _unitOfWork.GetRepository<Pupil>().Update((Pupil)user);
                break;
            case Teacher:
                await _unitOfWork.GetRepository<Teacher>().Update((Teacher)user);
                break;
            case BusSupervisor:
                await _unitOfWork.GetRepository<BusSupervisor>().Update((BusSupervisor)user);
                break;
            default:
                throw new InvalidOperationException("Unsupported user type");
        }

        await _unitOfWork.CommitTransactionAsync();
        await _unitOfWork.CompleteAsync();

        return true;
    }

    private async Task<BaseUserEntity?> GetUserByModeAndIdAsync(int mode, int userId)
    {
        return mode switch
        {
            (int)ModeLogin.SuperAdmin => await _unitOfWork.GetRepository<SuperAdmin>().Where(x => x.Id == userId).FirstOrDefaultAsync(),
            (int)ModeLogin.SchoolAdmin => await _unitOfWork.GetRepository<School>().Where(x => x.Id == userId).FirstOrDefaultAsync(),
            (int)ModeLogin.Donnor => await _unitOfWork.GetRepository<Pupil>().Where(x => x.Id == userId).FirstOrDefaultAsync(),
            (int)ModeLogin.Teacher => await _unitOfWork.GetRepository<Teacher>().Where(x => x.Id == userId).FirstOrDefaultAsync(),
            (int)ModeLogin.BusSuperVisor => await _unitOfWork.GetRepository<BusSupervisor>().Where(x => x.Id == userId).FirstOrDefaultAsync(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Invalid mode value")
        };
    }




}