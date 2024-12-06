using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class BusRouteController : BaseApiController<BusRoute>
    {
        private readonly IBusRouteService _busRouteService;
        private readonly IUriService _uriService;
        public BusRouteController(ILogger<BusRoute> logger, IBusRouteService busRouteService, IUriService uriService) : base(logger)
        {
            _busRouteService = busRouteService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-bus-route")]
        public async Task<IActionResult> GetAllBusRoute([FromQuery] PaginationFilter filters, int? status, string? keyword)
        {
            var route = Request.Path.Value;
            int schoolId = (int)SchoolId;
            var busRoutes = await _busRouteService.GetListBusRoute(filters, _uriService, route, status, keyword, schoolId);

            return HandleSuccess(busRoutes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("detail-bus-route/{id}")]
        public async Task<IActionResult> GetDetailGetBusRoute(int id)
        {
            var busRoute = await _busRouteService.GetBusRoute(id);

            return HandleSuccess(busRoute);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-bus-route")]
        public async Task<IActionResult> CreateBusRoute([FromBody] CreateBusRouteDto busRouteDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);

            int schoolId = (int)SchoolId; 
            var result = await _busRouteService.CreateBusRoute(busRouteDto, schoolId);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-bus-route/{id}")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] CreateBusRouteDto busRouteDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);
            int schoolId = (int)SchoolId;
            var result = await _busRouteService.UpdateBusRoute(id, busRouteDto, schoolId);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-bus-route/{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var result = await _busRouteService.DeleteBusRoute(id);

            return HandleSuccess(result) ;
        }
    }
}
