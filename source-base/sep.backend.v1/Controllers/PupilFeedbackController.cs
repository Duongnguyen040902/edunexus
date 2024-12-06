using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class PupilFeedbackController : BaseApiController<PupilFeedbackController>
    {
        IPupilFeedbackService _pupilFeedbackService;
        ISemesterService _semesterService;
        IValidator<PupilFeedbackDTO> _validator;
        IValidator<RequestGetPupilFeedbackDTO> _validatorDelete;
        public PupilFeedbackController(ILogger<PupilFeedbackController> logger, IPupilFeedbackService pupilFeedbackService, ISemesterService semesterService, IValidator<PupilFeedbackDTO> validator, IValidator<RequestGetPupilFeedbackDTO> validatorDelete) : base(logger)
        {
            _pupilFeedbackService = pupilFeedbackService;
            _semesterService = semesterService;
            _validator = validator;
            _validatorDelete = validatorDelete;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClassPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-list-class-feedback")]
        public async Task<IActionResult> GetListClassFeedback(int classId)
        {
            var semester = await _semesterService.GetCurrentSemester((int)SchoolId);
            var classes = await _pupilFeedbackService.GetPupilFeedbackOfClass(classId, semester.Id);

            return HandleSuccess(classes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPost("create-pupil-feedback")]
        public async Task<IActionResult> CreatePupilFeedback([FromBody] PupilFeedbackDTO model)
        {
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _pupilFeedbackService.CreatePupilFeedback(model);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPut("update-pupil-feedback")]
        public async Task<IActionResult> UpdatePupilFeedback([FromBody] PupilFeedbackDTO model)
        {
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _pupilFeedbackService.UpdatePupilFeedback(model);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpDelete("delete-pupil-feedback")]
        public async Task<IActionResult> DeletePupilFeedback(RequestGetPupilFeedbackDTO model)
        {
            var validationResult = _validatorDelete.Validate(model);
            if (!validationResult.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            var result = await _pupilFeedbackService.DeletePupilFeedback(model.PupilId, model.SemesterId);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-pupil-feedback")]
        public async Task<IActionResult> GetPupilFeedback()
        {
            var result = await _pupilFeedbackService.GetPupilFeedbacks((int)PupilId);

            return HandleSuccess(result);
        }
    }
}
