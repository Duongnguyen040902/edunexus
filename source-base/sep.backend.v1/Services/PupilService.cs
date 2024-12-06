using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Excel;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Validators;

namespace sep.backend.v1.Services
{
    public class PupilService : BaseService<PupilDTO, Pupil>, IPupilService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapper _mapper;

        public PupilService(IUnitOfWork unitOfWork, IAutoMapper mapper, IWebHostEnvironment environment) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreatePupilAccountAsync(int schoolId, CreatePupilDTO pupilDto)
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

                string username = await GenerateUsernameAsync(pupilDto);

                var pupilEntity = _mapper.ReverseMap<Pupil, CreatePupilDTO>(pupilDto);
                pupilEntity.Username = username;
                pupilEntity.SchoolId = schoolId;
                pupilEntity.Image = DefaultImagePaths.DefaultAvatar;

                await _unitOfWork.GetRepository<Pupil>().Add(pupilEntity);

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

        public async Task<PagedResponse<List<PupilDetailDTO>>> GetListPupilOfSchoolAsync(PaginationFilter filters, IUriService uriService, string route, int schoolId, int? accountStatus = null, string? searchKey = null)
        {

            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pupils = await _unitOfWork.PupilRepository.GetListPupilOfSchoolAsync(schoolId, accountStatus, searchKey);

            var pupilDTOs = pupils.Select(pupils => _mapper.Map<Pupil, PupilDetailDTO>(pupils)).ToList();
            var totalRecords = pupils.Count();
            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                pupilDTOs.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }

        public async Task<PupilDetailDTO> GetPupilDetailAsync(int pupilId)
        {

            var pupil = await _unitOfWork.PupilRepository.GetPupilDetailAsync(pupilId);

            if (pupil is null)
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Học sinh"));      

            var pupilDetailDTO = _mapper.Map<Pupil, PupilDetailDTO>(pupil);

            return pupilDetailDTO;

        }

        public async Task<bool> UpdatePupilAsync(int pupilId, UpdatePupilDTO updatePupilDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var pupil = await _unitOfWork.GetRepository<Pupil>().GetById(pupilId);
                if (pupil is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Học sinh"));

                if (updatePupilDto.AccountStatus == (int)Statuses.Active && pupil.Email is null)
                {
                    throw new CustomException(Messages.VERIFY_ACCOUNT);
                }

                _mapper.Map<UpdatePupilDTO, Pupil>(updatePupilDto, pupil);

                await _unitOfWork.GetRepository<Pupil>().Update(pupil);

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

        public async Task<ProfilePupilDTO> GetProfilePupilAsync(int id)
        {
            var pupilProfile = await _unitOfWork.PupilRepository.GetPupilProfile(id);
            if (pupilProfile == null)
            {
                throw new NotFoundException("Không tìm thấy hồ sơ");
            }
            var pupilProfileDTO = _mapper.Map<Pupil, ProfilePupilDTO>(pupilProfile);

            return pupilProfileDTO;
        }

        public async Task<bool> UpdateProfilePupilAsync(UpdateProfilePupilDTO profilePupilDto)
        {
            var pupilEntity = await _unitOfWork.PupilRepository.GetById(profilePupilDto.Id);
            pupilEntity.FirstName = profilePupilDto.FirstName;
            pupilEntity.LastName = profilePupilDto.LastName;
            pupilEntity.Gender = profilePupilDto.Gender;
            pupilEntity.DonorName = profilePupilDto.DonorName;
            pupilEntity.DonorPhoneNumber = profilePupilDto.DonorPhoneNumber;
            pupilEntity.Address = profilePupilDto.Address;
            pupilEntity.DateOfBirth = profilePupilDto.DateOfBirth;
            if (profilePupilDto.Image != null)
            {
                var fileUploadHelper = new FileUploadHelper();
                var filePath = await fileUploadHelper.UploadFile(profilePupilDto.Image, "PupilImage");
                pupilEntity.Image = filePath;
            }
            pupilEntity.UpdatedDate = DateTime.Now;

            var updateResult = await _unitOfWork.PupilRepository.Update(pupilEntity);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<List<PupilDetailDTO>> GetPupilAssignToClass(int semesterId,int schoolId)
        {
            var pupils = await _unitOfWork.PupilRepository.GetPupilWithoutClassesAsync(semesterId, schoolId);
            if (pupils.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            return _mapper.Map<List<Pupil>, List<PupilDetailDTO>>(pupils);
        }

        public async Task<bool> DeleteMultiplePupilAsync(IEnumerable<int> pupilId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var pupils = _unitOfWork.GetRepository<Pupil>()
                    .Where(t => pupilId.Contains(t.Id))
                    .Include(t => t.PupilClasses)
                    .Include(t => t.ClubEnrollments)
                    .Include(t => t.BusEnrollments)
                    .Include(t => t.AttendanceRecords)
                    .Include(t => t.PupilFeedbacks)
                    .Include(t => t.PupilScores)
                    .Include(t => t.ClassApplications)
                    .ToList();

                if (!pupils.Any())
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Học sinh"));

                var hasRelations = pupils.Any(p => p.PupilClasses != null && p.PupilClasses.Any() 
                                                || p.ClubEnrollments != null && p.ClubEnrollments.Any()
                                                || p.BusEnrollments != null && p.BusEnrollments.Any()
                                                || p.AttendanceRecords != null && p.AttendanceRecords.Any()
                                                || p.PupilFeedbacks != null && p.PupilFeedbacks.Any()
                                                || p.PupilScores != null && p.PupilScores.Any()
                                                || p.ClassApplications != null && p.ClassApplications.Any());

                if (hasRelations)
                    throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT,
                        "Không thể xóa học sinh vì có thể có một số học sinh đã được phân công hoạt động!"));


                await _unitOfWork.GetRepository<Pupil>().RemoveRange(pupils);
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
        public async Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreatePupilAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

             
            var filePath = await UploadFileExcelAsync(uploadExcelFileRequest);


            var (pupilList, validationErrors) = await ProcessExcelFileAsync(filePath);

            if (validationErrors.Any())
            {
                var errorFilePath = await WriteValidationErrorsToFileAsync(validationErrors);
                throw new ExcelRowProcessingException(-1, "Xác thực dữ liệu không thành công đối với một hoặc nhiều hàng trong tệp tin.", errorFilePath);
            }


            await SavePupilsAsync(schoolId, pupilList);

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

        private async Task<string> GenerateUsernameAsync(CreatePupilDTO pupilDto)
        {
            return await UsernameGeneratorHelper.GenerateUsernameAsync<Pupil>(_unitOfWork, pupilDto.FirstName, pupilDto.LastName);
        }

        private async Task<string> UploadFileExcelAsync(UploadExcelRequest uploadExcelFileRequest)
        {
            if (uploadExcelFileRequest.File != null && uploadExcelFileRequest.File.Length > 0)
            {
                var fileHelper = new FileUploadHelper();
                return await fileHelper.UploadExcelFile(uploadExcelFileRequest.File, "excel-pupil");
            }
            return null;
        }


        private async Task<(List<CreatePupilDTO> pupilList, List<string> validationErrors)> ProcessExcelFileAsync(
            string filePath)
        {
            var pupilList = new List<CreatePupilDTO>();
            var validationErrors = new List<string>();
            var validator = new PupilValidator();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var pupilDto = CreatePupilDTOFromRow(worksheet, row);
                    var validationResult = await validator.ValidateAsync(pupilDto);

                    if (!validationResult.IsValid)
                    {
                        validationErrors.AddRange(validationResult.Errors.Select(error => $"Dòng {row}: {error.ErrorMessage}"));
                    }
                    else
                    {
                        pupilList.Add(pupilDto);
                    }
                }
            }

            return (pupilList, validationErrors);
        }

        private CreatePupilDTO CreatePupilDTOFromRow(
            ExcelWorksheet worksheet,
            int row
            )
        {
            var genderText = worksheet.Cells[row, 3].Text.ToLower();
            bool? gender = genderText switch
            {
                "nam" => true,
                "nữ" => false,
                _ => null
            };

            return new CreatePupilDTO
            {
                LastName = worksheet.Cells[row, 1].Text,
                FirstName = worksheet.Cells[row, 2].Text,
                Gender = gender,
                DateOfBirth = DateTime.TryParse(worksheet.Cells[row, 4].Text, out var dob) ? dob : null,
                DonorName = worksheet.Cells[row, 5].Text,
                DonorPhoneNumber = worksheet.Cells[row, 6].Text,
                Address = worksheet.Cells[row, 7].Text
            };
        }

        private async Task<string> WriteValidationErrorsToFileAsync(List<string> validationErrors)
        {
            var errorFilePath = Path.Combine(Path.GetTempPath(), "error.txt");
            await File.WriteAllLinesAsync(errorFilePath, validationErrors);
            return errorFilePath;
        }

        private async Task SavePupilsAsync(int schoolId, List<CreatePupilDTO> pupilList)
        {
            foreach (var teacherDto in pupilList)
            {
                await CreatePupilAccountAsync(schoolId, teacherDto);
            }
        }

    }
}
