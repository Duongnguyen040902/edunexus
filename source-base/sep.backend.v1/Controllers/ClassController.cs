using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Controllers
{

    public class ClassController : BaseApiController<ClassController>
    {
        private readonly IClassService _classService;
        private readonly ITeacherService _teacherService;
        private readonly ISemesterService _semesterService;
        private readonly IValidator<AddClassDTO> _validator;
        private readonly IValidator<UpdateClassDTO> _validatorUpdate;
        private readonly IUriService _uriService;
        public ClassController(IClassService classService,
                               ITeacherService teacherService,
                               ISemesterService semesterService,
                               ILogger<ClassController> logger,
                                IUriService uriService,
            IValidator<UpdateClassDTO> validatorUpdate,
            IValidator<AddClassDTO> validator)
            : base(logger)
        {
            _classService = classService;
            _teacherService = teacherService;
            _semesterService = semesterService;
            _uriService = uriService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-assigned-classes")]
        public async Task<IActionResult> GetAssignClassesAsync()
        {
            var classes = await _classService.GetAssignClassAsync((int)TeacherId);

            return HandleSuccess(classes);
        }
        
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClassPolicy")]
        [Authorize(Roles = "TC")]
        [HttpGet("get-class-detail/{classId}/{semesterId}")]
        public async Task<IActionResult> GetClassAsync(int classId, int semesterId)
        {
            var classDetail = await _classService.GetClassAsync(classId, semesterId);

            return HandleSuccess(classDetail);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-list-class")]
        public async Task<IActionResult> GetListClass([FromQuery] PaginationFilter filter, string? keyword, int? status)
        {
            int schoolId = (int)SchoolId;
            var route = Request.Path.Value;
            var classes = await _classService.GetAllClass(filter, _uriService, route, schoolId, keyword, status);

            return HandleSuccess(classes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPost("add-classes")]
        public async Task<IActionResult> AddNewClass([FromBody] AddClassDTO addClassDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);

            int schoolId = (int)SchoolId;
            var classes = _classService.AddNewClass(addClassDto, schoolId);

            return HandleSuccess(classes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpPut("update-class")]
        public async Task<IActionResult> UpdateClass([FromBody] UpdateClassDTO updateClassDto)
        {
            if (!ModelState.IsValid)
                return HandleModelStateErrors(ModelState);

            int schoolId = (int)SchoolId;
            var updatedClass = await _classService.UpdateClass(updateClassDto, schoolId);

            return HandleSuccess(updatedClass);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpDelete("delete-class/{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var result = await _classService.DeleteClass(id);

            return HandleSuccess(result);
        }

        [HttpGet("get-enrollment-class")]
        public async Task<IActionResult> GetEnrollmentClassAsync()
        {
            var classes = await _classService.GetEnrollmentClassAsync((int)PupilId);

            return HandleSuccess(classes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-classes-of-pupil")]
        public async Task<IActionResult> GetClassesByPupilIdAsync()
        {
            var classes = await _classService.GetClassesByPupilIdAsync((int)PupilId);

            return HandleSuccess(classes);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "DN")]
        [HttpGet("get-pupil-class/{classId}/{semesterId}")]
        public async Task<IActionResult> GetPupilClassAsync(int classId, int semesterId)
        {
            var classDetail = await _classService.GetClassAsync(classId, semesterId);

            return HandleSuccess(classDetail);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "SA")]
        [HttpGet("get-class-detail-for-school-admin/{classId}")]
        public async Task<IActionResult> GetClassDetailForSchoolAdmin(int classId)
        {
            var classDetail = await _classService.GetClassDetailByID(classId);

            return HandleSuccess(classDetail);
        }
    }
}
