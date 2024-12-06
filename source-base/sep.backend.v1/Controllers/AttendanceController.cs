using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{

    public class AttendanceController : BaseApiController<AttendanceController>
    {
        private readonly IAttendanceRecordService _attendanceRecordService;
        private readonly IValidator<AttendanceRecordDTO> _validator;

        public AttendanceController(IAttendanceRecordService attendanceRecordService, IValidator<AttendanceRecordDTO> validator, ILogger<AttendanceController> logger) : base(logger)
        {
            _attendanceRecordService = attendanceRecordService;
            _validator = validator;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC, BSV")]
        [HttpGet("prepare-attendance-record")]
        public async Task<IActionResult> PrepareAttendanceRecords([FromQuery] int entityId, int session, int type, int semesterId)
        {
            var listrecord = await _attendanceRecordService.GetPupilForAttendance(entityId, session, type, semesterId);
            return HandleSuccess(listrecord);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC, BSV")]
        [HttpPost("create-attendance-record")]
        public async Task<IActionResult> CreateAttendanceRecords([FromBody] List<AttendanceRecordDTO> attendanceRecords)
        {
            foreach (var record in attendanceRecords)
            {
                var validationResult = await _validator.ValidateAsync(record);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return HandleModelStateErrors(ModelState);
                }
            }
            var result = await _attendanceRecordService.CreateAttendanceRecords(attendanceRecords);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC, BSV")]
        [HttpGet("get-attendance-record")]
        public async Task<IActionResult> GetAttendanceRecords([FromQuery] int entityId, int session, int type, int semesterId, DateTime date)
        {
            var listrecord = await _attendanceRecordService.GetAttendanceRecords(entityId, session, type, semesterId, date);
            
            return HandleSuccess(listrecord);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC, BSV")]
        [HttpPut("update-attendance-record")]
        public async Task<IActionResult> UpdateAttendanceRecords([FromBody] List<AttendanceRecordDTO> updatedRecords)
        {
            foreach (var record in updatedRecords)
            {
                var validationResult = await _validator.ValidateAsync(record);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return HandleModelStateErrors(ModelState);
                }
            }
            var result = await _attendanceRecordService.UpdateAttendanceRecords(updatedRecords);
            
            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "MultiRolePolicy")]
        [Authorize(Roles = "TC, BSV")]
        [HttpGet("get-attendance-list")]
        public async Task<IActionResult> GetAttendanceListDTOs([FromQuery] int entityId, int session, int type, int semesterId, DateTime date)
        {
            var listrecord = await _attendanceRecordService.GetAttendanceListDTOs(entityId, session, type, semesterId, date);
            
            return HandleSuccess(listrecord);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-pupil-attendance-list")]
        public async Task<IActionResult> GetPupilAttendanceList([FromQuery] int semesterId, DateTime date)
        {
            var listrecord = await _attendanceRecordService.GetPupilAttedanceList((int)PupilId, semesterId, date);

            return HandleSuccess(listrecord);
        }
    }
}
