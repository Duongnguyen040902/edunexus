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
using sep.backend.v1.Validators.BusSupervisor;

namespace sep.backend.v1.Services
{
    public class BusSupervisorService : BaseService<ProfileBusSupervisorDTO, BusSupervisor>, IBusSupervisorService
    {
        public BusSupervisorService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ProfileBusSupervisorDTO> GetBusSupervisorById(int id)
        {
            var busSupervisor = await _unitOfWork.GetRepository<BusSupervisor>().GetById(id);
            if (busSupervisor == null)
            {
                throw new NotFoundException("Không tìm thấy hồ sơ");
            }

            return _mapper.Map<BusSupervisor, ProfileBusSupervisorDTO>(busSupervisor);
        }

        public async Task<bool> UpdateBusSupervisor(UpdateProfileBusSupervisorDTO busSupervisorDTO)
        {
            var busSupervisor = await _unitOfWork.BusSupervisorRepository.GetById(busSupervisorDTO.Id);
            busSupervisor.FirstName = busSupervisorDTO.FirstName;
            busSupervisor.LastName = busSupervisorDTO.LastName;
            busSupervisor.PhoneNumber = busSupervisorDTO.PhoneNumber;
            busSupervisor.Address = busSupervisorDTO.Address;
            busSupervisor.Gender = busSupervisorDTO.Gender;
            if (busSupervisorDTO.Image != null)
            {
                var fileUploadHelper = new FileUploadHelper();
                var filePath = await fileUploadHelper.UploadFile(busSupervisorDTO.Image, "BusSupervisorImage");
                busSupervisor.Image = filePath;
            }
            busSupervisor.UpdatedDate = DateTime.Now;
            var updateResult = await _unitOfWork.BusSupervisorRepository.Update(busSupervisor);
            await _unitOfWork.CompleteAsync();

            return updateResult;
        }

        public async Task<bool> CreateBusSupervisorAccountAsync(int schoolId, CreateBusSupervisorDTO busSupervisorDto)
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

                string username = await GenerateUsernameAsync(busSupervisorDto);

                var busSupervisorEntity = _mapper.ReverseMap<BusSupervisor, CreateBusSupervisorDTO>(busSupervisorDto);
                busSupervisorEntity.Username = username;
                busSupervisorEntity.SchoolId = schoolId;
                busSupervisorEntity.Image = DefaultImagePaths.DefaultAvatar;

                await _unitOfWork.GetRepository<BusSupervisor>().Add(busSupervisorEntity);

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
        public async Task<PagedResponse<List<BusSupervisorAccountDetailDTO>>> GetListBusSupervisorOfSchoolAsync(PaginationFilter filters, IUriService uriService, string route, int schoolId, int? accountStatus = null, string? searchKey = null)
        {

            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var busSupervisors = await _unitOfWork.BusSupervisorRepository.GetListBusSupervisorOfSchoolAsync(schoolId, accountStatus, searchKey);

            var busSupervisorDTOs = busSupervisors.Select(busSupervisors => _mapper.Map<BusSupervisor, BusSupervisorAccountDetailDTO>(busSupervisors)).ToList();
            var totalRecords = busSupervisors.Count();
            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                busSupervisorDTOs.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }
        public async Task<BusSupervisorAccountDetailDTO> GetBusSupervisorDetailAsync(int busSupervisorId)
        {

            var busSupervisor = await _unitOfWork.GetRepository<BusSupervisor>().GetById(busSupervisorId);

            if (busSupervisor is null)
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Người phụ trách xe bus"));

            var busSupervisorDetailDTO = _mapper.Map<BusSupervisor, BusSupervisorAccountDetailDTO>(busSupervisor);

            return busSupervisorDetailDTO;

        }
        public async Task<bool> UpdateBusSupervisorAsync(int busSupervisorId, UpdateBusSupervisorDTO updateBusSupervisorDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busSupervisor = await _unitOfWork.GetRepository<BusSupervisor>().GetById(busSupervisorId);
                if (busSupervisor is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Người phụ trách xe bus"));

                if (updateBusSupervisorDto.AccountStatus == (int)Statuses.Active && busSupervisor.Email is null)
                {
                    throw new CustomException(Messages.VERIFY_ACCOUNT);
                }

                _mapper.Map<UpdateBusSupervisorDTO, BusSupervisor>(updateBusSupervisorDto, busSupervisor);

                await _unitOfWork.GetRepository<BusSupervisor>().Update(busSupervisor);

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


        public async Task<bool> DeleteMultipleBusSupervisorAsync(IEnumerable<int> busSupervisorId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busSupervisors = _unitOfWork.GetRepository<BusSupervisor>()
                    .Where(t => busSupervisorId.Contains(t.Id))
                    .Include(t => t.BusEnrollments)
                    .ToList();

                if (!busSupervisors.Any())
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Người phụ trách xe bus"));

                var hasRelations = busSupervisors.Any(p => p.BusEnrollments != null && p.BusEnrollments.Any());                                              

                if (hasRelations)
                    throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT,
                        "Không thể xóa người phụ trách bus vì có thể có một số người phụ trách bus đã được phân công hoạt động!"));


                await _unitOfWork.GetRepository<BusSupervisor>().RemoveRange(busSupervisors);
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

        public async Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateBusSupervisorAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            var filePath = await UploadFileExcelAsync(uploadExcelFileRequest);


            var (busSupervisorList, validationErrors) = await ProcessExcelFileAsync(filePath);

            if (validationErrors.Any())
            {
                var errorFilePath = await WriteValidationErrorsToFileAsync(validationErrors);
                throw new ExcelRowProcessingException(-1, "Xác thực dữ liệu không thành công đối với một hoặc nhiều hàng trong tệp tin.", errorFilePath);
            }


            await SaveBusSupervisorsAsync(schoolId, busSupervisorList);

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

        private async Task<string> GenerateUsernameAsync(CreateBusSupervisorDTO busSupervisorDto)
        {
            return await UsernameGeneratorHelper.GenerateUsernameAsync<BusSupervisor>(_unitOfWork, busSupervisorDto.FirstName, busSupervisorDto.LastName);
        }

        private async Task<string> UploadFileExcelAsync(UploadExcelRequest uploadExcelFileRequest)
        {
            if (uploadExcelFileRequest.File != null && uploadExcelFileRequest.File.Length > 0)
            {
                var fileHelper = new FileUploadHelper();
                return await fileHelper.UploadExcelFile(uploadExcelFileRequest.File, "excel-busSupervisor");
            }
            return null;
        }


        private async Task<(List<CreateBusSupervisorDTO> busSupervisorList, List<string> validationErrors)> ProcessExcelFileAsync(
            string filePath)
        {
            var busSupervisorList = new List<CreateBusSupervisorDTO>();
            var validationErrors = new List<string>();
            var validator = new BusSupervisorCreateValidator();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var busSupervisorDto = CreateBusSupervisorDTOFromRow(worksheet, row);
                    var validationResult = await validator.ValidateAsync(busSupervisorDto);

                    if (!validationResult.IsValid)
                    {
                        validationErrors.AddRange(validationResult.Errors.Select(error => $"Dòng {row}: {error.ErrorMessage}"));
                    }
                    else
                    {
                        busSupervisorList.Add(busSupervisorDto);
                    }
                }
            }

            return (busSupervisorList, validationErrors);
        }

        private CreateBusSupervisorDTO CreateBusSupervisorDTOFromRow(
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

            return new CreateBusSupervisorDTO
            {
                LastName = worksheet.Cells[row, 1].Text,
                FirstName = worksheet.Cells[row, 2].Text,        
                Gender = gender,
                PhoneNumber = worksheet.Cells[row, 4].Text,
                Address = worksheet.Cells[row, 5].Text
            };
        }

        private async Task<string> WriteValidationErrorsToFileAsync(List<string> validationErrors)
        {
            var errorFilePath = Path.Combine(Path.GetTempPath(), "error.txt");
            await File.WriteAllLinesAsync(errorFilePath, validationErrors);
            return errorFilePath;
        }

        private async Task SaveBusSupervisorsAsync(int schoolId, List<CreateBusSupervisorDTO> busSupervisorList)
        {
            foreach (var busSupervisorDto in busSupervisorList)
            {
                await CreateBusSupervisorAccountAsync(schoolId, busSupervisorDto);
            }
        }
    }
}
