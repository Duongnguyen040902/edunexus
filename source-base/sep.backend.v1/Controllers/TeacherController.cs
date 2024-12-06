
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
namespace sep.backend.v1.Controllers
{
    public class TeacherController : BaseApiController<TeacherController>
    {
        private readonly ITeacherService _teacherService;
        private readonly IValidator<UpdateProfileTeacherDTO> _validator;
        public TeacherController(ILogger<TeacherController> logger, ITeacherService teacherService, IValidator<UpdateProfileTeacherDTO> validator) : base(logger)
        {
            _teacherService = teacherService;
            _validator = validator;
        }

        [HttpGet("get-teacher-assign")]
        public async Task<IActionResult> GetTeacherAssign(int semesterId)
        {
            int schoolId = (int)SchoolId;
            var teachers = await _teacherService.GetTeacherAssignToClass(semesterId, schoolId);

            return HandleSuccess(teachers);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet]
        public async Task<IActionResult> GetProfileTeacherAsync()
        {
            var profileTeacher = await _teacherService.GetProfileTeacherAsync((int)TeacherId);
            return HandleSuccess(profileTeacher);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfileTeacherAsync([FromForm] UpdateProfileTeacherDTO profileTeacherDto)
        {
            var result = await _validator.ValidateAsync(profileTeacherDto);
            var update = await _teacherService.UpdateProfileTeacherAsync(profileTeacherDto);

            return HandleSuccess(update);
        }
    }
}
