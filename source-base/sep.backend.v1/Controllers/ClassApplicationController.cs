using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassApplicationController : BaseApiController<ClassApplicationController>
    {
        private readonly IClassApplicationService _classApplicationService;
        private readonly IValidator<CreateAndUpdateClassApplicationDTO> _validator;
        private readonly IValidator<ResponeClassApplicationDTO> _responeValidator;
        public ClassApplicationController(
            IClassApplicationService classApplicationService,
            IValidator<CreateAndUpdateClassApplicationDTO> validator,
            IValidator<ResponeClassApplicationDTO> responeValidator,
            ILogger<ClassApplicationController> logger) : base(logger)
        {
            _classApplicationService = classApplicationService;
            _validator = validator;
            _responeValidator = responeValidator;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClassPolicy")]
        [HttpGet("get-class-application")]
        public async Task<IActionResult> GetClassApplicationByClassId(int classId, int semesterId, int? categoryId)
        {
            var classApplications = await _classApplicationService.GetClassApplicationByClassId(classId, semesterId, categoryId);
            return HandleSuccess(classApplications);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpPost("create-class-application")]
        public async Task<IActionResult> CreateClassApplication(CreateAndUpdateClassApplicationDTO model)
        {
            model.PupilId = (int)PupilId;
            int schoolId = (int)SchoolId;
            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _classApplicationService.CreateClassApplication(model, schoolId);

            return HandleSuccess(success);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpPut("update-class-application")]
        public async Task<IActionResult> UpdateClassApplication(CreateAndUpdateClassApplicationDTO model)
        {
            model.PupilId = (int)PupilId;
            var result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _classApplicationService.UpdateClassApplication(model);

            return HandleSuccess(success);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPut("response-class-application")]
        public async Task<IActionResult> ResponeClassApplication(ResponeClassApplicationDTO model)
        {
            var result = await _responeValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var success = await _classApplicationService.ResponeClassApplication(model);

            return HandleSuccess(success);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-pupil-application")]
        public async Task<IActionResult> GetClassApplicationOfPupil(int semesterId)
        {
            int pupilId = (int)PupilId;
            var classApplications = await _classApplicationService.GetClassApplicationByPupilId(pupilId, semesterId);
            return HandleSuccess(classApplications);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [HttpGet("get-class-application-category")]
        public async Task<IActionResult> GetAllClassApplicationCategory()
        {
            var classApplicationCategories = await _classApplicationService.GetAllClassApplicationCategory();
            return HandleSuccess(classApplicationCategories);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpDelete("delete-class-application")]
        public async Task<IActionResult> DeleteClassApplication(int id)
        {
            var success = await _classApplicationService.DeleteClassApplication(id);
            return HandleSuccess(success);
        }
    }
}
