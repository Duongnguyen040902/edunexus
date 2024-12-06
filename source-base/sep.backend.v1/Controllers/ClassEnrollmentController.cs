using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class ClassEnrollmentController : BaseApiController<ClassEnrollmentController>
    {
        private readonly IClassEnrollmentService _classEnrollmentService;
        private readonly IUriService _uriService;
        public ClassEnrollmentController(ILogger<ClassEnrollmentController> logger, IClassEnrollmentService classEnrollmentService, IUriService uriService) : base(logger)
        {
            _classEnrollmentService = classEnrollmentService;
            _uriService = uriService;
        }

        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToClass([FromBody] AssignTeacherRequest request)
        {
            int schoolId = (int)SchoolId;
            var result = await _classEnrollmentService.AssignTeacherToClass(request, schoolId);

            return HandleSuccess(result);
        }


        [HttpPut("update-assign-teacher")]
        public async Task<IActionResult> UpdateAssignTeacherToClass([FromBody] UpdateAssignTeacherRequest request)
        {
            var result = await _classEnrollmentService.UpdateAssignTeacherToClass(request);

            return HandleSuccess(result);
        }

        [HttpPost("assign-pupil")]
        public async Task<IActionResult> AssignStudentToClass([FromBody] AssignPupilRequest[] request)
        {
            int schoolId = (int)SchoolId;
            var result = await _classEnrollmentService.AssignPupilToClass(request, schoolId);

            return HandleSuccess(result);
        }

        [HttpDelete("remove-teacher/{classId}/{semesterId}/{teacherId}")]
        public async Task<IActionResult> RemoveTeacherFromClass(int classId, int semesterId, int teacherId)
        {
            var result = await _classEnrollmentService.RemoveTeacherFromClass(classId, semesterId, teacherId);

            return HandleSuccess(result);
        }

        [HttpDelete("remove-pupil/{classId}/{semesterId}/{pupilId}")]
        public async Task<IActionResult> RemovePupilFromClass(int classId, int semesterId, int pupilId)
        {
            var result = await _classEnrollmentService.RemovePupilFromClass(classId, semesterId, pupilId);

            return HandleSuccess(result);
        }

        [HttpGet("get-teacher-by-classId-semesterId")]
        public async Task<IActionResult> getClassId(int classId, int semesterId)
        {
            var result = await _classEnrollmentService.GetTeacherId(classId, semesterId);

            return HandleSuccess(result);
        }

        [HttpGet("swap-teacher-in-class")]
        public async Task<IActionResult> SwapTeachersInClass(int ceTeacherId1, int ceTeacherId2)
        {
            int schoolId = (int)SchoolId;
            var teachers = await _classEnrollmentService.SwapTeachersInClass(ceTeacherId1, ceTeacherId2);

            return HandleSuccess(teachers);
        }

        [HttpGet("get-teacher-in-class")]
        public async Task<IActionResult> GetTeacherInClass(int semesterId, int ceTeacherId1)
        {
            int schoolId = (int)SchoolId;
            var teachers = await _classEnrollmentService.GetTeacherInClass(semesterId, schoolId, ceTeacherId1);

            return HandleSuccess(teachers);
        }

        [HttpGet("get-member-in-class-enrollment")]
        public async Task<IActionResult> GetMemberInClass([FromQuery] PaginationFilter filters,
                string? keyword, int semesterId, int classId)
        {
            int schoolId = (int)SchoolId;
            var route = Request.Path.Value;
            var teachers = await _classEnrollmentService.GetMemberInClass(filters, _uriService, route, keyword,semesterId, classId);

            return HandleSuccess(teachers);
        }

        [HttpGet("get-member-to-copy")]
        public async Task<IActionResult> GetMemberToCopy( int nextSemesterId)
        {
            int schoolId = (int)SchoolId;
            var route = Request.Path.Value;
            var teachers = await _classEnrollmentService.GetMemberToCopy(nextSemesterId);

            return HandleSuccess(teachers);
        }

        [HttpDelete("remove-member/{id}")]
        public async Task<IActionResult> RemoveMemberClass(int id)
        {
            var result = await _classEnrollmentService.RemoveMemberClass(id);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("assign-member-to-class")]
        public async Task<IActionResult> AssignMemberToClass([FromBody] AssignMemberToClass[] request)
        {
            int schoolId = (int)SchoolId;
            var result = await _classEnrollmentService.AssignMemberToClass(request);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("pupils-to-graduate")]
        public async Task<IActionResult> PupilsToGraduate([FromBody] int[] pupilIds)
        {
            int schoolId = (int)SchoolId;
            var result = await _classEnrollmentService.PupilsToGraduate(pupilIds);

            return HandleSuccess(result);
        }
    }
}
