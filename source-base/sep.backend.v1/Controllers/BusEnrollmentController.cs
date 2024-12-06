using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class BusEnrollmentController : BaseApiController<BusEnrollmentController>
    {
        private readonly IBusEnrollmentService _busEnrollmentService;
        private readonly IUriService _uriService;
        private readonly IValidator<UploadExcelRequest> _validatorUploadExcelFile;
        public BusEnrollmentController(
            ILogger<BusEnrollmentController> logger,
            IBusEnrollmentService busEnrollmentService,
            IUriService uriService
        ) : base(logger)
        {
            _busEnrollmentService = busEnrollmentService;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all-enrollments")]
        public async Task<IActionResult> GetAllEnrollments([FromQuery] PaginationFilter filters, int busId, int semesterId)
        {
            var route = Request.Path.Value;
            var enrollments = await _busEnrollmentService.GetListBusEnrollments(filters, _uriService, route, busId, semesterId);

            return HandleSuccess(enrollments);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-pupil-without-in-bus")]
        public async Task<IActionResult> GetPupilWithoutEnrollments(int semesterId)
        {
            var enrollments = await _busEnrollmentService.GetPupilWithoutEnrollments(semesterId,(int)SchoolId);

            return HandleSuccess(enrollments);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-bus-supervisor-without-in-bus")]
        public async Task<IActionResult> GetBusSupervisorWithoutEnrollments(int semesterId)
        {
            var enrollments = await _busEnrollmentService.GetBusSupervisorWithoutEnrollments(semesterId, (int)SchoolId);

            return HandleSuccess(enrollments);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-enrollment-detail/{id}")]
        public async Task<IActionResult> GetEnrollmentDetail(int id)
        {
            var enrollment = await _busEnrollmentService.GetBusEnrollmentDetail(id);

            return HandleSuccess(enrollment);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-bus-enrollment")]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateBusEnrollmentDTO[] enrollmentDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var createdEnrollment = await _busEnrollmentService.CreateBusEnrollment(enrollmentDto);

            return HandleSuccess(createdEnrollment);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-enrollment/{id}")]
        public async Task<IActionResult> UpdateEnrollment(int id, [FromBody] CreateBusEnrollmentDTO enrollmentDto)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var updatedEnrollment = await _busEnrollmentService.UpdateBusEnrollment(id, enrollmentDto);

            return HandleSuccess(updatedEnrollment);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-enrollment/{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var deleted = await _busEnrollmentService.DeleteBusEnrollment(id);

            return HandleSuccess(deleted);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-list-pupils-in-bus-stop")]
        public async Task<IActionResult> GetListPupilsInBusStop(int semesterId, int busStopId)
        {
            var busStop = await _busEnrollmentService.GetListPupilsInBusStop(semesterId, busStopId, (int)SchoolId);

            return HandleSuccess(busStop);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("import-excel-bus")]
        public async Task<IActionResult> ImportTeachers([FromForm] UploadExcelRequest uploadExcelFileRequest)
        {

            var result = await _validatorUploadExcelFile.ValidateAsync(uploadExcelFileRequest);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            try
            {
                await _busEnrollmentService.ImportExcelToCreateMemberBusAsync((int)SchoolId, uploadExcelFileRequest);
                return HandleSuccess("Thêm thành công.");
            }
            catch (ExcelRowProcessingException ex)
            {
                var errorDetails = await ex.GetErrorDetailsAsync();
                return StatusCode(errorDetails.StatusCode, errorDetails);
            }
        }

         [HttpGet("get-member-to-copy-bus")]
        public async Task<IActionResult> GetMemberToCopyBus(int nextSemesterId)
        {
            int schoolId = (int)SchoolId;
            var route = Request.Path.Value;
            var teachers = await _busEnrollmentService.GetMemberToCopy(nextSemesterId);

            return HandleSuccess(teachers);
        }
    }
}
