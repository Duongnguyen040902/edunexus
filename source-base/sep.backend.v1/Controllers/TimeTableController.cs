using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class TimeTableController : BaseApiController<TimeTableController>
    {
        private readonly ITimetableService _timetableService;
        private readonly IValidator<TimeTableDTO> _validator;
        public TimeTableController(ITimetableService timetableService,
            IValidator<TimeTableDTO> validator,
            ILogger<TimeTableController> logger)
            : base(logger)
        {
            _timetableService = timetableService;
            _validator = validator;
        }

        [HttpPost("create-timetable")]
        public async Task<IActionResult> CreateTimeTableAsync([FromBody] TimeTableDTO model)
        {

            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _timetableService.CreateTimeTableAsync(model);

            return HandleSuccess(model);

        }

        [HttpPut("update-timetable")]
        public async Task<IActionResult> UpdateTimeTableAsync([FromBody] TimeTableDTO model)
        {
            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _timetableService.UpdateTimeTableAsync(model);

            return HandleSuccess("Update successful");
        }

        [HttpDelete("delete-timetable")]
        public async Task<IActionResult> DeleteTimeTablesync(TimeTableDTO model)
        {
            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _timetableService.DeleteTimeTableAsync(model);

            return HandleSuccess("Create successful");
        }

        [HttpGet("get-timetable-detail/{classId}/{semesterId}")]
        public async Task<IActionResult> GetTimeTableDetailBySemesterAsync(int classId, int semesterId)
        {
            var timetable = await _timetableService.GetTimeTableDetailAsync(classId, semesterId);

            return HandleSuccess(timetable);
        }
    }
}
