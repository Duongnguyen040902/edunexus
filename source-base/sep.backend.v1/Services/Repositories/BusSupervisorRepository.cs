using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IRepositories;

namespace sep.backend.v1.Services.Repositories
{
    public class BusSupervisorRepository : Repository<BusSupervisor>, IBusSupervisorRepository
    {
        public BusSupervisorRepository(ApplicationContext context, ILogger<Repository<BusSupervisor>> logger) : base(context, logger)
        {
        }

        public async Task<object> LoginAsyncByModeBusSupervisor(string email, string password)
        {
            return await _context.BusSupervisors.LoginAsync(_context, _logger, email, password);
        }

        public async Task<List<BusSupervisor>> GetListBusSupervisorOfSchoolAsync(
        int schoolId,
        int? accountStatus = null,
        string? searchKey = null)
        {
            var query = _context.BusSupervisors
                .Where(t => t.SchoolId == schoolId)
                .AsQueryable();


            if (accountStatus.HasValue)
            {
                query = query.Where(t => t.AccountStatus == accountStatus);
            }

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                var lowerKeyword = searchKey.ToLower();
                query = query.Where(t => (t.LastName.ToLower() + " " + t.FirstName.ToLower()).Contains(lowerKeyword) ||
                        t.FirstName.ToLower().Contains(lowerKeyword) ||
                        t.LastName.ToLower().Contains(lowerKeyword) ||
                        t.Username.ToLower().Contains(lowerKeyword) ||
                        t.Email.ToLower().Contains(lowerKeyword));
            }

            query = query.OrderByDescending(t => t.Id);

            return await query.ToListAsync();
        }
    }
}
