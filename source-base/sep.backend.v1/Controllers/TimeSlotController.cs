using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class TimeSlotController : BaseApiController<TimeSlotController>
    {
        private readonly ITimeSlotService _timeSlotService;
        private readonly IUriService _uriService;
        public TimeSlotController(ILogger<TimeSlotController> logger,
            ITimeSlotService timeSlotService,
            IUriService uriService) : base(logger)
        {
            _timeSlotService = timeSlotService;
            _uriService = uriService;
        }

        [HttpGet("get-all-timeslot")]
        public async Task<IActionResult> GetAll()
        {
            var timeSlots = await _timeSlotService.GetAll((int)SchoolId);

            return HandleSuccess(timeSlots);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all-timeslots")]
        public async Task<IActionResult> GetAllTimeSlots([FromQuery] PaginationFilter filters, bool? isActive, string? keyword)
        {
            var route = Request.Path.Value;
            var schoolId = (int)SchoolId;
            var timeSlots = await _timeSlotService.GetAllTimeSlots(filters, _uriService, route, isActive, keyword, schoolId);
            return Ok(timeSlots);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-timeslot-detail/{id}")]
        public async Task<IActionResult> GetTimeSlotDetail(int id)
        {
            var timeSlot = await _timeSlotService.GetTimeSlotDetail(id);
            return HandleSuccess(timeSlot);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-timeslot")]
        public async Task<IActionResult> CreateTimeSlot([FromBody] CreateTimeSlotDTO timeSlotDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            int schoolId = (int)SchoolId;
            var createdTimeSlot = await _timeSlotService.CreateTimeSlot(schoolId,timeSlotDto);
            return HandleSuccess(createdTimeSlot);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-timeslot/{id}")]
        public async Task<IActionResult> UpdateTimeSlot(int id, [FromBody] CreateTimeSlotDTO timeSlotDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            int schoolId = (int)SchoolId;
            var updatedTimeSlot = await _timeSlotService.UpdateTimeSlot(id, timeSlotDto, schoolId);
            return HandleSuccess(updatedTimeSlot);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-timeslot/{id}")]
        public async Task<IActionResult> DeleteTimeSlot(int id)
        {
            var deleted = await _timeSlotService.DeleteTimeSlot(id);
            return HandleSuccess(deleted);
        }
    }
}

