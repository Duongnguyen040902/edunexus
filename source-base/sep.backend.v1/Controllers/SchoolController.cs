using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class SchoolController : BaseApiController<SchoolController>
    {

        private readonly ISchoolService _schoolService;
        private readonly ISchoolDashboardService _schoolDashboardService;
        private readonly IValidator<UpdateInfoSchoolDTO> _validator;
        private readonly IAuthService _authService;
        public SchoolController(ILogger<SchoolController> logger, ISchoolService schoolService, IValidator<UpdateInfoSchoolDTO> validator, ISchoolDashboardService schoolDashboard, IAuthService authService) : base(logger)
        {
            _schoolService = schoolService;
            _validator = validator;
            _schoolDashboardService = schoolDashboard;
            _authService = authService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-info-school")]
        public async Task<IActionResult> GetInfoSchool()
        {
            int schoolId = (int)SchoolId;
            var school = await _schoolService.GetSchoolById(schoolId);

            return HandleSuccess(school);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-info-school")]
        public async Task<IActionResult> UpdateInfoSchool([FromForm] UpdateInfoSchoolDTO updateSchoolDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var updateSuccess = await _schoolService.UpdateInfoSchool(updateSchoolDTO);

            return HandleSuccess(updateSuccess);
        }

        [HttpGet("get-school-dashboard")]
        public async Task<IActionResult> GetSchoolDashboard()
        {
            int schoolId = (int)SchoolId;
            var schoolDashboard = await _schoolDashboardService.GetSchoolDashboard((int)SchoolAdminId);

            return HandleSuccess(schoolDashboard);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var userId = SchoolAdminId ?? TeacherId ?? PupilId ?? BusSupervisorId;
            if (userId == null)
            {
                return HandleBadRequest("User ID not found");
            }

            var mode = SchoolAdminId != null ? (int)ModeLogin.SchoolAdmin :
                       TeacherId != null ? (int)ModeLogin.Teacher :
                       PupilId != null ? (int)ModeLogin.Donnor :
                       (int)ModeLogin.BusSuperVisor;

            var isChangePasswordSuccess = await _authService.ChangePassword(changePasswordDTO, mode, (int)userId);

            return HandleSuccess(isChangePasswordSuccess);
        }

    }
}
