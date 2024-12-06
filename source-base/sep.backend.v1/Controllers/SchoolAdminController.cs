using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    [Authorize(Roles = "SPA")]
    public class SchoolAdminController : BaseApiController<SchoolAdminController>
    {
        private readonly ISchoolAdminService _schoolAdminService;
        private readonly IUriService _uriService;

        public SchoolAdminController(ILogger<SchoolAdminController> logger, ISchoolAdminService schoolAdminService,
            IUriService uriService) : base(logger)
        {
            _schoolAdminService = schoolAdminService;
            _uriService = uriService;
        }

        [HttpGet("all-account-school-admin")]
        public async Task<IActionResult> GetAllAccountSchoolAdmin([FromQuery] PaginationFilter filters, int? status,
            string? keyword, int? subscriptionPlanId)
        {
            var route = Request.Path.Value;

            var schools = await _schoolAdminService.GetAllAccountSchoolAdmin(filters, _uriService, route, status,
                keyword, subscriptionPlanId);

            return Ok(schools);
        }

        [HttpGet("{id}/detail-account-school-admin")]
        public async Task<IActionResult> GetDetailAccountSchoolAdmin(int id)
        {
            var accountSchool = await _schoolAdminService.GetSchoolAdminAsync(id);

            return HandleSuccess(accountSchool);
        }

        [HttpPost("create-account-school-admin")]
        public async Task<IActionResult> CreateAccountSchoolAdmin([FromBody] CreateSchoolDTO createSchoolDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var newSchool = await _schoolAdminService.CreateSchoolAdminAsync(createSchoolDTO);

            return HandleSuccess(newSchool);
        }

        [HttpPut("{id}/update-account-school-admin")]
        public async Task<IActionResult> UpdateAccountSchoolAdmin(int id, [FromBody] UpdateSchoolDTO updateSchoolDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var updatedSchool = await _schoolAdminService.UpdateSchoolAdminAsync(id, updateSchoolDTO);

            return HandleSuccess(updatedSchool);
        }

        [HttpDelete("{id}/delete-account-school-admin")]
        public async Task<IActionResult> DeleteAccountSchoolAdmin(int id) //TODO: Pending QA
        {
            throw new NotImplementedException();
        }
    }
}