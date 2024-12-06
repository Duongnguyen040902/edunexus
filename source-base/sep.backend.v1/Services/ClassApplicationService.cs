using MailKit;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class ClassApplicationService : BaseService<ClassAppicationDTO, ClassApplication>, IClassApplicationService
    {
        IEmailService _mailService;
        ISemesterService _semesterService;
        IClassService _classService;
        public ClassApplicationService(IUnitOfWork unitOfWork, IAutoMapper mapper, IEmailService mailService, ISemesterService semesterService, IClassService classService) : base(unitOfWork, mapper)
        {
            _mailService = mailService;
            _semesterService = semesterService;
            _classService = classService;
        }

        public async Task<bool> CreateClassApplication(CreateAndUpdateClassApplicationDTO model, int schoolId)
        {
            var classOfPupil = await _unitOfWork.GetRepository<ClassEnrollment>().GetSingleByCondition(
                x => x.PupilId == model.PupilId && x.SemesterId == model.SemesterId,
                new string[] { "Pupil" }
            );

            if (classOfPupil == null)
            {
                throw new NotFoundException("Bạn chưa được xếp lớp trong kỳ này nên tạm thời chưa thể gửi đơn");
            }

            var classApplication = _mapper.Map<CreateAndUpdateClassApplicationDTO, ClassApplication>(model);
            classApplication.Status = (int)ClassApplicationStatus.Pending;
            await _unitOfWork.GetRepository<ClassApplication>().Add(classApplication);
            await _unitOfWork.CompleteAsync();

            var teacher = await _classService.GetCurrentTeacherOfClassAsync(classOfPupil.ClassId, classOfPupil.SemesterId);
            if (teacher != null)
            {
                var teacherReal = _mapper.Map<TeacherAccountDTO, Teacher>(teacher);
                if (teacherReal != null && !string.IsNullOrEmpty(teacherReal.Email))
                {
                    // Lấy template email từ file HTML
                    var body = Helpers.TemplateHelper.GetEmailTemplate("send-application.html");

                    // Thay thế các placeholder trong template bằng các giá trị thực tế
                    body = body.Replace("{{teacherFirstName}}", teacherReal.FirstName)
                               .Replace("{{teacherLastName}}", teacherReal.LastName)
                               .Replace("{{pupilFirstName}}", classOfPupil.Pupil.FirstName)
                               .Replace("{{pupilLastName}}", classOfPupil.Pupil.LastName)
                               .Replace("{{classApplicationTitle}}", classApplication.Title)
                               .Replace("{{classApplicationDescription}}", classApplication.Description);

                    // Tiêu đề email
                    var pupilName = $"{classOfPupil.Pupil.FirstName} {classOfPupil.Pupil.LastName}";
                    var subject = $"{pupilName} đã gửi một đơn vào lớp học";

                    // Gửi email với nội dung HTML
                    await _mailService.SendEmailAsync(teacherReal.Email, subject, body);
                }
            }

            return true;
        }






        public async Task<bool> DeleteClassApplication(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var findApplication = await _unitOfWork.GetRepository<ClassApplication>().GetSingleByCondition(x => x.Id == id);
                if (findApplication is null)
                {
                    throw new NotFoundException("Đơn không tồn tại");
                }
                await _unitOfWork.GetRepository<ClassApplication>().Delete(id);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<List<ClassApplicationCategoryDTO>> GetAllClassApplicationCategory()
        {
            var categories = await _unitOfWork.GetRepository<ClassApplicationCategory>().All();
            if (categories is null)
            {
                return null;
            }

            return _mapper.Map<List<ClassApplicationCategory>, List<ClassApplicationCategoryDTO>>(categories.OrderBy(x => x.Id).ToList());
        }

        public async Task<List<GetClassAppicationDetailDTO>> GetClassApplicationByClassId(int classId, int semesterId, int? categoryId)
        {
            var pupils = await _unitOfWork.GetRepository<Pupil>()
                .GetMulti(x => x.PupilClasses != null && x.PupilClasses.Any(pc => pc.ClassId == classId && pc.PupilId != null),
                          new string[] { "PupilClasses", "ClassApplications" });

            var classApplications = pupils
                .SelectMany(x => x.ClassApplications ?? Enumerable.Empty<ClassApplication>())
                .Where(ca => (categoryId == null || categoryId == 0 || ca.ApplicationCategoryId == categoryId)
                && ca.SemesterId == semesterId).OrderByDescending(x => x.CreatedDate).ToList();

            return classApplications.Count == 0 ? null : _mapper.Map<List<ClassApplication>, List<GetClassAppicationDetailDTO>>(classApplications);
        }


        public async Task<List<GetClassAppicationDetailDTO>> GetClassApplicationByPupilId(int pupilId, int semesterId)
        {
            var classApplications = await _unitOfWork.GetRepository<ClassApplication>()
                .GetMulti(x => x.PupilId == pupilId && x.SemesterId == semesterId, new string[] { "ClassApplicationCategory", "Pupil" });
            var orderedClassApplications = classApplications.OrderByDescending(x => x.CreatedDate).ToList();
            if (orderedClassApplications.Count != 0)
            {
                return _mapper.Map<List<ClassApplication>, List<GetClassAppicationDetailDTO>>(orderedClassApplications);
            }

            return null;
        }

        public async Task<GetClassAppicationDetailDTO> GetClassApplicationDetailById(int id)
        {
            var classApplication = await _unitOfWork.GetRepository<ClassApplication>()
                .GetSingleByCondition(x => x.Id == id
                , new string[] { "ClassApplicationCategory", "Pupil" });

            if (classApplication is null)
            {
                throw new NotFoundException("Đơn không tồn tại");
            }

            return _mapper.Map<ClassApplication, GetClassAppicationDetailDTO>(classApplication);
        }

        public async Task<bool> ResponeClassApplication(ResponeClassApplicationDTO model)
        {
            var findApplication = await _unitOfWork.GetRepository<ClassApplication>().GetSingleByCondition(x => x.Id == model.Id);
            if (findApplication is null)
            {
                throw new NotFoundException("Đơn này không tồn tại");
            }

            // Cập nhật thông tin phản hồi và trạng thái
            findApplication.Response = model.Response;
            findApplication.Status = model.Status;
            await _unitOfWork.GetRepository<ClassApplication>().Update(findApplication);
            await _unitOfWork.CompleteAsync();

            var student = await _unitOfWork.GetRepository<Pupil>().GetSingleByCondition(x => x.Id == findApplication.PupilId);
            if (student.Email != null)
            {
                var body = Helpers.TemplateHelper.GetEmailTemplate("response-application.html");

                body = body.Replace("{{studentFirstName}}", student.FirstName)
                           .Replace("{{studentLastName}}", student.LastName)
                           .Replace("{{applicationTitle}}", findApplication.Title)
                           .Replace("{{responseContent}}", findApplication.Response)
                           .Replace("{{responseStatus}}", GetResponseStatusText(findApplication.Status)); // Thêm trạng thái phản hồi

                var subject = $"Đơn '{findApplication.Title}' đã được giáo viên phản hồi";

                await _mailService.SendEmailAsync(student.Email, subject, body);
            }

            return true;
        }

        
        private string GetResponseStatusText(int status)
        {
            switch ((ResponseApplicationStatus)status)
            {
                case ResponseApplicationStatus.Approved:
                    return "Chấp thuận";
                case ResponseApplicationStatus.Rejected:
                    return "Từ chối";
                default:
                    return "Không xác định";  
            }
        }

        public async Task<bool> UpdateClassApplication(CreateAndUpdateClassApplicationDTO model)
        {
            var findApplication = await _unitOfWork.GetRepository<ClassApplication>().GetSingleByCondition(x => x.Id == model.Id);
            if (findApplication is null)
            {
                throw new NotFoundException("Đơn này không tồn tại");
            }
            _mapper.Map(model, findApplication);
            findApplication.UpdatedDate = DateTime.Now;
            findApplication.Status = (int)ClassApplicationStatus.Pending;
            await _unitOfWork.GetRepository<ClassApplication>().Update(findApplication);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
