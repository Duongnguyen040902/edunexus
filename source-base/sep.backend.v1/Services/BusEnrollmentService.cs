using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sep.backend.v1.Services
{
    public class BusEnrollmentService : BaseService<BusEnrollmentDTO, BusEnrollment>, IBusEnrollmentService
    {
        private ApplicationContext _context;
        public BusEnrollmentService(IUnitOfWork unitOfWork, IAutoMapper mapper, ApplicationContext context) : base(unitOfWork, mapper)
        {
            _context = context;
        }

        public async Task<bool> CreateBusEnrollment(CreateBusEnrollmentDTO[] busEnrollmentDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busIds = busEnrollmentDto.Select(x => x.BusId).Distinct().ToArray();
                if (busIds.Length > 1 || busIds.Length == 0)
                {
                    throw new ConflictException(Responses.ConflictBus);
                }
                var bus = await _unitOfWork.BusRepository.GetSingleByCondition(x=> x.Id == busIds[0]);
                if (bus is null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "xe" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                foreach (var enrollmentDto in busEnrollmentDto)
                {
                    var existingEnrollment = await _unitOfWork.BusEnrollmentRepository.GetSingleByCondition(
                        be => be.BusId == enrollmentDto.BusId &&
                              be.SemesterId == enrollmentDto.SemesterId &&
                             ((be.PupilId == enrollmentDto.PupilId && be.BusSupervisorId == null) || 
                             (be.BusSupervisorId == enrollmentDto.BusSupervisorId && be.PupilId == null))
                    );

                    if (existingEnrollment != null)
                    {
                        throw new ConflictException(Responses.ConflictAddMemberBus);
                    }
                }
                var currentEnrollmentCount = await _unitOfWork.BusEnrollmentRepository.GetMulti(be => be.BusId == bus.Id && be.SemesterId == busEnrollmentDto[0].SemesterId);
                var seatActiveInBus = currentEnrollmentCount.Count();
                var totalEnrollments = seatActiveInBus + busEnrollmentDto.Length;
                if (totalEnrollments > bus.SeatNumber)
                {
                    var availableSeats = bus.SeatNumber - seatActiveInBus;
                    var placeholders = new Dictionary<string, string> { { "actualSeat", availableSeats.ToString() } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.ConflictAddPupilsToBus, placeholders));
                }
                await _unitOfWork.BusEnrollmentRepository.CheckEnrollmentIdsExistAsync(busEnrollmentDto);
                var newEnrollment = _mapper.Map<CreateBusEnrollmentDTO[], BusEnrollment[]>(busEnrollmentDto);

                await _context.BulkInsertAsync(newEnrollment);
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

        public async Task<CreateBusEnrollmentDTO?> UpdateBusEnrollment(int id, CreateBusEnrollmentDTO busEnrollmentDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var enrollment = await _unitOfWork.BusEnrollmentRepository.GetById(id);
                if (enrollment == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _mapper.Map(busEnrollmentDto, enrollment);
                _unitOfWork.BusEnrollmentRepository.Update(enrollment);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<BusEnrollment, CreateBusEnrollmentDTO>(enrollment);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteBusEnrollment(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var enrollment = await _unitOfWork.BusEnrollmentRepository.GetById(id);
                if (enrollment == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _unitOfWork.BusEnrollmentRepository.Delete(enrollment.Id);
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

        public async Task<BusEnrollmentDTO?> GetBusEnrollmentDetail(int id)
        {
            var enrollments = await _unitOfWork.BusEnrollmentRepository.GetMulti(
                x => x.Id == id,
                new string[] { "Bus", "Pupil", "BusSupervisor", "Semester" }
            );

            var enrollment = enrollments.FirstOrDefault();
            if (enrollment == null)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            return _mapper.Map<BusEnrollment, BusEnrollmentDTO>(enrollment);
        }

        public async Task<PagedResponse<List<BusEnrollmentDTO>>> GetListBusEnrollments(
            PaginationFilter filters,
            IUriService uriService,
            string route,
            int busId,
            int semesterId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.BusEnrollmentRepository.GetBusEnrollments(busId, semesterId);
            var totalRecords = pagedData.Count();
            var enrollmentDto = _mapper.Map<List<BusEnrollment>, List<BusEnrollmentDTO>>(pagedData);
            var sortedEnrollmentDto = enrollmentDto
                .OrderByDescending(x => x.BusSupervisorId != null) 
                .ThenBy(x => x.PupilName)
                .ToList();

            return PagedResponseHelper.CreatePagedResponse(
                sortedEnrollmentDto.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );
        }

        public async Task<List<PupilDetailDTO>> GetPupilWithoutEnrollments(int semesterId, int schoolId)
        {
            var enrolledPupils = await _unitOfWork.BusEnrollmentRepository.GetMulti(x => x.SemesterId == semesterId && x.PupilId != null);
            var allPupils = await _unitOfWork.PupilRepository.GetMulti(x => x.SchoolId == schoolId && (x.AccountStatus == (int)Statuses.Active || x.AccountStatus == (int)Statuses.Inactive));
            var pupilsWithoutEnrollments = allPupils.Where(p => !enrolledPupils.Any(ep => ep.PupilId == p.Id)).ToList();
            if (pupilsWithoutEnrollments.Count == 0)
            {
                var placeholders = new Dictionary<string, string>
            {
                { "attribute", "thông tin" }
            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            return _mapper.Map<List<Pupil>, List<PupilDetailDTO>>(pupilsWithoutEnrollments);
        }

        public async Task<List<BusSupervisorDTO>> GetBusSupervisorWithoutEnrollments(int semesterId, int schoolId)
        {
            var enrolledSupervisors = await _unitOfWork.BusEnrollmentRepository.GetMulti(x => x.SemesterId == semesterId && x.BusSupervisorId != null );
            var allSupervisors = await _unitOfWork.BusSupervisorRepository.GetMulti(x => x.SchoolId == schoolId && (x.AccountStatus == (int)Statuses.Active || x.AccountStatus == (int)Statuses.Inactive));
            var supervisorsWithoutEnrollments = allSupervisors
                .Where(s => !enrolledSupervisors.Any(es => es.BusSupervisorId == s.Id))
                .ToList();
            if (supervisorsWithoutEnrollments.Count == 0)
            {
                var placeholders = new Dictionary<string, string>
            {
                { "attribute", "thông tin" }
            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            return _mapper.Map<List<BusSupervisor>, List<BusSupervisorDTO>>(supervisorsWithoutEnrollments);
        }
        public async Task<List<PupilDetailDTO>> GetListPupilsInBusStop(int semesterId, int busStopId, int schoolId)
        {
            var enrolledPupils = await _unitOfWork.BusEnrollmentRepository.GetMulti(x => x.SemesterId == semesterId && x.BusStopId == busStopId);
            var allPupils = await _unitOfWork.PupilRepository.GetMulti(x => x.SchoolId == schoolId && (x.AccountStatus == (int)Statuses.Active || x.AccountStatus == (int)Statuses.Inactive));
            var pupilsWithoutEnrollments = allPupils.Where(p => enrolledPupils.Any(ep => ep.PupilId == p.Id)).ToList();
            if(pupilsWithoutEnrollments.Count == 0)
            {
                var placeholders = new Dictionary<string, string>
            {
                { "attribute", "thông tin" }
            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            return _mapper.Map<List<Pupil>, List<PupilDetailDTO>>(pupilsWithoutEnrollments);
        }

        public Task<(bool isSuccess, string errorFilePath)> ImportExcelToCreateMemberBusAsync(int schoolId, UploadExcelRequest uploadExcelFileRequest)
        {
            throw new NotImplementedException();
        }
        
        public async Task<List<BusEnrollmentDTO>> GetMemberToCopy(int nextSemesterId)
        {
            var enrollmentsInNextSemester = await _unitOfWork.BusEnrollmentRepository.GetMembersInNextSemester(nextSemesterId);
            if (enrollmentsInNextSemester.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            var newEnrollment = _mapper.Map<List<BusEnrollment>, List<BusEnrollmentDTO>>((List<BusEnrollment>)enrollmentsInNextSemester);

            return newEnrollment;
        }
    }
}
