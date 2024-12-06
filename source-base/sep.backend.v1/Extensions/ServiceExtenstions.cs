using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Data.Seeders;
using sep.backend.v1.DTOs;
using sep.backend.v1.DTOs.Profiles;
using sep.backend.v1.Requests.Auth;
using sep.backend.v1.Services;
using sep.backend.v1.Services.HostedService;
using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.Repositories;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Services.UnitOfWorks;
using sep.backend.v1.Validators;
using sep.backend.v1.Validators.Teacher;
using System.Reflection;
using sep.backend.v1.Validators.Invoice;
using sep.backend.v1.Validators.Payment;
using sep.backend.v1.Validators.SubscriptionPlan;
using sep.backend.v1.Validators.SchoolAdmin;
using sep.backend.v1.Validators.BusSupervisor;

using sep.backend.v1.Requests.Excel;

namespace sep.backend.v1.Extensions
{
    public static class ServiceExtenstions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true)
                        .AllowCredentials());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IPupilRepository, PupilRepository>();
            services.AddScoped<IClassEnrollmentRepository, ClassEnrollmentRepository>();
            services.AddScoped<IBusSupervisorRepository, BusSupervisorRepository>();
            services.AddScoped<ISuperAdminRepository, SuperAdminRepository>();
            services.AddScoped<ISchoolSubscriptionRepository, SchoolSubscriptionRepository>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IBusRepository, BusRepository>();
            services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IBusRouteRepository, BusRouteRepository>();
            services.AddScoped<ISchoolYearRepository, SchoolYearRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IBusStopRepository, BusStopRepository>();
            services.AddScoped<IBusEnrollmentRepository, BusEnrollmentRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IClubEnrollmentRepository, ClubEnrollmentRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<ITimetableService, TimetableService>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISchoolAdminService, SchoolAdminService>();
            services.AddScoped<ISchoolSubscriptionService, SchoolSubscriptionService>();
            services.AddScoped<IPupilService, PupilService>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IClassApplicationService, ClassApplicationService>();
            services.AddTransient<IJobSendInvoice, JobSendInvoice>();
            services.AddScoped<IAttendanceRecordService, AttendanceRecordService>();
            services.AddTransient<IBusService, BusService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IClassEnrollmentService, ClassEnrollmentService>();
            services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IEmailInvoiceService, EmailInvoiceService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddHostedService<SubscriptionAndInvoiceUpdateService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddTransient<IClubEnrollmentService, ClubEnrollmentService>();
            services.AddTransient<IPupilScoreService, PupilScoreService>();
            services.AddScoped<IBusRouteService, BusRouteService>();
            services.AddScoped<ISchoolYearService, SchoolYearService>();
            services.AddTransient<IPupilScoreService, PupilScoreService>();
            services.AddScoped<IPupilFeedbackService, PupilFeedbackService>();
            services.AddScoped<IBusStopService, BusStopService>();
            services.AddScoped<IBusEnrollmentService, BusEnrollmentService>();
            services.AddScoped<IBusSupervisorService, BusSupervisorService>();
            services.AddScoped<ISchoolDashboardService, SchoolDashboardService>();
            services.AddScoped<IEmailConfirmCode, EmailConfirmCode>();
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureValidador(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddScoped<IValidator<RegisterDTORemake>, RegisterSchoolValidator>();
            services.AddScoped<IValidator<VerifyEmailRequest>, VerifyCodeValidator>();
            services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordValidator>();
            services.AddScoped<IValidator<ForgotPasswordRequest>, ForgotPasswordValidator>();
            services.AddScoped<IValidator<TimeTableDTO>, TimeTableValidator>();
            services.AddScoped<IValidator<CreateTeacherDTO>, TeacherValidator>();
            services.AddScoped<IValidator<UpdateTeacherDTO>, TeacherUpdateValidator>();
            services.AddScoped<IValidator<CreatePupilDTO>, PupilValidator>();
            services.AddScoped<IValidator<UpdatePupilDTO>, PupilUpdateValidator>();
            services.AddScoped<IValidator<CreateSchoolDTO>, CreateSchoolValidator>();
            services.AddScoped<IValidator<UpdateSchoolDTO>, UpdateSchoolValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<AddNotificationDTO>, NotificationValidator>();
            services.AddScoped<IValidator<UpdateNotificationDTO>, UpdateNotificationValidate>();
            services.AddScoped<IValidator<CreateAndUpdateClassApplicationDTO>, ClassAppicationValidator>();
            services.AddScoped<IValidator<ResponeClassApplicationDTO>, ResponeClassApplicationValidator>();
            services.AddScoped<IValidator<AttendanceRecordDTO>, AttendanceRecordValidator>();
            services.AddScoped<IValidator<UpdateInfoSchoolDTO>, SchoolValidator>();
            services.AddScoped<IValidator<AddClassDTO>, ClassValidator>();
            services.AddScoped<IValidator<UpdateClassDTO>, UpdateClassValidator>();
            services.AddScoped<IValidator<CreateInvoiceDTO>, CreateInvoiceValidator>();
            services.AddScoped<IValidator<PaymentDTO>, CreatePaymentValidator>();
            services.AddScoped<IValidator<PupilScoreDTO>, PupilScoreValidator>();
            services.AddScoped<IValidator<CreateBusRouteDto>, BusRouteValidator>();
            services.AddScoped<IValidator<SubscriptionPlanDTO>, CreateSubscriptionPlanValidator>();
            services.AddScoped<IValidator<CreateAndUpdateSchoolYearDTO>, SchoolYearValidator>();
            services.AddScoped<IValidator<CreateAndUpdateSemesterDTO>, SemesterValidator>();
            services.AddScoped<IValidator<PupilFeedbackDTO>, PupilFeedbackValidator>();
            services.AddScoped<IValidator<RequestGetPupilFeedbackDTO>, RequestGetPupilFeedbackValidator>();
            services.AddScoped<IValidator<UpdateProfilePupilDTO>, PupilUpdateProfileValidator>();
            services.AddScoped<IValidator<UpdateProfileTeacherDTO>, TeacherUpdateProfileValidator>();
            services.AddScoped<IValidator<SubjectDTO>, SubjectValidator>();
            services.AddScoped<IValidator<CreateBusDto>, BusValidator>();
            services.AddScoped<IValidator<CreateBusStopDTO>, BusStopValidator>();
            services.AddScoped<IValidator<UpdateProfileBusSupervisorDTO>, BusSupervisorValidator>();
            services.AddScoped<IValidator<CreateTimeSlotDTO>, TimeSlotValidator>();
            services.AddScoped<IValidator<CreateClubDTO>, ClubValidator>();
            services.AddScoped<IValidator<VerifyFirstLoginRequest>, VerifyFirstLoginValidator>();
            services.AddScoped<IValidator<UploadExcelRequest>, UploadExcelValidator>();
            services.AddScoped<IValidator<CreateBusSupervisorDTO>, BusSupervisorCreateValidator>();
            services.AddScoped<IValidator<UpdateBusSupervisorDTO>, BusSupervisorUpdateValidator>();
            services.AddScoped<IValidator<ChangePasswordDTO>, ChangePasswordValidator>();           
        }

        public static void ConfigureFileUploadHelper(this IServiceCollection services)
        {
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IAutoMapper, CustomAutoMapper>();
            services.AddScoped(typeof(UserProfile));
            services.AddScoped(typeof(RegisterProfile));
            services.AddScoped(typeof(ClassProfile));
            services.AddScoped(typeof(TeacherProfile));
            services.AddScoped(typeof(PupilProfile));
            services.AddScoped(typeof(SubjectProfile));
            services.AddScoped(typeof(SemesterProfile));
            services.AddScoped(typeof(TimeSlotProfile));
            services.AddScoped(typeof(SchoolSubscriptionProfile));
            services.AddScoped(typeof(FeatureProfile));
            services.AddScoped(typeof(SchoolProfile));
            services.AddScoped(typeof(SubscriptionPlanProfile));
            services.AddScoped(typeof(SchoolSubscriptionPlanProfile));
            services.AddScoped(typeof(InvoiceProfile));
            services.AddScoped(typeof(PaymentProfile));
            services.AddScoped(typeof(NotificationProfile));
            services.AddScoped(typeof(ClubProfile));
            services.AddScoped(typeof(BusProfile));
            services.AddScoped(typeof(ClubEnrollmentProfile));
            services.AddScoped(typeof(BusRouteProfile));
            services.AddScoped(typeof(SchoolYearProfile));
            services.AddScoped(typeof(PupilScoreProfile));
            services.AddScoped(typeof(PupilFeedbackProfile));
            services.AddScoped(typeof(BusStopProfile));
            services.AddScoped(typeof(BusEnrollmentProfile));
            services.AddScoped(typeof(BusSupervisorProfile));
        }

        public static void AddDBSeeder(this IServiceCollection services)
        {
            services.AddTransient<DBSeeder>();
        }
    }
}