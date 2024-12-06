using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class SchoolDashboardService : BaseService<School, SchoolDTO>, ISchoolDashboardService
    {
        public SchoolDashboardService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<SchoolDashboardDTO> GetSchoolDashboard(int schoolId)
        {
            var activeClasses = await _unitOfWork.GetRepository<Class>().Count(c => c.SchoolId == schoolId && ( c.Status == (int)ClassStatus.Active|| c.Status == (int)ClassStatus.Inactive) );
            var activeClubs = await _unitOfWork.GetRepository<Club>().Count(c => c.SchoolId == schoolId && c.Status==1);
            var activeBusRoutes = await _unitOfWork.GetRepository<BusRoute>().Count(br => br.SchoolId == schoolId && br.Status==1);
            var activeBuses = await _unitOfWork.GetRepository<Bus>().Count(b => b.BusRoute.SchoolId == schoolId && b.Status ==1);
            var activePupils = await _unitOfWork.GetRepository<Pupil>().Count(p => p.SchoolId == schoolId && (p.AccountStatus == (int)StatusAccount.Active || p.AccountStatus==(int)StatusAccount.Inactive) );
            var activeTeachers = await _unitOfWork.GetRepository<Teacher>().Count(t => t.SchoolId == schoolId && (t.AccountStatus == (int)StatusAccount.Active || t.AccountStatus == (int)StatusAccount.Inactive));
            var activeSupervisors = await _unitOfWork.GetRepository<BusSupervisor>().Count(s => s.SchoolId == schoolId && (s.AccountStatus == (int)StatusAccount.Active || s.AccountStatus == (int)StatusAccount.Inactive));
            var totalActiveAccounts = activePupils + activeTeachers + activeSupervisors;

            var currentSemester = await _unitOfWork.GetRepository<Semester>().GetSingleByCondition(s => s.SchoolYear.SchoolId == schoolId && s.IsActive);
            var currentSchoolYear = await _unitOfWork.GetRepository<SchoolYear>().GetSingleByCondition(sy => sy.SchoolId == schoolId && sy.IsActive);

            var dashboardDTO = new SchoolDashboardDTO
            {
                CountActiveClass = activeClasses,
                CountActiveClub = activeClubs,
                CountActiveBusRoute = activeBusRoutes,
                CountActiveBus = activeBuses,
                CountActivePupil = activePupils,
                CountActiveTeacher = activeTeachers,
                CountActiveSupervisor = activeSupervisors,
                TotalActiveAccount = totalActiveAccounts,
                SemesterName = currentSemester?.SemesterName,
                SchoolYearName = currentSchoolYear?.Name
            };

            return dashboardDTO;
        }
    }
}