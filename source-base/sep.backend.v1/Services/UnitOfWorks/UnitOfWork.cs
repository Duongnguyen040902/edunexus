using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.Repositories;
using sep.backend.v1.Services.UnitOfWork;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationContext context, ILogger<UnitOfWork> logger, IServiceProvider serviceProvider)
        {
            _context = context;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<Type, object>();

            UserRepository = CreateRepository<UserRepository, IUserRepository>();
            BusSupervisorRepository = CreateRepository<BusSupervisorRepository, IBusSupervisorRepository>();
            SchoolRepository = CreateRepository<SchoolRepository, ISchoolRepository>();
            ClassEnrollmentRepository = CreateRepository<ClassEnrollmentRepository, IClassEnrollmentRepository>();
            PupilRepository = CreateRepository<PupilRepository, IPupilRepository>();
            TeacherRepository = CreateRepository<TeacherRepository, ITeacherRepository>();
            ClassRepository = CreateRepository<ClassRepository, IClassRepository>();
            TimetableRepository = CreateRepository<TimetableRepository, ITimetableRepository>();
            ClubRepository = CreateRepository<ClubRepository, IClubRepository>();
            AttendanceRecordRepository = CreateRepository<AttendanceRecordRepository, IAttendanceRecordRepository>();
            BusRouteRepository = CreateRepository<BusRouteRepository, IBusRouteRepository>();
            SchoolSubscriptionRepository =
                CreateRepository<SchoolSubscriptionRepository, ISchoolSubscriptionRepository>();
            SubscriptionPlanRepository = CreateRepository<SubscriptionPlanRepository, ISubscriptionPlanRepository>();
            InvoiceRepository = CreateRepository<InvoiceRepository, IInvoiceRepository>();
            BusRepository = CreateRepository<BusRepository, IBusRepository>();
            FeedbackRepository = CreateRepository<FeedbackRepository, IFeedbackRepository>();
            SubjectRepository = CreateRepository<SubjectRepository, ISubjectRepository>();
            BusStopRepository = CreateRepository<BusStopRepository, IBusStopRepository>();
            BusEnrollmentRepository = CreateRepository<BusEnrollmentRepository, IBusEnrollmentRepository>();
            TimeSlotRepository = CreateRepository<TimeSlotRepository, ITimeSlotRepository>();
            ClubEnrollmentRepository = CreateRepository<ClubEnrollmentRepository, IClubEnrollmentRepository>();
        }

        public IUserRepository UserRepository { get; }
        public IBusSupervisorRepository BusSupervisorRepository { get; }
        public ISchoolRepository SchoolRepository { get; }
        public IPupilRepository PupilRepository { get; }

        public ITeacherRepository TeacherRepository { get; }
        public IClassRepository ClassRepository { get; }
        public ITimetableRepository TimetableRepository { get; }
        public IClubRepository ClubRepository { get; }
        public ISchoolSubscriptionRepository SchoolSubscriptionRepository { get; }
        public ISubscriptionPlanRepository SubscriptionPlanRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IBusRepository BusRepository { get; }
        public IBusRouteRepository BusRouteRepository { get; }
        public IAttendanceRecordRepository AttendanceRecordRepository { get; }
        public IClassEnrollmentRepository ClassEnrollmentRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public ISubjectRepository SubjectRepository { get; }
        public IBusEnrollmentRepository BusEnrollmentRepository { get; }
        public IBusStopRepository BusStopRepository { get; }
        public ITimeSlotRepository TimeSlotRepository { get; }
        public IClubEnrollmentRepository ClubEnrollmentRepository { get; }
        private TRepository CreateRepository<TRepository, TInterface>() where TRepository : class, TInterface
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<TRepository>>();
            return (TRepository)Activator.CreateInstance(typeof(TRepository), _context, logger);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IRepository<TEntity>)repository;
            }

            var logger = _serviceProvider.GetRequiredService<ILogger<Repository<TEntity>>>();
            var newRepository = new Repository<TEntity>(_context, logger);
            _repositories[typeof(TEntity)] = newRepository;

            return newRepository;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Database changes committed successfully.");
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Transaction started.");
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
                _logger.LogInformation("Transaction committed.");
            }
            catch (Exception ex)
            {
                await _transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while committing the transaction.");
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
                _logger.LogInformation("Transaction rolled back.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rolling back the transaction.");
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _logger.LogInformation("UnitOfWork disposed.");
        }
    }
}