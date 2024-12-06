using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace sep.backend.v1.Controllers
{
    public class SemesterController : BaseApiController<SemesterController>
    {
        private readonly ISemesterService _semesterService;
        private readonly IValidator<CreateAndUpdateSemesterDTO> _validator;

        public SemesterController(ILogger<SemesterController> logger, ISemesterService semesterService, IValidator<CreateAndUpdateSemesterDTO> validator) : base(logger)
        {
            _semesterService = semesterService;
            _validator = validator;
        }

        [HttpGet("get-all-semester")]
        public async Task<IActionResult> GetAll()
        {

            var semesters = await _semesterService.GetAllAsync((int)SchoolId);

            return HandleSuccess(semesters);
        }

        [HttpGet("get-current-semester")]
        public async Task<IActionResult> GetCurrentSemester()
        {
            var semester = await _semesterService.GetCurrentSemester((int)SchoolId);

            return HandleSuccess(semester);
        }

        [HttpGet("get-next-semester")]
        public async Task<IActionResult> GetNextSemester()
        {
            var semester = await _semesterService.GetNextSemester((int)SchoolId);

            return HandleSuccess(semester);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "SchoolYearPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-semesters-by-schoolyearid")]
        public async Task<IActionResult> GetListSemestersBySchoolYearId(int schoolYearId)
        {
            var semesters = await _semesterService.GetListSemesters((int)SchoolId, schoolYearId);

            return HandleSuccess(semesters);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("detail-semester/{id}")]
        public async Task<IActionResult> GetSemester(int id)
        {
            var semester = await _semesterService.GetSemester(id);

            return HandleSuccess(semester);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("create-semester")]
        public async Task<IActionResult> Create([FromBody] CreateAndUpdateSemesterDTO semester)
        {
            var validationResult = await _validator.ValidateAsync(semester);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _semesterService.Create(semester);

            return HandleSuccess(result);

        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-semester")]
        public async Task<IActionResult> Update([FromBody] CreateAndUpdateSemesterDTO semester)
        {
            var validationResult = await _validator.ValidateAsync(semester);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _semesterService.Update(semester, (int)SchoolId);

            return HandleSuccess(result);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-semester/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _semesterService.Delete(id);

            return HandleSuccess(result);

        }
    }
}
