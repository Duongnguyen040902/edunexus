using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;
using System.Data;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : BaseApiController<TeachersController>
    {
        private readonly ITeacherService _teacherService;
        private readonly IUriService _uriService;
        private readonly IValidator<CreateTeacherDTO> _validatorCreate;
        private readonly IValidator<UpdateTeacherDTO> _validatorUpdate;
        private readonly IValidator<UploadExcelRequest> _validatorUploadExcelFile;
        public TeachersController(ILogger<TeachersController> logger, ITeacherService teacherService, IUriService uriService, IValidator<CreateTeacherDTO> validatorCreate, IValidator<UpdateTeacherDTO> validatorUpdate, IValidator<UploadExcelRequest> validatorUploadExcelFile) : base(logger)
        {
            _teacherService = teacherService;
            _uriService = uriService;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
            _validatorUploadExcelFile = validatorUploadExcelFile;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTeacherAsync([FromForm] CreateTeacherDTO teacherDto)
        {
            var result = await _validatorCreate.ValidateAsync(teacherDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            await _teacherService.CreateTeacherAccountAsync((int)SchoolId, teacherDto);
            return HandleSuccess("Create teacher Successful");
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetListTeachersAsync([FromQuery] PaginationFilter filter, [FromQuery] int? subjectId = null, [FromQuery] int? accountStatus = null, string? searchKey = null)
        {
            var route = Request.Path.Value;
            var teachers = await _teacherService.GetListTeacherOfSchoolAsync(filter, _uriService, route, (int)SchoolId, subjectId, accountStatus, searchKey);

            return Ok(teachers);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-detail")]
        public async Task<IActionResult> GetTeacherByIdAsync(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherDetailAsync(teacherId);

            return HandleSuccess(teacher);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update/{teacherId}")]
        public async Task<IActionResult> UpdateTeacherAsync(int teacherId, [FromForm] UpdateTeacherDTO updateTeacherDto)
        {

            var result = await _validatorUpdate.ValidateAsync(updateTeacherDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            await _teacherService.UpdateTeacherAsync(teacherId, updateTeacherDto);

            return HandleSuccess("Update teacher Successful");
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-multiple-teachers")]
        public async Task<IActionResult> DeleteMultipleTeacherAsync([FromBody] int[] ids)
        {
            var result = await _teacherService.DeleteMultipleTeacherAsync(ids);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("import-teachers")]
        public async Task<IActionResult> ImportTeachers([FromForm] UploadExcelRequest uploadExcelFileRequest)
        {

            var result = await _validatorUploadExcelFile.ValidateAsync(uploadExcelFileRequest);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            try
            {
                await _teacherService.ImportExcelToCreateTeacherAsync((int)SchoolId, uploadExcelFileRequest);
                return HandleSuccess("Teachers imported successfully.");
            }
            catch (ExcelRowProcessingException ex)
            {
                var errorDetails = await ex.GetErrorDetailsAsync();
                return StatusCode(errorDetails.StatusCode, errorDetails);
            }
        }

    }
}
