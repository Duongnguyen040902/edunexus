using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class BusEnrollmentRepository : Repository<BusEnrollment>, IBusEnrollmentRepository
    {
        public BusEnrollmentRepository(ApplicationContext context, ILogger<Repository<BusEnrollment>> logger) : base(context, logger)
        {
        }

        public async Task<bool> CheckEnrollmentIdsExistAsync(CreateBusEnrollmentDTO[] busEnrollmentDtos)
        {
            foreach (var dto in busEnrollmentDtos)
            {
                var busExists = await _context.Buses.AnyAsync(b => b.Id == dto.BusId);
                if (!busExists)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "xe buýt" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                if (dto.PupilId.HasValue)
                {
                    var pupilExists = await _context.Pupils.AnyAsync(p => p.Id == dto.PupilId.Value);
                    if (!pupilExists)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "học sinh" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                }

                if (dto.BusSupervisorId.HasValue)
                {
                    var busSupervisorExists = await _context.BusSupervisors.AnyAsync(bs => bs.Id == dto.BusSupervisorId.Value);
                    if (!busSupervisorExists)
                    {
                        var placeholders = new Dictionary<string, string> { { "attribute", "người giám sát xe buýt" } };
                        throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                    }
                }

                var semesterExists = await _context.Semesters.AnyAsync(s => s.Id == dto.SemesterId);
                if (!semesterExists)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "học kỳ" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
            }

            return true;
        }

        public async Task<List<BusEnrollment>> GetBusEnrollments(int? busId, int? semesterId)
        {
            var query = _context.BusEnrollments.AsQueryable();

            if (busId.HasValue)
            {
                query = query.Where(be => be.BusId == busId.Value);
            }

            if (semesterId.HasValue)
            {
                query = query.Where(be => be.SemesterId == semesterId.Value);
            }
            query = query
                .Include(be => be.Bus)
                .Include(be => be.Pupil)
                .Include(be => be.BusSupervisor)
                .Include(be => be.BusStop)
                .Include(be => be.Semester)
                .ThenInclude(s => s.SchoolYear);

            return await query.ToListAsync();
        }

        public async Task<List<BusEnrollment>> GetMembersInNextSemester(int semesterId)
        {
            var query = _context.BusEnrollments.AsQueryable();

            query = query.Where(x => x.SemesterId == semesterId);

            // Include related entities
            query = query
                .Include(be => be.Bus)
                .Include(be => be.Pupil)
                .Include(be => be.BusSupervisor)
                .Include(be => be.BusStop)
                .Include(be => be.Semester)
                .ThenInclude(s => s.SchoolYear);

            return await query.ToListAsync();
        }
    }
}
