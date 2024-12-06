using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{

    public class BusController : BaseApiController<BusController>
    {
        private readonly IBusService _busService;
        private readonly ISemesterService _semesterService;
        private readonly IUriService _uriService;
        public BusController(ILogger<BusController> logger, IBusService busService, ISemesterService semesterService,IUriService uriService) : base(logger)
        {
            _busService = busService;
            _semesterService = semesterService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "BSV")]
        [HttpGet("get-assigned-bus")]
        public async Task<IActionResult> GetAssignedBus()
        {
            var bus = await _busService.GetAssignedBus((int)BusSupervisorId);

            return HandleSuccess(bus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-enrollment-bus")]
        public async Task<IActionResult> GetEnrollmentBus()
        {
            var bus = await _busService.GetEnrollmentBus((int)PupilId);

            return HandleSuccess(bus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "BusPolicy")]
        [Authorize(Roles = "BSV")]
        [HttpGet("get-bus-detail")]
        public async Task<IActionResult> GetBusDetail(int busId)
        {
            var semester = await _semesterService.GetCurrentSemester((int)SchoolId);
            var bus = await _busService.GetBusDetail(busId, semester.Id);

            return HandleSuccess(bus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all-bus")]
        public async Task<IActionResult> GetAllBuses([FromQuery] PaginationFilter filters, int? status, string? keyword, int busRouteId)
        {
            var route = Request.Path.Value;
            int schoolId = (int)SchoolId;
            var buses = await _busService.GetListBus(filters, _uriService, route, status, keyword, schoolId, busRouteId);

            return Ok(buses);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-bus-for-admin-school/{busId}")]
        public async Task<IActionResult> GetBusForDetail(int busId)
        {
            var bus = await _busService.GetBusDetail(busId);

            return HandleSuccess(bus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-bus")]
        public async Task<IActionResult> CreateBus([FromBody] CreateBusDto busDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var createdBus = await _busService.CreateBus(busDto);

            return HandleSuccess(createdBus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-bus/{id}")]
        public async Task<IActionResult> UpdateBus(int id, [FromBody] CreateBusDto busDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var updatedBus = await _busService.UpdateBus(id, busDto);

            return HandleSuccess(updatedBus);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-bus/{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var deleted = await _busService.DeleteBus(id);

            return HandleSuccess(deleted);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-list-bus-of-pupil")]
        public async Task<IActionResult> GetListBusOfPupil()
        {
            var result = await _busService.GetViewBusEnrolls((int)PupilId);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-bus-detail-of-pupil/{busId}/{semesterId}")]
        public async Task<IActionResult> GetBusDetailOfPupil(int busId, int semesterId)
        {
            var bus = await _busService.GetBusDetail(busId, semesterId);

            return HandleSuccess(bus);
        }
    }
}
