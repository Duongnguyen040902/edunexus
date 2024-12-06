using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class ClubEnrollmentController : BaseApiController<ClubEnrollmentController>
    {
        public readonly IClubEnrollmentService _clubEnrollmentService;
        public readonly ISemesterService _semesterService;
        private readonly IUriService _uriService;
        public ClubEnrollmentController(
            IClubEnrollmentService clubEnrollmentService,
            ISemesterService semesterService,
            IUriService uriService,
            ILogger<ClubEnrollmentController> logger) : base(logger)
        {
            _clubEnrollmentService = clubEnrollmentService;
            _semesterService = semesterService;
            _uriService = uriService;
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpPost("pupil-create-club-enrollment")]
        public async Task<IActionResult> PupilCreateClubEnrollment([FromBody] ClubEnrollmentDTO model)
        {
            var semester = await _semesterService.GetNextSemester((int)SchoolId);
            var result = await _clubEnrollmentService.PupilCreateClubEnrollmentAsync(model, semester.Id, (int)PupilId);
            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpPut("update-club-enrollment")]
        public async Task<IActionResult> UpdateClubEnrollment([FromBody] ClubEnrollmentDTO model)
        {
            var semester = await _semesterService.GetNextSemester((int)SchoolId);
            var result = await _clubEnrollmentService.UpdateClubEnrollmentAsync(model, semester.Id, (int)PupilId);
            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-club-enrollment-by-pupil-id")]
        public async Task<IActionResult> GetClubEnrollmentByPupilId(int semesterId)
        {
            var clubEnrollments = await _clubEnrollmentService.GetClubEnrollmentByPupilIdAsync((int)PupilId, semesterId);
            return HandleSuccess(clubEnrollments);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-club-enrollment-by-semesterId")]
        public async Task<IActionResult> GetListClubEnrollments(
                [FromQuery] PaginationFilter filters,
                string? keyword,
                int clubId,
                int semesterId)
        {
            var route = Request.Path.Value;
            var result = await _clubEnrollmentService.GetListClubEnrollments(filters, _uriService, route, keyword, clubId, semesterId);
            
            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("assign-member-to-club")]
        public async Task<IActionResult> AssignMemberToClub([FromBody] AssignMemberRequest[] request)
        {
            int schoolId = (int)SchoolId;
            var result = await _clubEnrollmentService.AssignMemberToClub(request, schoolId);
           
            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-member-in-club")]
        public async Task<IActionResult> UpdateMemberInClub([FromBody] UpdateMemberRequest[] request)
        {
            var result = await _clubEnrollmentService.UpdateMemberInClub(request);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("remove-member-in-club/{id}")]
        public async Task<IActionResult> RemoveMemberFromClub(int id)
        {
            var result = await _clubEnrollmentService.RemoveMemberFromClub(id);
            
            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-teachers-not-in-club")]
        public async Task<IActionResult> GetTeachersNotInClub(int clubId, int semesterId)
        {
            var teachers = await _clubEnrollmentService.GetTeachersNotInClub(clubId, semesterId,(int)SchoolId);

            return HandleSuccess(teachers);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-pupils-not-in-club")]
        public async Task<IActionResult> GetPupilsNotInClub(int clubId, int semesterId)
        {
            var pupils = await _clubEnrollmentService.GetPupilsNotInClub(clubId, semesterId,(int)SchoolId);

            return HandleSuccess(pupils);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-pupils-register-club")]
        public async Task<IActionResult> GetPupilsRegisterClub(int clubId, int semesterId)
        {
            var pupils = await _clubEnrollmentService.GetPupilsRegisterClub(clubId, semesterId);

            return HandleSuccess(pupils);
        }

        [HttpGet("get-member-to-copy-club")]
        public async Task<IActionResult> GetMemberToCopyClub(int nextSemesterId)
        {
            int schoolId = (int)SchoolId;
            var route = Request.Path.Value;
            var teachers = await _clubEnrollmentService.GetMemberToCopy(nextSemesterId);

            return HandleSuccess(teachers);
        }
    }
}
