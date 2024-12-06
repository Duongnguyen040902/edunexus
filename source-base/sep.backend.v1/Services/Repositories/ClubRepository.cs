using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;
using System.Linq;

namespace sep.backend.v1.Services.Repositories;

public class ClubRepository : Repository<Club>, IClubRepository
{
    public ClubRepository(ApplicationContext context, ILogger<Repository<Club>> logger) : base(context, logger)
    {
    }

    public async Task<Club> GetClubDetailAsync(int clubId, int semesterId)
    {
        var result = await _context.Clubs
            .Include(c => c.School)
            .Include(c => c.ClubEnrollments.Where(ce =>
                ce.SemesterId == semesterId &&
                (ce.Status == (int)ClubEnrollmentStatus.Teaching || ce.Status == (int)ClubEnrollmentStatus.Approved)))
                .ThenInclude(ce => ce.Semester)
                .ThenInclude(s => s.SchoolYear)
            .Include(c => c.ClubEnrollments.Where(ce =>
                ce.SemesterId == semesterId &&
                (ce.Status == (int)ClubEnrollmentStatus.Teaching || ce.Status == (int)ClubEnrollmentStatus.Approved)))
                .ThenInclude(ce => ce.Pupil)
            .Include(c => c.ClubEnrollments.Where(ce =>
                ce.SemesterId == semesterId &&
                (ce.Status == (int)ClubEnrollmentStatus.Teaching || ce.Status == (int)ClubEnrollmentStatus.Approved)))
                .ThenInclude(ce => ce.Teacher)
            .FirstOrDefaultAsync(c => c.Id == clubId);

        return result;
    }

    public async Task<List<Club>> GetClubsDetailAsync(int schoolId)
    {
        var result = await _context.Clubs.Where(c=>c.Status==(int)Statuses.Active && c.SchoolId== schoolId)
            .Include(c => c.School)
            .Include(c => c.ClubEnrollments)
                .ThenInclude(ce => ce.Semester)
                .ThenInclude(s => s.SchoolYear)
            .Include(c => c.ClubEnrollments)
                .ThenInclude(ce => ce.Pupil)
            .Include(c => c.ClubEnrollments)
                .ThenInclude(ce => ce.Teacher)
            .OrderByDescending(c => c.Id)
            .ToListAsync();
        return result;
    }
    public async Task<List<Club>> GetAllClubs(int? status, string? keyword, int? schoolId)
    {
        var query = _context.Clubs.AsQueryable();
        if (schoolId.HasValue)
            query = query.Where(c => c.SchoolId == schoolId.Value);

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(c => c.Name.ToLower().Contains(keyword.ToLower()) || (c.Description != null && c.Description.ToLower().Contains(keyword.ToLower())));

        return await query.ToListAsync();
    }
}
