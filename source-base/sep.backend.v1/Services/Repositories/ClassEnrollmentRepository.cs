using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace sep.backend.v1.Services.Repositories
{
    public class ClassEnrollmentRepository : Repository<ClassEnrollment>, IClassEnrollmentRepository
    {
        public ClassEnrollmentRepository(ApplicationContext context, ILogger<Repository<ClassEnrollment>> logger) : base(context, logger)
        {
        }

        public async Task<ClassEnrollment> GetEnrollmentByClassAndTeacherAsync(int classId, int teacherId, int semesterId, int schoolId) //TO-DO
        {
            return await _context.Set<ClassEnrollment>()
            .Include(e => e.Class)
            .FirstOrDefaultAsync(e => e.ClassId == classId && e.TeacherId == teacherId && e.SemesterId == semesterId && e.Class.SchoolId == schoolId);
        }

        public async Task<ClassEnrollment> GetEnrollmentByClassAndPupilAsync(int pupilId, int classId, int semesterId, int schoolId) // TO-DO
        {
            return await _context.Set<ClassEnrollment>()
                .Include(e => e.Class)
                .FirstOrDefaultAsync(e => e.PupilId == pupilId && e.ClassId == classId && e.SemesterId == semesterId && e.Class.SchoolId == schoolId);
        }

        public async Task<bool> ValidateUpdateAssignTeacherAsync(UpdateAssignTeacherRequest request)
        {
            var classExists = await _context.Classes.FindAsync(request.ClassId);
            if (classExists == null)
            {
                throw new NotFoundException(Responses.NotFoundClass);
            }
            var teacherExists = await _context.Teachers.FindAsync(request.TeacherId);
            if (teacherExists == null)
            {
                throw new NotFoundException(Responses.NotFoundTeacher);
            }
            var isAssignedToAnotherClass = await _context.ClassEnrollments
                .AnyAsync(x => x.TeacherId == request.TeacherId && x.SemesterId == request.SemesterId && x.ClassId != request.ClassId);

            if (isAssignedToAnotherClass)
            {
                throw new ConflictException(Responses.ConflictAssignTeacher);
            }
            var classEnrollment = await GetById(request.ClassEnrollmentId);
            if (classEnrollment == null)
            {
                throw new NotFoundException(Responses.NotFoundClass);
            }
            return true;
        }

        public async Task<List<ClassEnrollment>> GetMembersInClass(int semesterId, int classId, string? keyword)
        {
            var query = _context.ClassEnrollments.AsQueryable();
            // Filter by class and semester
            query = query.Where(x => x.SemesterId == semesterId && x.ClassId == classId);

            // Include related entities
            query = query
                .Include(ce => ce.Teacher)
                .Include(ce => ce.Pupil)
                .Include(ce => ce.Class)
                .Include(ce => ce.Semester)
                .ThenInclude(s => s.SchoolYear);

            // Apply keyword search if provided
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(ce =>
                    (ce.Teacher != null &&
                     (ce.Teacher.FirstName.ToLower().Contains(keyword) || ce.Teacher.LastName.ToLower().Contains(keyword) || ce.Teacher.Username.ToLower().Contains(keyword))) ||
                    (ce.Pupil != null &&
                     (ce.Pupil.FirstName.ToLower().Contains(keyword) || ce.Pupil.LastName.ToLower().Contains(keyword) || ce.Pupil.Username.ToLower().Contains(keyword))));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CheckEnrollmentIdsExistAsync(AssignMemberToClass[] assignRequests)
        {
            foreach (var request in assignRequests)
            {
                var clubExists = await _context.Clubs.AnyAsync(c => c.Id == request.ClassId);
                if (!clubExists)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "lớp học" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                if (request.PupilId.HasValue)
                {
                    var pupilExists = await _context.Pupils.AnyAsync(p => p.Id == request.PupilId.Value);
                    if (!pupilExists)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "học sinh" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                }

                if (request.TeacherId.HasValue)
                {
                    var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == request.TeacherId.Value);
                    if (!teacherExists)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "giáo viên" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                }

                var semesterExists = await _context.Semesters.AnyAsync(s => s.Id == request.SemesterId);
                if (!semesterExists)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "học kỳ" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
            }

            return true;
        }

        public async Task<List<ClassEnrollment>> GetMembersInNextSemester(int semesterId)
        {
            var query = _context.ClassEnrollments.AsQueryable();

            query = query.Where(x => x.SemesterId == semesterId);

            // Include related entities
            query = query
                .Include(ce => ce.Teacher)
                .Include(ce => ce.Pupil) 
                .Include(ce => ce.Class)
                .Include(ce => ce.Semester)
                .ThenInclude(s => s.SchoolYear);

            return await query.ToListAsync();
        }
    }
}
