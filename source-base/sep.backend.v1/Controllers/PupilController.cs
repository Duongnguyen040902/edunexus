using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class PupilController : BaseApiController<PupilController>
    {
        private readonly IPupilService _pupilService;
        private readonly IValidator<UpdateProfilePupilDTO> _validator;
        public PupilController(ILogger<PupilController> logger, IPupilService pupilService, IValidator<UpdateProfilePupilDTO> validator) : base(logger)
        {
            _pupilService = pupilService;
            _validator = validator;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-pupil-assign")]
        public async Task<IActionResult> GetPupilAssign(int semesterId)
        {
            int schoolId = (int)SchoolId;
            var pupils = await _pupilService.GetPupilAssignToClass(semesterId,schoolId);

            return HandleSuccess(pupils);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet]
        public async Task<IActionResult> GetProfilePupilAsync()
        {
            var profilePupil = await _pupilService.GetProfilePupilAsync((int)PupilId);
            return HandleSuccess(profilePupil);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfilePupilAsync([FromForm] UpdateProfilePupilDTO profilePupilDto)
        {
            var result = await _validator.ValidateAsync(profilePupilDto);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var update = await _pupilService.UpdateProfilePupilAsync(profilePupilDto);

            return HandleSuccess(update);
        }
    }
}
