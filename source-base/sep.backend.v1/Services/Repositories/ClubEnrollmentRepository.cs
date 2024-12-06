using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;
using System;

namespace sep.backend.v1.Services.Repositories
{
    public class ClubEnrollmentRepository : Repository<ClubEnrollment>, IClubEnrollmentRepository
    {
        public ClubEnrollmentRepository(ApplicationContext context, ILogger<Repository<ClubEnrollment>> logger) : base(context, logger)
        {
            
        }

        public async Task<bool> CheckEnrollmentIdsExistAsync(AssignMemberRequest[] assignRequests)
        {
            foreach (var request in assignRequests)
            {
                var clubExists = await _context.Clubs.AnyAsync(c => c.Id == request.ClubId);
                if (!clubExists)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "câu lạc bộ" } };
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

        public async Task<List<ClubEnrollment>> GetClubEnrollments(int? clubId, int? semesterId, string? keyword)
        {
            var query = _context.ClubEnrollments.AsQueryable();
            query = query.Where(x => x.Status == 2 || x.Status == 5);
            if (semesterId.HasValue)
            {
                query = query.Where(be => be.SemesterId == semesterId);
            }
            if (clubId.HasValue)
            {
                query = query.Where(be => be.ClubId == clubId);
            }   
            query = query
                .Include(be => be.Teacher)
                .Include(be => be.Pupil)
                .Include(be => be.Semester)
                .ThenInclude(s => s.SchoolYear);

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(be =>
                    (be.Teacher != null &&
                     (be.Teacher.FirstName.ToLower().Contains(keyword.ToLower()) ||
                      be.Teacher.LastName.ToLower().Contains(keyword.ToLower()) ||
                      be.Teacher.Username.ToLower().Contains(keyword.ToLower()))) ||
                    (be.Pupil != null &&
                     (be.Pupil.FirstName.ToLower().Contains(keyword.ToLower()) ||
                      be.Pupil.LastName.ToLower().Contains(keyword.ToLower()) ||
                      be.Pupil.Username.ToLower().Contains(keyword.ToLower())))
                );
            }
            
            return await query.ToListAsync();
        }
        public async Task<List<Teacher>> GetTeachersNotInClub(int clubId, int semesterId, int schoolId)
        {
            var teacherIdsInClub = await _context.ClubEnrollments
                .Where(ce => ce.ClubId == clubId && ce.SemesterId == semesterId && ce.TeacherId.HasValue)
                .Select(ce => ce.TeacherId.Value)
                .ToListAsync();
            var teachersNotInClub = await _context.Teachers
                .Where(t => t.SchoolId == schoolId && !teacherIdsInClub.Contains(t.Id) && (t.AccountStatus == (int)Statuses.Active || t.AccountStatus == (int)Statuses.Inactive))
                .ToListAsync();

            return teachersNotInClub;
        }
        public async Task<List<Pupil>> GetPupilsNotInClubAsync(int clubId, int semesterId, int schoolId)
        {
            var pupilIdsInClub = await _context.ClubEnrollments
                .Where(ce => ce.ClubId == clubId && ce.SemesterId == semesterId && ce.PupilId.HasValue)
                .Select(ce => ce.PupilId.Value)
                .ToListAsync();
            var pupilsNotInClub = await _context.Pupils
                .Where(p =>p.SchoolId == schoolId && !pupilIdsInClub.Contains(p.Id) && (p.AccountStatus == (int)Statuses.Active || p.AccountStatus == (int)Statuses.Inactive))
                .ToListAsync();

            return pupilsNotInClub;
        }

        public async Task<List<ClubEnrollment>> GetPupilsRegisterClub(int? clubId, int? semesterId)
        {
            var query = _context.ClubEnrollments.AsQueryable();

            if (semesterId.HasValue)
            {
                query = query.Where(be => be.SemesterId == semesterId);
            }
            if (clubId.HasValue)
            {
                query = query.Where(be => be.ClubId == clubId);
            }
            query = query.Where(be => be.Status == 1 && be.PupilId != null);
            query = query
                .Include(be => be.Teacher)
                .Include(be => be.Pupil)
                .Include(be => be.Semester)
                .ThenInclude(s => s.SchoolYear);

            return await query.OrderBy(x=> x.CreatedDate).ToListAsync();
        }

        public async Task<List<ClubEnrollment>> GetMembersInNextSemester(int semesterId)
        {
            var query = _context.ClubEnrollments.AsQueryable();

            query = query.Where(x => x.SemesterId == semesterId);

            // Include related entities
            query = query
           .Include(be => be.Teacher)
           .Include(be => be.Pupil)
           .Include(be => be.Semester)
           .ThenInclude(s => s.SchoolYear);

            return await query.ToListAsync();
        }
    }
}
