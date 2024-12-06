using AutoMapper;
using Hangfire.PostgreSql.Utils;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Validators;
using System.Collections;

namespace sep.backend.v1.Services
{
    public class TeacherService : BaseService<TeacherDTO, Teacher>, ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapper _mapper;

        public TeacherService(IUnitOfWork unitOfWork, IAutoMapper mapper, IWebHostEnvironment environment) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateTeacherAccountAsync(int schoolId, CreateTeacherDTO teacherDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentSubscriptionOfSchool = await _unitOfWork.SchoolSubscriptionRepository.GetCurrentSubscriptionOfSchoolAsync(schoolId);

                if (currentSubscriptionOfSchool is null)
                {
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói đăng ký của trường"));
                }

                int accountLimit = currentSubscriptionOfSchool.SubscriptionPlan.MaxActiveAccounts;


                await CheckAccountLimitAsync(schoolId, accountLimit);

                string username = await GenerateUsernameAsync(teacherDto);

                var teacherEntity = _mapper.ReverseMap<Teacher, CreateTeacherDTO>(teacherDto);

                teacherEntity.Username = username;
                teacherEntity.SchoolId = schoolId;
                teacherEntity.Image = DefaultImagePaths.DefaultAvatar;

                await _unitOfWork.GetRepository<Teacher>().Add(teacherEntity);

                await _unitOfWork.CompleteAsync();


                if (teacherDto.SubjectIds != null && teacherDto.SubjectIds.Any())
                {
                    await AssignSubjectsToTeacherAsync(teacherEntity.Id, teacherDto.SubjectIds);
                }
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<PagedResponse<List<TeacherDetailDTO>>> GetListTeacherOfSchoolAsync(
        PaginationFilter filters,
        IUriService uriService,
        string route,
        int schoolId,
        int? subjectId = null,
        int? accountStatus = null,
        string? searchKey = null)
        {

            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var teachers = await _unitOfWork.TeacherRepository.GetListTeacherOfSchoolAsync(
                schoolId, subjectId, accountStatus, searchKey);

            var teacherDTOs = teachers.Select(teacher => _mapper.Map<Teacher, TeacherDetailDTO>(teacher)).ToList();
            var totalRecords = teachers.Count();

            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                teacherDTOs.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route);

            return pagedResponse;

        }


        public async Task<TeacherDetailDTO> GetTeacherDetailAsync(int teacherId)
        {
            var teacher = await _unitOfWork.TeacherRepository.GetTeacherDetailAsync(teacherId);

            if (teacher is null)
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Giáo viên"));

            var teacherDetailDTO = _mapper.Map<Teacher, TeacherDetailDTO>(teacher);

            return teacherDetailDTO;
        }

        public async Task<bool> UpdateTeacherAsync(int teacherId, UpdateTeacherDTO updateTeacherDto)
        {

            await _unitOfWork.BeginTransactionAsync();
            var teacher = await _unitOfWork.TeacherRepository.GetTeacherDetailAsync(teacherId);
            try
            {

                if (teacher is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Giáo viên"));


                if (updateTeacherDto.AccountStatus == (int)Statuses.Active && teacher.Email is null)
                {
                    throw new ConflictException(Messages.VERIFY_ACCOUNT);
                }


                _mapper.Map<UpdateTeacherDTO, Teacher>(updateTeacherDto, teacher);

                await _unitOfWork.GetRepository<Teacher>().Update(teacher);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var currentSubjects = teacher.TeacherSubjects.Select(ts => ts.SubjectId).ToList();
                var newSubjectIds = updateTeacherDto.SubjectIds;

                var subjectsToRemove = currentSubjects.Except(newSubjectIds).ToList();
                var subjectsToAdd = newSubjectIds.Except(currentSubjects).ToList();

                if (subjectsToRemove.Any())
                {
                    foreach (var subjectId in subjectsToRemove)
                    {
                        _unitOfWork.GetRepository<TeacherSubject>().DeleteMulti(x => x.SubjectId == subjectId && x.TeacherId == teacherId);
                    }
                }

                if (subjectsToAdd.Any())
                {
                    var addTeacherSubjects = subjectsToAdd.Select(subjectId => new TeacherSubject
                    {
                        TeacherId = teacherId,
                        SubjectId = subjectId
                    }).ToList();
                    await _unitOfWork.GetRepository<TeacherSubject>().BulkInsert(addTeacherSubjects);
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<List<TeacherDetailDTO>> GetTeacherAssignToClass(int semesterId, int schoolId)
        {
            var teachers = await _unitOfWork.TeacherRepository.GetTeachersWithoutClassesAsync(semesterId, schoolId);
            if (teachers.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            return _mapper.Map<List<Teacher>, List<TeacherDetailDTO>>(teachers);
        }

        public async Task<ProfileTeacherDTO> GetProfileTeacherAsync(int id)
        {
            var teacherProfile = await _unitOfWork.TeacherRepository.GetTeacherProfile(id);
            if (teacherProfile == null)
            {
                throw new NotFoundException("Không tìm thấy hồ sơ");
            }
            var teacherProfileDTO = _mapper.Map<Teacher, ProfileTeacherDTO>(teacherProfile);

            return teacherProfileDTO;
        }

        public async Task<bool> UpdateProfileTeacherAsync(UpdateProfileTeacherDTO profileTeacherDto)
        {
            var teacherEntity = await _unitOfWork.TeacherRepository.GetById(profileTeacherDto.Id);
            teacherEntity.FirstName = profileTeacherDto.FirstName;
            teacherEntity.LastName = profileTeacherDto.LastName;
            teacherEntity.DateOfBirth = profileTeacherDto.DateOfBirth;
            teacherEntity.Gender = profileTeacherDto.Gender;
            teacherEntity.Address = profileTeacherDto.Address;
            teacherEntity.PhoneNumber = profileTeacherDto.PhoneNumber;
            if (profileTeacherDto.Image != null)
            {
                var fileUploadHelper = new FileUploadHelper();
                var filePath = await fileUploadHelper.UploadFile(profileTeacherDto.Image, "TeacherImage");
                teacherEntity.Image = filePath;
            }
            teacherEntity.UpdatedDate = DateTime.Now;

            var updateResult = await _unitOfWork.TeacherRepository.Update(teacherEntity);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<bool> DeleteMultipleTeacherAsync(IEnumerable<int> teacherId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var teachers = _unitOfWork.GetRepository<Teacher>()
                    .Where(t => teacherId.Contains(t.Id))
                    .Include(t => t.ClassEnrollments)
                    .Include(t => t.ClubEnrollments)
                    .ToList();

                if (!teachers.Any())
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Giáo viên"));

                var hasRelations = teachers.Any(t => t.ClassEnrollments != null && t.ClassEnrollments.Any()
                                                   || t.ClubEnrollments != null && t.ClubEnrollments.Any());
                if (hasRelations)
                    throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT,
                        "Không thể xóa giáo viên vì có thể có một số giáo viên đã được phân công hoạt động!"));

                var teacherSubjectsToDelete = _unitOfWork.GetRepository<TeacherSubject>()
                .Where(ts => teacherId.Contains(ts.TeacherId))
                .ToList();

                if (teacherSubjectsToDelete.Any())
                {
                    await _unitOfWork.GetRepository<TeacherSubject>().RemoveRange(teacherSubjectsToDelete);
                }

                await _unitOfWork.GetRepository<Teacher>().RemoveRange(teachers);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateTeacherAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            
            var filePath = await UploadFileExcelAsync(uploadExcelFileRequest);

            var allSubjects = await GetAllSubjectsAsync(schoolId);
            var subjectNameToId = MapSubjectNameToId(allSubjects);

            var (teacherList, validationErrors) = await ProcessExcelFileAsync(filePath, subjectNameToId);

            if (validationErrors.Any())
            {
                var errorFilePath = await WriteValidationErrorsToFileAsync(validationErrors);
                throw new ExcelRowProcessingException(-1, "Xác thực dữ liệu không thành công đối với một hoặc nhiều hàng trong tệp tin.", errorFilePath);
            }


            await SaveTeachersAsync(schoolId, teacherList);

            return (true, null);
        }



        private async Task CheckAccountLimitAsync(int schoolId, int accountLimit)
        {
            int totalAccounts = await _unitOfWork.GetRepository<Teacher>().Count(t => t.SchoolId == schoolId && t.AccountStatus == (int)StatusAccount.Active || t.AccountStatus == (int)StatusAccount.Inactive) +
                                await _unitOfWork.GetRepository<Pupil>().Count(t => t.SchoolId == schoolId && t.AccountStatus == (int)StatusAccount.Active || t.AccountStatus == (int)StatusAccount.Inactive) +
                                await _unitOfWork.GetRepository<BusSupervisor>().Count(t => t.SchoolId == schoolId && t.AccountStatus == (int)StatusAccount.Active || t.AccountStatus == (int)StatusAccount.Inactive);

            if (totalAccounts >= accountLimit)
            {
                throw new CustomException(Messages.ACCOUNT_LIMIT);
            }
        }
        private async Task<string> GenerateUsernameAsync(CreateTeacherDTO teacherDto)
        {
            return await UsernameGeneratorHelper.GenerateUsernameAsync<Teacher>(_unitOfWork, teacherDto.FirstName, teacherDto.LastName);
        }

        private async Task<string> UploadFileExcelAsync(UploadExcelRequest uploadExcelFileRequest)
        {
            if (uploadExcelFileRequest.File != null && uploadExcelFileRequest.File.Length > 0)
            {
                var fileHelper = new FileUploadHelper();
                return await fileHelper.UploadExcelFile(uploadExcelFileRequest.File, "excel-teacher");
            }
            return null;
        }
        private async Task AssignSubjectsToTeacherAsync(int teacherId, List<int> subjectIds)
        {
            foreach (var subjectId in subjectIds)
            {
                var teacherSubject = new TeacherSubject
                {
                    TeacherId = teacherId,
                    SubjectId = subjectId
                };
                await _unitOfWork.GetRepository<TeacherSubject>().Add(teacherSubject);
            }
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<SubjectDTO>> GetAllSubjectsAsync(int schoolId)
        {
            var subjects = await _unitOfWork.GetRepository<Subject>()
                .GetMulti(x => x.SchoolId == schoolId);
            if (!subjects.Any())
            {
                return null;
            }

            return subjects.Select(t => _mapper.Map<Subject, SubjectDTO>(t)).ToList();
        }

        private Dictionary<string, int> MapSubjectNameToId(IEnumerable<SubjectDTO> subjects)
        {
            if (subjects == null)
            {
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Danh sách môn học"));
            }
            return subjects.ToDictionary(s => s.Name.ToLower(), s => s.Id);
        }

        private async Task<(List<CreateTeacherDTO> teacherList, List<string> validationErrors)> ProcessExcelFileAsync(
            string filePath,
            Dictionary<string, int> subjectNameToId
            )
        {
            var teacherList = new List<CreateTeacherDTO>();
            var validationErrors = new List<string>();
            var validator = new TeacherValidator();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var teacherDto = CreateTeacherDTOFromRow(worksheet, row, subjectNameToId);
                    var validationResult = await validator.ValidateAsync(teacherDto);

                    if (!validationResult.IsValid)
                    {
                        validationErrors.AddRange(validationResult.Errors.Select(error => $"Dòng {row}: {error.ErrorMessage}"));
                    }
                    else
                    {
                        teacherList.Add(teacherDto);
                    }
                }
            }

            return (teacherList, validationErrors);
        }

        private CreateTeacherDTO CreateTeacherDTOFromRow(
            ExcelWorksheet worksheet,
            int row,
            Dictionary<string, int> subjectNameToId
            )
        {
            var genderText = worksheet.Cells[row, 3].Text.ToLower();
            bool? gender = genderText switch
            {
                "nam" => true,
                "nữ" => false,
                _ => null
            };

            var subjectText = worksheet.Cells[row, 7].Text;
            var subjectIds = subjectText
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(s => subjectNameToId.ContainsKey(s.Trim().ToLower()) ? subjectNameToId[s.Trim().ToLower()] : 0)
                .Where(id => id != 0)
                .ToList();

            return new CreateTeacherDTO
            {
                LastName = worksheet.Cells[row, 1].Text,
                FirstName = worksheet.Cells[row, 2].Text,              
                Gender = gender,
                DateOfBirth = DateTime.TryParse(worksheet.Cells[row, 4].Text, out var dob) ? dob : null,
                PhoneNumber = worksheet.Cells[row, 5].Text,
                Address = worksheet.Cells[row, 6].Text,
                SubjectIds = subjectIds
            };
        }

        private async Task<string> WriteValidationErrorsToFileAsync(List<string> validationErrors)
        {
            var errorFilePath = Path.Combine(Path.GetTempPath(), "error.txt");
            await File.WriteAllLinesAsync(errorFilePath, validationErrors);
            return errorFilePath;
        }

        private async Task SaveTeachersAsync(int schoolId, List<CreateTeacherDTO> teacherList)
        {
            foreach (var teacherDto in teacherList)
            {
                await CreateTeacherAccountAsync(schoolId, teacherDto);
            }
        }

    }

}
