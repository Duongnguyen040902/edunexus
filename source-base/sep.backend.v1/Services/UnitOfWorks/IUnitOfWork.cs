using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.IServices;
using System;
using System.Threading.Tasks;

namespace sep.backend.v1.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IBusSupervisorRepository BusSupervisorRepository { get; }
        ISchoolRepository SchoolRepository { get; }
        IPupilRepository PupilRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        IClassRepository ClassRepository { get; }
        IClubRepository ClubRepository { get; }
        ITimetableRepository TimetableRepository { get; }
        IAttendanceRecordRepository AttendanceRecordRepository { get; }
        ISchoolSubscriptionRepository SchoolSubscriptionRepository { get; }
        IBusRouteRepository BusRouteRepository { get; }
        ISubscriptionPlanRepository SubscriptionPlanRepository { get; }
        IClubEnrollmentRepository ClubEnrollmentRepository { get; }
        IBusRepository BusRepository { get; }

        ITimeSlotRepository TimeSlotRepository { get; }
        IClassEnrollmentRepository ClassEnrollmentRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IBusStopRepository BusStopRepository { get; }
        IBusEnrollmentRepository BusEnrollmentRepository { get; }
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}