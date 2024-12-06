using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Extensions;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace sep.backend.v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseApiController<NotificationController>
    {

        private readonly INotificationService _service;
        private readonly IValidator<AddNotificationDTO> _validator;
        private readonly IValidator<UpdateNotificationDTO> _validatorUpdate;
        private readonly IUriService _uriService;
        private readonly ISemesterService _semesterService;
        public NotificationController(ILogger<NotificationController> logger, INotificationService service
            , IValidator<AddNotificationDTO> validator
            , IValidator<UpdateNotificationDTO> validatorUpdate, IUriService uriService, ISemesterService semesterService, IEmailService emailService) : base(logger)
        {
            _service = service;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
            _uriService = uriService;
            _semesterService = semesterService;
        }
        
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Policy = "ClassOfTeacherAndPupilPolicy")]
        [Authorize(Roles = "TC, DN")]
        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] int classId)
        {
            var semester = _semesterService.GetCurrentSemester((int)SchoolId).Result;
            int schoolYearId = semester.SchoolYearId;

            var notifications = await _service.GetNotifications(classId, schoolYearId);
            return HandleSuccess(notifications);

        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC, DN")]
        [HttpGet("/api/Notification/Detail")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var notification = await _service.GetNotificationAsync(id);
            return HandleSuccess(notification);
        }

        [HttpGet("/api/Notification/Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _service.notificationCategories();
            return HandleSuccess(categories);
        }

        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPost]
        public async Task<IActionResult> AddNotification([FromForm] AddNotificationDTO addNotificationDTO)
        {
            var result = await _validator.ValidateAsync(addNotificationDTO);
            if (!result.IsValid)
            {
                return HandleModelStateErrors(ModelState);
            }
            // Kiểm tra nếu danh sách FileImage null hoặc rỗng
            if (addNotificationDTO.FileImage == null || !addNotificationDTO.FileImage.Any())
            {
                addNotificationDTO.FileImage = new List<IFormFile>();
            }

            var semester = _semesterService.GetCurrentSemester((int)SchoolId).Result;
            int schoolYearId = semester.SchoolYearId;
            var notification = await _service.AddNotificationAsync(addNotificationDTO, schoolYearId);
            
            return HandleSuccess(notification);
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpPut]
        public async Task<IActionResult> UpdateNotification([FromForm] UpdateNotificationDTO updateNotificationDTO)
        {
            try
            {
                var result = await _validatorUpdate.ValidateAsync(updateNotificationDTO);
                if (!result.IsValid)
                {
                    return HandleModelStateErrors(ModelState);
                }
                var notification = await _service.UpdateNotificationAsync(updateNotificationDTO);

                return HandleSuccess(notification);
            }
            catch (NotFoundException ex)
            {
                return HandleNotFound(ex.Message);
            }
        }
        [Authorize(Policy = "SchoolPolicy")]
        [Authorize(Roles = "TC")]
        [HttpDelete]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var notification = await _service.DeleteNotificationAsync(id);

                return HandleSuccess(notification);
            }
            catch (NotFoundException ex)
            {
                return HandleNotFound(ex.Message);
            }
        }
    }
}