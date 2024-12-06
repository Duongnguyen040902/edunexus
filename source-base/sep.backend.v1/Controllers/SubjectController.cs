using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{
    public class SubjectController : BaseApiController<SubjectController>
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ILogger<SubjectController> logger, ISubjectService subjectService)
            : base(logger)
        {
            _subjectService = subjectService;
        }

        [HttpGet("get-all-subject")]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAll((int)SchoolId);

            return HandleSuccess(subjects);
        }

        [HttpGet("get-subject-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _subjectService.GetSubjectById(id);

            return HandleSuccess(subject);
        }

        [HttpPost("create-subject")]
        public async Task<IActionResult> Create([FromBody] SubjectDTO subjectDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var subject = await _subjectService.CreateSubject(subjectDTO, (int)SchoolId);

            return HandleSuccess(subject);
        }

        [HttpPut("update-subject/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SubjectDTO subjectDTO)
        {
            if (!ModelState.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }

            var subject = await _subjectService.UpdateSubject(id, subjectDTO);

            return HandleSuccess(subject);
        }

        [HttpDelete("delete-subject/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subjectService.DeleteSubject(id, (int)SchoolId);

            return HandleSuccess(result);
        }
    }
}