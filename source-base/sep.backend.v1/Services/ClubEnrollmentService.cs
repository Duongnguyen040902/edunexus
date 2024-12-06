using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper.Execution;
using sep.backend.v1.Extensions.EF;
using EFCore.BulkExtensions;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Helpers;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Common.Filters;
using System;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace sep.backend.v1.Services
{
    public class ClubEnrollmentService : BaseService<ClubEnrollment, ClubEnrollmentDTO>, IClubEnrollmentService
    {
        private ApplicationContext _context;
        public ClubEnrollmentService(IUnitOfWork unitOfWork, IAutoMapper mapper, ApplicationContext context) : base(unitOfWork, mapper)
        {
            _context = context;
        }

        public async Task<List<ClubEnrollmentDetailDTO>> GetClubEnrollmentByPupilIdAsync(int pupilId, int semesterId)
        {
            var clubIds = (await _unitOfWork.GetRepository<ClubEnrollment>()
                .GetMulti(x => x.PupilId == pupilId && x.SemesterId == semesterId))
                .Select(x => x.ClubId)
                .Distinct()
                .ToList();

            var clubEnrollments = (await _unitOfWork.GetRepository<ClubEnrollment>()
                .GetMulti(x => clubIds.Contains(x.ClubId) && x.SemesterId == semesterId, new string[] { "Club", "Teacher", "Semester", "Pupil" }))
                .OrderByDescending(x => x.ClubId)
                .ToList();

            if (!clubEnrollments.Any())
            {
                throw new NotFoundException("Không có dữ liệu");
            }

            var groupedEnrollments = clubEnrollments
                .GroupBy(e => new { e.ClubId, e.SemesterId })
                .Select(g => new ClubEnrollmentDetailDTO
                {
                    ClubId = g.Key.ClubId,
                    SemesterId = g.Key.SemesterId,
                    ClubName = g.First().Club?.Name,
                    ClubDescription = g.First().Club?.Description,
                    SemesterName = g.First().Semester?.SemesterName,
                    Teacher = _mapper.Map<Teacher, TeacherDTO >(g.FirstOrDefault(e => e.Teacher != null)?.Teacher),
                    PupilId = pupilId,
                    Status = g.FirstOrDefault(e => e.PupilId == pupilId)?.Status ?? 0
                })
                .ToList();

            return groupedEnrollments;
        }

        public async Task<bool> PupilCreateClubEnrollmentAsync(ClubEnrollmentDTO model, int semesterId, int pupilId)
        {
            var clubEnrollment = _mapper.Map<ClubEnrollmentDTO, ClubEnrollment>(model);
            clubEnrollment.Status = (int)ClubEnrollmentStatus.Register;
            clubEnrollment.SemesterId = semesterId;
            clubEnrollment.PupilId = pupilId;
            await _unitOfWork.GetRepository<ClubEnrollment>().Add(clubEnrollment);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateClubEnrollmentAsync(ClubEnrollmentDTO model, int semesterId, int pupilId)
        {
            //Tìm ra clubEnrollment cần update bằng clubId và pupilId và semesterId
            var existingEnrollment = await _unitOfWork.GetRepository<ClubEnrollment>()
            .Where(e => e.ClubId == model.ClubId && e.PupilId == pupilId && e.SemesterId== semesterId).FirstOrDefaultAsync();

            existingEnrollment.Status = (int)model.Status;
            existingEnrollment.UpdatedDate = DateTime.Now;
            await _unitOfWork.GetRepository<ClubEnrollment>().Update(existingEnrollment);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> AssignMemberToClub(AssignMemberRequest[] request, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var clubIds = request.Select(x => x.ClubId).Distinct().ToArray();
                if (clubIds.Length > 1 || clubIds.Length == 0)
                {
                    throw new ConflictException(Responses.ConflictAssignToClub);
                }
                var club = await _unitOfWork.ClubRepository.GetSingleByCondition(x => x.Id == clubIds[0]);
                if (club is null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "câu lạc bộ" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                foreach (var memberRequest in request)
                {
                 
                        var existingEnrollment = await _unitOfWork.ClubEnrollmentRepository.GetSingleByCondition(
                        ce => ce.ClubId == memberRequest.ClubId &&
                              ce.SemesterId == memberRequest.SemesterId &&
                              (ce.PupilId == memberRequest.PupilId && ce.TeacherId == memberRequest.TeacherId) 
                    );
                        if (existingEnrollment != null)
                        {
                            throw new ConflictException(Responses.ConflictAddMemberClub);
                        }
                    
                }
                await _unitOfWork.ClubEnrollmentRepository.CheckEnrollmentIdsExistAsync(request);
                var newEnrollment = _mapper.Map<AssignMemberRequest[], ClubEnrollment[]>(request);
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

        public async Task<bool> UpdateMemberInClub(UpdateMemberRequest[] request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var clubIds = request.Select(x => x.ClubId).Distinct().ToArray();
                if (clubIds.Length > 1 || clubIds.Length == 0)
                {
                    throw new ConflictException(Responses.ConflictAssignToClub);
                }
                var club = await _unitOfWork.ClubRepository.GetSingleByCondition(x => x.Id == clubIds[0]);
                if (club is null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "câu lạc bộ" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                var members = new List<ClubEnrollment>();
                foreach (var member in request)
                {
                    var enrollment = await _unitOfWork.ClubEnrollmentRepository.GetSingleByCondition(x => x.PupilId == member.PupilId && x.ClubId == member.ClubId);
                    if (enrollment == null)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                    members.Add(enrollment);
                }              
                _mapper.Map(request, members);
                await _context.BulkUpdateAsync(members);
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

        public async Task<bool> RemoveMemberFromClub(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var enrollment = await _unitOfWork.ClubEnrollmentRepository.GetById(id);
                if (enrollment == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _unitOfWork.ClubEnrollmentRepository.Delete(enrollment.Id);
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

        public async Task<PagedResponse<List<ClubEnrollmentForAdminSchoolDTO>>> GetListClubEnrollments(PaginationFilter filters, IUriService uriService, string route, string? keyword, int clubId, int semesterId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.ClubEnrollmentRepository.GetClubEnrollments(clubId, semesterId, keyword);
            var totalRecords = pagedData.Count();
            var enrollmentDto = _mapper.Map<List<ClubEnrollment>, List<ClubEnrollmentForAdminSchoolDTO>>(pagedData);
            var sortedEnrollmentDto = enrollmentDto
                .OrderByDescending(x => x.TeacherId != null)
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
        public async Task<List<TeacherAssignClubDTO>> GetTeachersNotInClub(int clubId, int semesterId, int schoolId)
        {
            var teachersNotInClub = await _unitOfWork.ClubEnrollmentRepository.GetTeachersNotInClub(clubId, semesterId,schoolId);
            if (teachersNotInClub.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            var teacherDtos = _mapper.Map<List<Teacher>, List<TeacherAssignClubDTO>>(teachersNotInClub);

            return teacherDtos;
        }
        public async Task<List<PupilAssignToClubDTO>> GetPupilsNotInClub(int clubId, int semesterId, int schoolId)
        {
            var pupilsNotInClub = await _unitOfWork.ClubEnrollmentRepository.GetPupilsNotInClubAsync(clubId, semesterId, schoolId);
            if(pupilsNotInClub.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            var pupilDtos =_mapper.Map<List<Pupil>, List<PupilAssignToClubDTO>>(pupilsNotInClub);

            return pupilDtos;
        }

        public async Task<List<ClubEnrollmentForAdminSchoolDTO>> GetPupilsRegisterClub(int clubId, int semesterId)
        {
            var pupils = await _unitOfWork.ClubEnrollmentRepository.GetPupilsRegisterClub(clubId, semesterId);
            if (pupils.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            var enrollmentDto = _mapper.Map<List<ClubEnrollment>, List<ClubEnrollmentForAdminSchoolDTO>>(pupils);

            return enrollmentDto;
        }

        public async Task<List<ClubEnrollmentForAdminSchoolDTO>> GetMemberToCopy(int nextSemesterId)
        {
            var enrollmentsInNextSemester = await _unitOfWork.ClubEnrollmentRepository.GetMembersInNextSemester(nextSemesterId);
            if (enrollmentsInNextSemester.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            var newEnrollment = _mapper.Map<List<ClubEnrollment>, List<ClubEnrollmentForAdminSchoolDTO>>((List<ClubEnrollment>)enrollmentsInNextSemester);

            return newEnrollment;
        }
    }
}
