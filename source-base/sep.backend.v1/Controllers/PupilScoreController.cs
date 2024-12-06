using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{

    [ApiController]
    public class PupilScoreController : BaseApiController<PupilsController>
    {
        IPupilScoreService _pupilScoreService;
        private readonly IValidator<PupilScoreDTO> _validator;
        public PupilScoreController(ILogger<PupilsController> logger, IPupilScoreService pupilScoreService, IValidator<PupilScoreDTO> validator) : base(logger)
        {
            _pupilScoreService = pupilScoreService;
            _validator = validator;
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClassPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-class-score")]
        public async Task<IActionResult> GetClassScore(int classId, int semesterId)
        {
            var result = await _pupilScoreService.GetPupilScoreListDTOs(classId, semesterId);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-pupil-for-create")]
        public async Task<IActionResult> GetPupilForCreate(int entityId, int semesterId, int subjectId)
        {
            var result = await _pupilScoreService.GetPupilForCreate(entityId, semesterId, subjectId);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPost("create-pupil-scores")]
        public async Task<IActionResult> CreatePupilScores([FromBody] List<PupilScoreDTO> pupilScores)
        {
            foreach (var record in pupilScores)
            {
                var validationResult = await _validator.ValidateAsync(record);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return HandleModelStateErrors(ModelState);
                }
            }
            var result = await _pupilScoreService.CreatePupilScores(pupilScores);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPut("update-pupil-scores")]
        public async Task<IActionResult> UpdatePupilScore([FromBody] List<PupilScoreDTO> pupilScores)
        {
            foreach (var record in pupilScores)
            {
                var validationResult = await _validator.ValidateAsync(record);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return HandleModelStateErrors(ModelState);
                }
            }
            var result = await _pupilScoreService.UpdatePupilScores(pupilScores);

            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-pupil-scores")]
        public async Task<IActionResult> GetPupilScores(int entityId, int semesterId, int subjectId)
        {
            var result = await _pupilScoreService.GetPupilScores(entityId, semesterId, subjectId);
            return HandleSuccess(result);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-individual-pupil-scores")]
        public async Task<IActionResult> GetIndividualPupilScores(int semesterId)
        {
            var result = await _pupilScoreService.GetIndividualPupilScores((int)PupilId, semesterId);
            return HandleSuccess(result);
        }
    }
}
