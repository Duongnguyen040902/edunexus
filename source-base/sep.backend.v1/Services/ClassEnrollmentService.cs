using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class ClassEnrollmentService : BaseService<ClassEnrollmentDTO, ClassEnrollment>, IClassEnrollmentService
    {
        private ApplicationContext _context;
        public ClassEnrollmentService(IUnitOfWork unitOfWork, IAutoMapper mapper, ApplicationContext context) : base(unitOfWork, mapper)
        {
            _context = context;
        }

        public async Task<bool> AssignPupilToClass(AssignPupilRequest[] requests, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (requests == null || !requests.Any())
                {
                    throw new NotFoundException(Responses.EmptyRequest);
                }
                var classIds = requests.Select(r => r.ClassId).Distinct().ToList();
                if (classIds.Count > 1)
                {
                    throw new ConflictException(Responses.MultipleClassesError);
                }
                var classId = classIds.First();
                var classExists = await _unitOfWork.GetRepository<Class>().GetById(classId);
                if (classExists == null)
                {
                    throw new NotFoundException(Responses.NotFoundClass);
                }
                if(classExists.Status == (int)ClassStatus.Inactive)
                {
                    classExists.Status = (int)ClassStatus.Active;
                    await _unitOfWork.GetRepository<Class>().Update(classExists);
                }         
                foreach (var request in requests)
                {
                    var existingEnrollment = await _unitOfWork.GetRepository<ClassEnrollment>().GetSingleByCondition(
                        ce => ce.ClassId == request.ClassId && ce.SemesterId == request.SemesterId && ce.PupilId == request.PupilId
                    );

                    if (existingEnrollment != null)
                    {
                        throw new ConflictException(Responses.ConflictAddMemberClass);
                    }
                }
                var enrollments = _mapper.Map<AssignPupilRequest[], ClassEnrollment[]>(requests);
                await _context.BulkInsertAsync(enrollments);
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


        public async Task<bool> AssignTeacherToClass(AssignTeacherRequest request, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var classExists = await _unitOfWork.GetRepository<Class>().GetById(request.ClassId);
                if (classExists == null)
                {
                    throw new NotFoundException(Responses.NotFoundClass);
                }
                classExists.Status = (int)ClassStatus.Active;
                await _unitOfWork.GetRepository<Class>().Update(classExists);
                var teacherExists = await _unitOfWork.GetRepository<Teacher>().GetById(request.TeacherId);
                if (teacherExists == null)
                {
                    throw new NotFoundException(Responses.NotFoundTeacher);
                }
                var existingEnrollment = await _unitOfWork.ClassEnrollmentRepository
                    .GetEnrollmentByClassAndTeacherAsync(request.ClassId, request.TeacherId, request.SemesterId, schoolId);
                if (existingEnrollment != null)
                {
                    throw new ConflictException(Responses.ConflictAssignTeacher);
                }
                var enrollment = _mapper.Map<AssignTeacherRequest, ClassEnrollment>(request);
                enrollment.CreatedDate = DateTime.Now;
                await _unitOfWork.GetRepository<ClassEnrollment>().Add(enrollment);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<bool> RemoveTeacherFromClass(int classId, int semesterId, int teacherId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var classEnrollment = await _unitOfWork.ClassEnrollmentRepository.
                GetSingleByCondition(x => x.ClassId == classId && x.SemesterId == semesterId && x.TeacherId == teacherId);
                if (classEnrollment is null)
                {
                    throw new NotFoundException(Responses.NotFoundTeacher);
                }
                _unitOfWork.ClassEnrollmentRepository.Delete(classEnrollment.Id);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> RemovePupilFromClass(int classId, int semesterId, int pupilId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var classEnrollment = await _unitOfWork.ClassEnrollmentRepository.
                GetSingleByCondition(x => x.ClassId == classId && x.SemesterId == semesterId && x.PupilId == pupilId);
                if (classEnrollment is null)
                {
                    throw new NotFoundException(Responses.NotFoundPupil);
                }
                _unitOfWork.ClassEnrollmentRepository.Delete(classEnrollment.Id);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<int> GetTeacherId(int classId, int semesterId)
        {
            var teacher = await _unitOfWork.ClassEnrollmentRepository.GetSingleByCondition(x => x.ClassId == classId && x.SemesterId == semesterId && x.TeacherId != null);
            if (teacher != null)
            {
                return (int)teacher.TeacherId;
            }
            throw new NotFoundException(Responses.NotFoundTeacher);
        }

        public async Task<bool> UpdateAssignTeacherToClass(UpdateAssignTeacherRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.ClassEnrollmentRepository.ValidateUpdateAssignTeacherAsync(request);
                var classEnrollment = await _unitOfWork.ClassEnrollmentRepository.GetById(request.ClassEnrollmentId);
                classEnrollment.OldTeacher = string.IsNullOrEmpty(classEnrollment.OldTeacher)
                                            ? classEnrollment.TeacherId?.ToString()
                                            : classEnrollment.OldTeacher + ";" + classEnrollment.TeacherId?.ToString();
                classEnrollment.TeacherId = request.TeacherId;
                classEnrollment.CreatedDate = DateTime.Now;
                _unitOfWork.ClassEnrollmentRepository.Update(classEnrollment);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> SwapTeachersInClass(int ceTeacherId1, int ceTeacherId2)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var enrollment1 = await _unitOfWork.ClassEnrollmentRepository.GetSingleByCondition(x => x.Id == ceTeacherId1);
                var enrollment2 = await _unitOfWork.ClassEnrollmentRepository.GetSingleByCondition(x => x.Id == ceTeacherId2);

                if (enrollment1 == null || enrollment2 == null )
                {
                    throw new NotFoundException(Responses.NotFoundClass);
                }
                if(enrollment1.TeacherId == null || enrollment2.TeacherId == null)
                {
                    throw new NotFoundException(Responses.NotFoundTeacher);
                }
                enrollment1.OldTeacher = enrollment1.OldTeacher == null
                     ? enrollment2.Id.ToString()
                     : enrollment1.OldTeacher + ";" + enrollment2.Id;
                enrollment2.OldTeacher = enrollment2.OldTeacher == null
                    ? enrollment1.Id.ToString()
                    : enrollment2.OldTeacher + ";" + enrollment1.Id; int tempTeacherId = (int)enrollment1.TeacherId;
                enrollment1.TeacherId = enrollment2.TeacherId;
                enrollment2.TeacherId = tempTeacherId;
                var ob1 = enrollment1;
                var ob2 = enrollment2;
                _unitOfWork.ClassEnrollmentRepository.Delete(enrollment1.Id);
                _unitOfWork.ClassEnrollmentRepository.Delete(enrollment2.Id);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClassEnrollmentRepository.Add(ob1);
                _unitOfWork.ClassEnrollmentRepository.Add(ob2);
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

        public async Task<List<TeacherSwapDTO>> GetTeacherInClass(int semesterId, int schoolId, int ceTeacherId1)
        {
            var classEnrollments = await _unitOfWork.ClassEnrollmentRepository
                .GetMulti(ce => ce.SemesterId == semesterId && ce.TeacherId != null && ce.Id != ceTeacherId1,
                          new string[] { "Teacher","Class" });

            if (classEnrollments == null || !classEnrollments.Any())
            {
                throw new NotFoundException(Responses.NotFoundTeacher);
            }
            var teacherSwaps = _mapper.Map<List<ClassEnrollment>, List<TeacherSwapDTO>>(classEnrollments.ToList());

            return teacherSwaps;
        }

        public async Task<PagedResponse<List<MemberInClassDTO>>> GetMemberInClass(PaginationFilter filters, IUriService uriService, string route, string? keyword, int classId, int semesterId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.ClassEnrollmentRepository.GetMembersInClass(classId, semesterId, keyword);
            var totalRecords = pagedData.Count();
            var memberDtos = _mapper.Map<List<ClassEnrollment>, List<MemberInClassDTO>>(pagedData);
            var sortedMemberDtos = memberDtos
                .OrderByDescending(x => x.TeacherCode != null)
                .ThenBy(x => x.PupilName)
                .ToList();

            return PagedResponseHelper.CreatePagedResponse(
                memberDtos.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );
        }

        public async Task<bool> RemoveMemberClass(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var classEnrollment = await _unitOfWork.ClassEnrollmentRepository.
                GetSingleByCondition(x => x.Id == id);
                if (classEnrollment is null)
                {
                    throw new NotFoundException(Responses.NotFoundPupil);
                }
                _unitOfWork.ClassEnrollmentRepository.Delete(classEnrollment.Id);
                await _unitOfWork.CommitTransactionAsync();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            };
        }

        public async Task<bool> AssignMemberToClass(AssignMemberToClass[] request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var classIds = request.Select(x => x.ClassId).Distinct().ToArray();
                if (classIds.Length > 1 || classIds.Length == 0)
                {
                    throw new ConflictException(Responses.ConflictAssignToClass);
                }
                var club = await _unitOfWork.ClubRepository.GetSingleByCondition(x => x.Id == classIds[0]);
                if (club is null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "lớp học" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                foreach (var memberRequest in request)
                {

                    var existingEnrollment = await _unitOfWork.ClubEnrollmentRepository.GetSingleByCondition(
                    ce => ce.ClubId == memberRequest.ClassId &&
                          ce.SemesterId == memberRequest.SemesterId &&
                          (ce.PupilId == memberRequest.PupilId && ce.TeacherId == memberRequest.TeacherId)
                );
                    if (existingEnrollment != null)
                    {
                        throw new ConflictException(Responses.ConflictAddMemberClass);
                    }

                }
                await _unitOfWork.ClassEnrollmentRepository.CheckEnrollmentIdsExistAsync(request);
                var newEnrollment = _mapper.Map<AssignMemberToClass[], ClassEnrollment[]>(request);
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

        public async Task<List<MemberInClassDTO>> GetMemberToCopy(int nextSemesterId)
        {
            var enrollmentsInNextSemester = await _unitOfWork.ClassEnrollmentRepository.GetMembersInNextSemester(nextSemesterId);
            if (enrollmentsInNextSemester.Count == 0)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            var newEnrollment = _mapper.Map<List<ClassEnrollment>, List<MemberInClassDTO>>((List<ClassEnrollment>)enrollmentsInNextSemester);

            return newEnrollment;
        }

        public async Task<bool> PupilsToGraduate(int[] pupilIds)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var members = new List<Pupil>();
                foreach (var pupilId in pupilIds)
                {
                    var pupil = await _unitOfWork.PupilRepository.GetSingleByCondition(x => x.Id == pupilId);
                    if (pupil == null)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "thông tin" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                    if (pupil.AccountStatus == (int)Statuses.Active && pupil.Email is null)
                    {
                        throw new CustomException(Messages.VERIFY_ACCOUNT);
                    }
                    pupil.AccountStatus = (int)Statuses.Delete;
                    members.Add(pupil);
                }
                await _context.BulkUpdateAsync(members);
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
    }
}
