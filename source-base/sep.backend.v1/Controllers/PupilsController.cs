using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;
using System.Data;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PupilsController : BaseApiController<PupilsController>
    {
        private readonly IPupilService _pupilService;
        private readonly IUriService _uriService;
        private readonly IValidator<CreatePupilDTO> _validatorCreate;
        private readonly IValidator<UpdatePupilDTO> _validatorUpdate;
        private readonly IValidator<UploadExcelRequest> _validatorUploadExcelFile;
        public PupilsController(ILogger<PupilsController> logger, IPupilService pupilService, IUriService uriService, IValidator<CreatePupilDTO> validatorCreate, IValidator<UpdatePupilDTO> validatorUpdate, IValidator<UploadExcelRequest> validatorUploadExcelFile) : base(logger)
        {
            _pupilService = pupilService;
            _uriService = uriService;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
            _validatorUploadExcelFile = validatorUploadExcelFile;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create")]
        public async Task<IActionResult> CreatePupilAsync([FromForm] CreatePupilDTO pupilDto)
        {
            var result = await _validatorCreate.ValidateAsync(pupilDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            await _pupilService.CreatePupilAccountAsync((int)SchoolId, pupilDto);
            return HandleSuccess("Pupil information created successfully.");

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetListPupilsAsync([FromQuery] PaginationFilter filter, int? accountStatus = null, string? searchKey = null)
        {
            var route = Request.Path.Value;
            var pupils = await _pupilService.GetListPupilOfSchoolAsync(filter, _uriService, route, (int)SchoolId, accountStatus, searchKey);

            return Ok(pupils);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-detail/{pupilId}")]
        public async Task<IActionResult> GetPupilByIdAsync(int pupilId)
        {

            var pupil = await _pupilService.GetPupilDetailAsync(pupilId);
            return HandleSuccess(pupil);

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update/{pupilId}")]
        public async Task<IActionResult> UpdatePupilAsync([FromForm] UpdatePupilDTO updatePupilDto, int pupilId)
        {
            var result = await _validatorUpdate.ValidateAsync(updatePupilDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            await _pupilService.UpdatePupilAsync(pupilId, updatePupilDto);
            return HandleSuccess("Pupil information updated successfully.");

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-multiple-pupils")]
        public async Task<IActionResult> DeleteMultiplePupilAsync([FromBody] int[] ids)
        {
            var result = await _pupilService.DeleteMultiplePupilAsync(ids);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("import-pupils")]
        public async Task<IActionResult> ImportPupils([FromForm] UploadExcelRequest uploadExcelFileRequest)
        {

            var result = await _validatorUploadExcelFile.ValidateAsync(uploadExcelFileRequest);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            try
            {
                var (isSuccess, errorFilePath) = await _pupilService.ImportExcelToCreatePupilAsync((int)SchoolId, uploadExcelFileRequest);

                return HandleSuccess("Pupils imported successfully.");
            }
            catch (ExcelRowProcessingException ex)
            {
                var errorDetails = await ex.GetErrorDetailsAsync();
                return StatusCode(errorDetails.StatusCode, errorDetails);
            }
        }
    }

}
