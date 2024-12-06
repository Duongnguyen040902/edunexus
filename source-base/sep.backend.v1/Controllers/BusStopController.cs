using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class BusStopController : BaseApiController<BusStopController>
    {
        private readonly IBusStopService _busStopService;
        private readonly IUriService _uriService;

        public BusStopController(ILogger<BusStopController> logger, IBusStopService busStopService, IUriService uriService) : base(logger)
        {
            _busStopService = busStopService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all-bus-stops")]
        public async Task<IActionResult> GetAllBusStops([FromQuery] PaginationFilter filters, int? status, string? keyword, int busRouteId)
        {
            var route = Request.Path.Value;
            var busStops = await _busStopService.GetListBusStop(filters, _uriService, route, status, keyword, busRouteId);

            return Ok(busStops);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-bus-stop-detail/{id}")]
        public async Task<IActionResult> GetBusStopDetail(int id)
        {
            var busStop = await _busStopService.GetBusStopDetail(id);

            return HandleSuccess(busStop);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-bus-stop")]
        public async Task<IActionResult> CreateBusStop([FromBody] CreateBusStopDTO busStopDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var createdBusStop = await _busStopService.CreateBusStop(busStopDto);

            return HandleSuccess(createdBusStop);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-bus-stop/{id}")]
        public async Task<IActionResult> UpdateBusStop(int id, [FromBody] CreateBusStopDTO busStopDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var updatedBusStop = await _busStopService.UpdateBusStop(id, busStopDto);

            return HandleSuccess(updatedBusStop);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-bus-stop/{id}")]
        public async Task<IActionResult> DeleteBusStop(int id)
        {
            var deleted = await _busStopService.DeleteBusStop(id);

            return HandleSuccess(deleted);
        }
    }
}
