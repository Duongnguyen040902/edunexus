using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Configurations;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Extensions.EF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public ApplicationContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolSubscriptionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureConfiguration());
            modelBuilder.ApplyConfiguration(new FeatureAccessesConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TimeSlotConfiguration());
            modelBuilder.ApplyConfiguration(new TimeTableConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new ClassEnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new AttendanceRecordConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationsConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationImageConfiguration());
            modelBuilder.ApplyConfiguration(new SchoolYearConfiguration());
            modelBuilder.ApplyConfiguration(new SemesterConfiguration());
            modelBuilder.ApplyConfiguration(new PupilFeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new PupilScoreConfiguration());
            modelBuilder.ApplyConfiguration(new BusConfiguration());
            modelBuilder.ApplyConfiguration(new BusRouteConfiguration());
            modelBuilder.ApplyConfiguration(new BusStopConfiguration());
            modelBuilder.ApplyConfiguration(new BusRouteRegistrationConfiguration());
            modelBuilder.ApplyConfiguration(new BusEnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new BusSupervisorConfiguration());
            modelBuilder.ApplyConfiguration(new PupilConfiguration());
            modelBuilder.ApplyConfiguration(new SuperAdminConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new TimeTableSlotSubjectConfiguration());
            modelBuilder.ApplyConfiguration(new ClubConfiguration());
            modelBuilder.ApplyConfiguration(new ClubEnrollmentConfiguration());
            modelBuilder.ApplyConfiguration(new ClassApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new BlacklistConfiguration());
            modelBuilder.ApplyConfiguration(new ClassApplicationCategoryConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<SchoolSubscriptionPlan> SchoolSubscriptionPlans { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureAccess> FeatureAccesses { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassEnrollment> ClassEnrollments { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationCategory> NotificationCategories { get; set; }
        public DbSet<NotificationImage> NotificationImages { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<PupilFeedback> PupilFeedbacks { get; set; }
        public DbSet<PupilScore> PupilScores { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<BusRouteRegistration> BusRouteRegistrations { get; set; }
        public DbSet<BusEnrollment> BusEnrollments { get; set; }
        public DbSet<BusSupervisor> BusSupervisors { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeTableSlotSubject> TimeTableSlotSubjects { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEnrollment> ClubEnrollments { get; set; }
        public DbSet<ClassApplication> ClassApplication { get; set; }
        public DbSet<ClassApplicationCategory> ClassApplicationCategory { get; set; }
        public DbSet<Blacklist> Blacklists { get; set; }
    }
}