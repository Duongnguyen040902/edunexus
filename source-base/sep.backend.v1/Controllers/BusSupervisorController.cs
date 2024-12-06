using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class BusSupervisorController : BaseApiController<BusSupervisorController>
    {
        private readonly IBusSupervisorService _busSupervisorService;
        private readonly IValidator<UpdateProfileBusSupervisorDTO> _validator;
        private readonly IValidator<CreateBusSupervisorDTO> _validatorCreate;
        private readonly IValidator<UpdateBusSupervisorDTO> _validatorUpdate;
        private readonly IValidator<UploadExcelRequest> _validatorUploadExcelFile;
        private readonly IUriService _uriService;
        public BusSupervisorController(ILogger<BusSupervisorController> logger, IBusSupervisorService busSupervisorService, IValidator<UpdateProfileBusSupervisorDTO> validator, IValidator<CreateBusSupervisorDTO> validatorCreate, IValidator<UpdateBusSupervisorDTO> validatorUpdate, IValidator<UploadExcelRequest> validatorUploadExcelFile, IUriService uriService) : base(logger)
        {
            _busSupervisorService = busSupervisorService;
            _validator = validator;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
            _uriService = uriService;
            _validatorUploadExcelFile = validatorUploadExcelFile;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "BSV")]
        [HttpGet("get-bus-supervisor-detail")]
        public async Task<IActionResult> GetBusSupervisorById()
        {
            var busSupervisor = await _busSupervisorService.GetBusSupervisorById((int)BusSupervisorId);
            return HandleSuccess(busSupervisor);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "BSV")]
        [HttpPut("update-bus-supervisor")]
        public async Task<IActionResult> UpdateBusSupervisor([FromForm] UpdateProfileBusSupervisorDTO busSupervisorDTO)
        {
            var result = await _validator.ValidateAsync(busSupervisorDTO);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var update = await _busSupervisorService.UpdateBusSupervisor(busSupervisorDTO);

            return HandleSuccess(update);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBusSupervisorAsync([FromForm] CreateBusSupervisorDTO busSupervisorDto)
        {
            var result = await _validatorCreate.ValidateAsync(busSupervisorDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            await _busSupervisorService.CreateBusSupervisorAccountAsync((int)SchoolId, busSupervisorDto);
            return HandleSuccess("BusSupervisor information created successfully.");

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetListBusSupervisorsAsync([FromQuery] PaginationFilter filter, int? accountStatus = null, string? searchKey = null)
        {
            var route = Request.Path.Value;
            var busSupervisors = await _busSupervisorService.GetListBusSupervisorOfSchoolAsync(filter, _uriService, route, (int)SchoolId, accountStatus, searchKey);

            return Ok(busSupervisors);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-detail/{busSupervisorId}")]
        public async Task<IActionResult> GetBusSupervisorByIdAsync(int busSupervisorId)
        {

            var busSupervisor = await _busSupervisorService.GetBusSupervisorDetailAsync(busSupervisorId);
            return HandleSuccess(busSupervisor);

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update/{busSupervisorId}")]
        public async Task<IActionResult> UpdateBusSupervisorAsync([FromForm] UpdateBusSupervisorDTO updateBusSupervisorDto, int busSupervisorId)
        {
            var result = await _validatorUpdate.ValidateAsync(updateBusSupervisorDto);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            await _busSupervisorService.UpdateBusSupervisorAsync(busSupervisorId, updateBusSupervisorDto);
            return HandleSuccess("BusSupervisor information updated successfully.");

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-multiple-busSupervisors")]
        public async Task<IActionResult> DeleteMultipleBusSupervisorsAsync([FromBody] int[] ids)
        {
            var result = await _busSupervisorService.DeleteMultipleBusSupervisorAsync(ids);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("import-busSupervisors")]
        public async Task<IActionResult> ImportBusSupervisors([FromForm] UploadExcelRequest uploadExcelFileRequest)
        {

            var result = await _validatorUploadExcelFile.ValidateAsync(uploadExcelFileRequest);

            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            try
            {
                var (isSuccess, errorFilePath) = await _busSupervisorService.ImportExcelToCreateBusSupervisorAsync((int)SchoolId, uploadExcelFileRequest);

                return HandleSuccess("BusSupervisors imported successfully.");
            }
            catch (ExcelRowProcessingException ex)
            {
                var errorDetails = await ex.GetErrorDetailsAsync();
                return StatusCode(errorDetails.StatusCode, errorDetails);
            }
        }
    }
}
