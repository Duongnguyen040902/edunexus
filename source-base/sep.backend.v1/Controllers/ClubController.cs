using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class ClubController : BaseApiController<ClubController>
    {
        private readonly IClubService _clubService;
        private readonly ISemesterService _semesterService;
        private readonly IUriService _uriService;
        public ClubController(
            IClubService clubService,
            ISemesterService semesterService,
            IUriService uriService,
            ILogger<ClubController> logger) : base(logger)
        {
            _clubService = clubService;
            _semesterService = semesterService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-assign-club")]
        public async Task<IActionResult> GetAssignClubAsync()
        {
            int teacherId = (int)TeacherId;
            var clubs = await _clubService.GetAssignClubAsync(teacherId);

            return HandleSuccess(clubs);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClubPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-current-club-detail/{clubId}")]
        public async Task<IActionResult> GetCurrentClubAsync(int clubId)
        {
            int schoolId = (int)SchoolId;
            var semester = await _semesterService.GetCurrentSemester(schoolId);
            var club = await _clubService.GetClubAsync(clubId, semester.Id);

            return HandleSuccess(club);
        }

        [Authorize(Roles = "DN")]
        [HttpGet("get-enrolled-club")]
        public async Task<IActionResult> GetEnrolledClubAsync()
        {
            int pupilId = (int)PupilId;
            var clubs = await _clubService.GetEnrolledClubAsync(pupilId);

            return HandleSuccess(clubs);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-clubs-by-school-id")]
        public async Task<IActionResult> GetClubsBySchoolId([FromQuery] PaginationFilter filters)
        {
            var route = Request.Path.Value;
            var clubs = await _clubService.GetClubsBySchoolId(filters, _uriService, route, (int)SchoolId);

            return Ok(clubs);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN,SA")]
        [HttpGet("get-club-detail")]
        public async Task<IActionResult> GetClubDetail(int clubId)
        {
            var club = await _clubService.GetClubDetail(clubId);

            return HandleSuccess(club);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all-clubs")]
        public async Task<IActionResult> GetAllClubs([FromQuery] PaginationFilter filters, int? status, string? keyword)
        {
            var route = Request.Path.Value;
            int schoolId = (int)SchoolId;
            var clubs = await _clubService.GetListClubs(filters, _uriService, route, status, keyword, schoolId);

            return HandleSuccess(clubs);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-club")]
        public async Task<IActionResult> CreateClub([FromBody] CreateClubDTO clubDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);

            int schoolId = (int)SchoolId;
            var result = await _clubService.CreateClub(clubDto, schoolId);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-club/{id}")]
        public async Task<IActionResult> UpdateClub(int id, [FromBody] CreateClubDTO clubDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);

            var result = await _clubService.UpdateClub(id, clubDto);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-club/{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var result = await _clubService.DeleteClub(id);

            return HandleSuccess(result);
        }

    }
}
