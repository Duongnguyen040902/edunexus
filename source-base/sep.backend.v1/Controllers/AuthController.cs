using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Helpers;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Auth;
using sep.backend.v1.Services.IServices;
using System.Security.Claims;
using AutoMapper.Configuration.Conventions;
using sep.backend.v1.DTOs;
using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.Controllers
{
    public class AuthController : BaseApiController<AuthController>
    {
        private readonly IAuthService _authService;
        private readonly IUriService _uriService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger,
            IAuthService authService,
            IUriService uriService,
            IConfiguration configuration,
            IEmailService emailService)
            : base(logger)
        {
            _authService = authService;
            _uriService = uriService;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleModelStateErrors(ModelState);
                }

                var userEntity = await _authService.LoginByModeAsync(loginRequest);

                return HandleSuccess(userEntity);
            }
            catch (UnauthorizedAccessException e)
            {
                return HandleUnauthorized(e.Message);
            }
        }

        [HttpPost("register-school-admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDTORemake registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var verificationCode = RandomCodeHelper.GenerateVerificationCode();
            var registerClaims = GenerateClaimsRegister(registerDTO, verificationCode);
            var token = JWTHelper.CreateToken(registerClaims, _configuration, tokenValidityInSeconds: 1000);
            var body = Helpers.TemplateHelper.GetEmailTemplate("verify.html");
            body = body.Replace("{{username}}", registerDTO.Email)
                .Replace("{{title_header}}", "Xác Thực Tài Khoản")
                .Replace("{{code}}", verificationCode)
                .Replace("{{title}}", "Xác Thực Tài Khoản")
                .Replace("{{content}}", "Để đăng nhập vào tài khoản của bạn, vui lòng xác thực tài khoản")
                .Replace("{{content_footer}}", "Vui lòng nhập mã này vào trang đăng nhập để truy cập tài khoản của bạn. Nếu bạn không yêu cầu mã này hoặc không cố gắng truy cập tài khoản, hãy liên hệ với chúng tôi ngay để được hỗ trợ.");
            await _emailService.SendEmailAsync(registerDTO.Email, "Xác thực tài khoản",
                body);

            var response = new RegisterResponse
            {
                Message = "Send code success",
                Token = token
            };

            return HandleSuccess(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest verifyEmailRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleModelStateErrors(ModelState);
                }

                var principal = JWTHelper.GetPrincipalFromExpiredToken(verifyEmailRequest.Token, _configuration);
                var storedCode = principal?.Claims.FirstOrDefault(c => c.Type == "VerificationCode")?.Value;
                var isStoreCodeVerify =
                    IsStoreCodeVerify(verifyEmailRequest.Token, verifyEmailRequest.VerificationCode);
                if (!isStoreCodeVerify)
                {
                    return HandleBadRequest(Messages.CONFIRM_CODE_INVALID, "verifyCode");
                }

                var registerDTO = CreateRegisterDTOFromClaims(principal);
                var userEntity = await _authService.RegisterAdminSchool(registerDTO);

                return HandleSuccess(userEntity);
            }
            catch (SecurityTokenException ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleModelStateErrors(ModelState);
                }

                var userEntity =
                    await _authService.FindUserByMode(forgotPasswordRequest.Mode, forgotPasswordRequest.Email);
                var verificationCode = RandomCodeHelper.GenerateVerificationCode();
                var claimsForForgotPassword = new Dictionary<string, string>
                {
                    { "Mode", forgotPasswordRequest.Mode.ToString() },
                    { "Email", forgotPasswordRequest.Email },
                    { "VerificationCode", verificationCode }
                };

                var claims = JWTHelper.GenerateClaims(claimsForForgotPassword);
                var token = JWTHelper.CreateToken(claims, _configuration, tokenValidityInSeconds: 1000);
                var body = Helpers.TemplateHelper.GetEmailTemplate("verify.html");
                body = body.Replace("{{username}}", "")
                    .Replace("{{title_header}}", "Quên Mật Khẩu")
                    .Replace("{{code}}", verificationCode)
                    .Replace("{{title}}", "Xác Thực Tài Khoản")
                    .Replace("{{content}}", "Để cung cấp lại mật khẩu cho tài khoản của bạn, vui lòng xác thực tài khoản")
                    .Replace("{{content_footer}}", "Vui lòng nhập mã này vào trang quên mật khẩu để truy cập tài khoản của bạn. Nếu bạn không yêu cầu mã này hoặc không cố gắng truy cập tài khoản, hãy liên hệ với chúng tôi ngay để được hỗ trợ.");
                await _emailService.SendEmailAsync(forgotPasswordRequest.Email, "Quên mật khẩu",body);

                var response = new
                {
                    Message = "Send code success",
                    Token = token
                };

                return HandleSuccess(response);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost("confirm-code-reset")]
        public async Task<IActionResult> ConfirmCodeReset([FromBody] VerifyEmailRequest confirmCodeResetRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return HandleModelStateErrors(ModelState);
                }

                var isStoreCodeVerify = IsStoreCodeVerify(confirmCodeResetRequest.Token,
                    confirmCodeResetRequest.VerificationCode);
                if (!isStoreCodeVerify)
                {
                    return HandleBadRequest(Messages.CONFIRM_CODE_INVALID, "verifyCode");
                }

                var email = JWTHelper.GetPrincipalFromExpiredToken(confirmCodeResetRequest.Token, _configuration)
                    ?.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
                var mode = JWTHelper.GetPrincipalFromExpiredToken(confirmCodeResetRequest.Token, _configuration)?.Claims
                    .FirstOrDefault(c => c.Type == "Mode")?.Value;
                var claims = new List<Claim>
                {
                    new Claim("Email", email),
                    new Claim("Mode", mode)
                };
                var token = JWTHelper.CreateToken(claims, _configuration, tokenValidityInSeconds: 1000);

                var response = new
                {
                    Message = "Confirm success",
                    Token = token
                };

                return HandleSuccess(response);
            }
            catch (SecurityTokenException ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var email = JWTHelper.GetPrincipalFromExpiredToken(resetPasswordRequest.Token, _configuration)?.Claims
                .FirstOrDefault(c => c.Type == "Email")?.Value;
            var mode = JWTHelper.GetPrincipalFromExpiredToken(resetPasswordRequest.Token, _configuration)?.Claims
                .FirstOrDefault(c => c.Type == "Mode")?.Value;

            _authService.ResetPasswordByMode(int.Parse(mode), email, resetPasswordRequest.Password);

            return HandleSuccess("Reset password success");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            throw new NotImplementedException();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string accessToken)
        {
            var isLogoutSuccess = await _authService.LogoutAsync(accessToken);

            return HandleSuccess(isLogoutSuccess);
        }

        [HttpPost("verify-first-login")]
        public async Task<IActionResult> VerifyFirstLogin([FromBody] VerifyFirstLoginRequest verifyFirstLoginRequest)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var userEntity = await _authService.UpdateFirstLoginAsync(verifyFirstLoginRequest);

            return HandleSuccess(userEntity);
        }

        [HttpPost("confirm-code-first-login")]
        public async Task<IActionResult> ConfirmCodeFirstLogin([FromBody] VerifyEmailRequest verifyEmailRequest)
        {
            var isValidToken = JWTHelper.IsTokenValid(verifyEmailRequest.Token, _configuration);
            if (!isValidToken)
            {
                return HandleBadRequest(Messages.INVALID, "token");
            }

            var isStoreCodeVerify = IsStoreCodeVerify(verifyEmailRequest.Token,
                verifyEmailRequest.VerificationCode);

            if (!isStoreCodeVerify)
            {
                return HandleBadRequest(Messages.CONFIRM_CODE_INVALID, "VerificationCode");
            }

            var mode = JWTHelper.GetPrincipalFromExpiredToken(verifyEmailRequest.Token, _configuration)?.Claims
                .FirstOrDefault(c => c.Type == "Mode")?.Value;
            var id = JWTHelper.GetPrincipalFromExpiredToken(verifyEmailRequest.Token, _configuration)?.Claims
                .FirstOrDefault(c => c.Type == "Id")?.Value;

            await _authService.UpdateStatusCompleteVerifyTask(int.Parse(mode), int.Parse(id));

            return HandleSuccess(verifyEmailRequest);
        }

        private List<Claim> GenerateClaimsRegister(RegisterDTORemake registerDTO, string verificationCode)
        {
            return new List<Claim>
            {
                new Claim("Username", registerDTO.Username),
                new Claim("Name", registerDTO.Name),
                new Claim("Email", registerDTO.Email),
                new Claim("VerificationCode", verificationCode),
                new Claim("Password", registerDTO.Password),
            };
        }

        private RegisterDTO CreateRegisterDTOFromClaims(ClaimsPrincipal principal)
        {
            return new RegisterDTO
            {
                Username = principal?.Claims.FirstOrDefault(c => c.Type == "Username")?.Value,
                Name = principal?.Claims.FirstOrDefault(c => c.Type == "Name")?.Value,
                Email = principal?.Claims.FirstOrDefault(c => c.Type == "Email")?.Value,
                Password = principal?.Claims.FirstOrDefault(c => c.Type == "Password")?.Value
            };
        }

        private bool IsStoreCodeVerify(string token, string verificationCode)
        {
            var principal = JWTHelper.GetPrincipalFromExpiredToken(token, _configuration);
            var storedCode = principal?.Claims.FirstOrDefault(c => c.Type == "VerificationCode")?.Value;

            if (verificationCode != storedCode)
            {
                return false;
            }

            return true;
        }
    }
}