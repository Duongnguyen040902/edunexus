using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolYearController : BaseApiController<SchoolYearController>
    {
        private readonly ISchoolYearService _service;
        private readonly IValidator<CreateAndUpdateSchoolYearDTO> _validator;
        private readonly IUriService _uriService;
        public SchoolYearController(ILogger<SchoolYearController> logger, ISchoolYearService service, IValidator<CreateAndUpdateSchoolYearDTO> validator, IUriService uriService) : base(logger)
        {
            _service = service;
            _validator = validator;
            _uriService = uriService;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-schoolyear-by-schoolid")]
        public async Task<IActionResult> GetSchoolYearsBySchoolId([FromQuery] PaginationFilter paginationFilter)
        {
            var route = Request.Path.Value;
            var schoolYears = await _service.GetSchoolYearsBySchoolId(paginationFilter, _uriService, route, (int)SchoolId);

            return Ok(schoolYears);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("{id}/detail-schoolyear")]
        public async Task<IActionResult> GetSchoolYear(int id)
        {
            var schoolYear = await _service.GetSchoolYear(id);

            return HandleSuccess(schoolYear);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-schoolyear")]
        public async Task<IActionResult> CreateSchoolYear([FromBody]  CreateAndUpdateSchoolYearDTO createSchoolYearDTO)
        {
            var validationResult = await _validator.ValidateAsync(createSchoolYearDTO);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _service.CreateSchoolYear(createSchoolYearDTO, (int)SchoolId);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-schoolyear")]
        public async Task<IActionResult> UpdateSchoolYear([FromBody] CreateAndUpdateSchoolYearDTO updateSchoolYearDTO)
        {
            var validationResult = await _validator.ValidateAsync(updateSchoolYearDTO);
            if(!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _service.UpdateSchoolYear(updateSchoolYearDTO, (int)SchoolId);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("{id}/delete-schoolyear")]
        public async Task<IActionResult> DeleteSchoolYear(int id)
        {
            var result = await _service.DeleteSchoolYear(id);

            return HandleSuccess(result);
        }
    }
}
