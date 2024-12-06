using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using System.Linq;

namespace sep.backend.v1.Services
{
    public class NotificationService : BaseService<NotificationDTO, Notifications>, INotificationService
    {
        IEmailService _mailService;
        public NotificationService(IUnitOfWork unitOfWork, IAutoMapper mapper, IEmailService mailService) : base(unitOfWork, mapper)
        {
            _mailService = mailService;
        }

        public async Task<DetailNotificationDTO> GetNotificationAsync(int id)
        {
            var notificationEntity = _unitOfWork.GetRepository<Notifications>().GetSingleByCondition(x => x.Id == id, new string[] { "Images" }).Result;
            if (notificationEntity is null)
            {
                throw new NotFoundException("Không có dữ liệu");
            }
            var notificationDto = _mapper.Map<Notifications, DetailNotificationDTO>(notificationEntity);

            return notificationDto;
        }

        public async Task<List<NotificationDTO>> GetNotifications(int classId, int schoolYearId)
        {
            var notifications = _unitOfWork.GetRepository<Notifications>().GetMulti(x => x.ClassId == classId && x.SchoolYearId == schoolYearId).Result.OrderByDescending(x => x.Id).ToList();
            if(notifications is null || !notifications.Any())
            {
                throw new NotFoundException("Không có dữ liệu");
            }
            var notificationDto = _mapper.Map<List<Notifications>, List<NotificationDTO>>(notifications);

            return notificationDto;
        }

        public async Task<bool> AddNotificationAsync(AddNotificationDTO addNotificationDTO, int schoolYearId)
        {
            var notificationEntity = _mapper.Map<AddNotificationDTO, Notifications>(addNotificationDTO);
            notificationEntity.SchoolYearId = schoolYearId;
            await _unitOfWork.GetRepository<Notifications>().Add(notificationEntity);
            await _unitOfWork.CompleteAsync();
            var LastNotificationId = _unitOfWork.GetRepository<Notifications>().All().Result.OrderByDescending(x => x.Id).FirstOrDefault();

            if (addNotificationDTO.FileImage.Any())
            {
                var fileUploadHelper = new FileUploadHelper();
                foreach (var item in addNotificationDTO.FileImage)
                {
                    var filePath = await fileUploadHelper.UploadFile(item, "NotificationImage");
                    var notiImg = new NotificationImage()
                    {
                        Url = filePath,
                        NotificationId = LastNotificationId.Id
                    };
                    await _unitOfWork.GetRepository<NotificationImage>().Add(notiImg);
                }
            }
            await _unitOfWork.CompleteAsync();
            //get list pupil
            var listPupil = await GetListPupil(addNotificationDTO.ClassId, schoolYearId);
            //lấy ra list email
            var listEmail = listPupil.Select(x => x.Email).ToList();

            var subject = $"Có một thông báo trong lớp học";
            var body = $@"Bạn vừa có một thông báo trong lớp học.
            Tiêu đề: {addNotificationDTO.Title}
            Nội dung: {addNotificationDTO.Descriptions}

            Vui lòng truy cập hệ thống để xem thông báo.

            Trân trọng,
            EduNexus";

            foreach (var pupil in listPupil)
            {
                if(pupil.Email != null)
                {
                    var message = $@"Xin chào {pupil.DonorName},
                        {body}";
                    await _mailService.SendEmailAsync(pupil.Email, subject, message);
                }
            }
            return true;
        }

        public async Task<bool> UpdateNotificationAsync(UpdateNotificationDTO updateNotificationDTO)
        {
            var notificationEntity = await _unitOfWork.GetRepository<Notifications>()
                .GetSingleByCondition(x => x.Id == updateNotificationDTO.Id, new string[] { "Images" });
            if (notificationEntity is null)
            {
                throw new NotFoundException();
            }

            notificationEntity.Title = updateNotificationDTO.Title;
            notificationEntity.Descriptions = updateNotificationDTO.Descriptions;
            notificationEntity.CategoryId = updateNotificationDTO.CategoryId;

            // Lấy danh sách URL ảnh hiện tại từ database
            var existingImages = notificationEntity.Images.ToList();
            var newImageFileNames = updateNotificationDTO.FileImage?.Select(file => file.FileName).ToList() ?? new List<string>();

            // Tìm các ảnh cần xóa (ảnh trong cơ sở dữ liệu mà không có trong danh sách ảnh mới)
            var imagesToDelete = existingImages
                .Where(img => !newImageFileNames.Any(newFileName => img.Url.Contains(newFileName)))
                .ToList();

            // Xóa các ảnh cũ không có trong danh sách mới
            foreach (var img in imagesToDelete)
            {
                _unitOfWork.GetRepository<NotificationImage>().Delete(img.Id);
            }

            // Lọc ảnh mới: Loại bỏ những ảnh đã tồn tại trong cơ sở dữ liệu khỏi danh sách file ảnh mới
            if (updateNotificationDTO.FileImage != null)
            {
                updateNotificationDTO.FileImage = updateNotificationDTO.FileImage
                    .Where(file => existingImages.All(img => !img.Url.Contains(file.FileName)))
                    .ToList();

                // Thêm các ảnh mới còn lại trong danh sách file
                var fileUploadHelper = new FileUploadHelper();
                foreach (var file in updateNotificationDTO.FileImage)
                {
                    var newImage = new NotificationImage
                    {
                        Url = await fileUploadHelper.UploadFile(file, "NotificationImage"),
                        NotificationId = notificationEntity.Id
                    };
                    await _unitOfWork.GetRepository<NotificationImage>().Add(newImage);
                }
            }

            await _unitOfWork.GetRepository<Notifications>().Update(notificationEntity);
            await _unitOfWork.CompleteAsync();

            return true;
        }




        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = _unitOfWork.GetRepository<Notifications>().GetById(id).Result;
            if (notification is null)
            {
                throw new NotFoundException();
            }
            _unitOfWork.GetRepository<NotificationImage>().DeleteMulti(x => x.NotificationId == id);
            await _unitOfWork.GetRepository<Notifications>().Delete(id);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<List<NotificationCategory>> notificationCategories()
        {
            var notificationCategories = _unitOfWork.GetRepository<NotificationCategory>().All().Result.ToList();

            return notificationCategories;
        }

        public async Task<List<Pupil>> GetListPupil(int classId, int schoolYearId)
        {
            var pupils = await _unitOfWork.GetRepository<Pupil>().GetMulti(
                p => p.PupilClasses.Any(c => c.ClassId == classId && c.Semester.SchoolYearId == schoolYearId),
                includes: new string[] { "PupilClasses", "PupilClasses.Semester" }
                );

            return pupils.ToList();
        }
    }
}