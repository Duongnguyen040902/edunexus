using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sep.backend.v1.Data.Seeders;
using sep.backend.v1.Extensions;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Middlewares;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.PostgreSql;
using sep.backend.v1.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using sep.backend.v1.Requests.Policy;

namespace sep.backend.v1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Set Npgsql switches
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Handle database seeding if 'seed' argument is present
            if (args.Contains("seed"))
            {
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<DBSeeder>();
                    await seeder.SeedAsync();
                }

                return;
            }

            // Configure middleware
            ConfigureMiddleware(app);

            // Run the application
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            #region Hangfire&&AppContext

            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                        LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());


            services.AddHangfire(cfg =>
            {
                cfg.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection"));
                cfg.UseSimpleAssemblyNameTypeSerializer();
                cfg.UseRecommendedSerializerSettings();
            });

            services.AddHangfireServer(options => { options.ServerName = configuration["Hangfire:ServerName"]; });

            #endregion

            // Configure controllers
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 257;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            // Register services and helpers
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.ConfigureRepositoryManager();
            services.ConfigureUnitOfWork();
            services.ConfigureFileUploadHelper();
            services.ConfigureCors();
            services.ConfigureServices();
            services.ConfigureValidador();
            services.AddAutoMapper();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDBSeeder();
            services.ConfigureValidador();
            // Configure authentication
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:ValidIssuer"],
                        ValidAudience = configuration["Jwt:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                    };
                });

            // Configure policy
            services.AddScoped<IAuthorizationHandler, SchoolRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ClassRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ClubRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, BusRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, MultiRoleRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ClassOfTeacherAndPupilRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, SchoolYearRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SchoolPolicy", policy =>
                    policy.Requirements.Add(new SchoolRequirement()));
                options.AddPolicy("ClassPolicy", policy =>
                    policy.Requirements.Add(new ClassRequirement()));
                options.AddPolicy("ClubPolicy", policy =>
                    policy.Requirements.Add(new ClubRequirement()));
                options.AddPolicy("BusPolicy", policy =>
                    policy.Requirements.Add(new BusRequirement()));
                options.AddPolicy("MultiRolePolicy", policy =>
                    policy.Requirements.Add(new MultiRoleRequirement()));
                options.AddPolicy("ClassOfTeacherAndPupilPolicy", policy =>
                    policy.Requirements.Add(new ClassOfTeacherAndPupilRequirement()));
                options.AddPolicy("SchoolYearPolicy", policy =>
                    policy.Requirements.Add(new SchoolYearRequirement()));
            });


            // Configure Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wedding Planner API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            // Serve static files
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new CompositeFileProvider(
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Templates"))
                ),
                RequestPath = new PathString("/Resources")
            });

            app.UseHttpsRedirection();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions()
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser()
                            {
                                Login = app.Configuration["Hangfire:DashboardUsername"],
                                PasswordClear = app.Configuration["Hangfire:DashboardPassword"]
                            }
                        }
                    })
                }
            });
            RecurringJob.AddOrUpdate<IJobSendInvoice>("send-invoice-job", job => job.ExecuteAsync(), Cron.Daily(6, 0));
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<TokenLoginMiddleware>();
            app.UseMiddleware<TokenBlacklistMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePathBase("/api");
            app.MapControllers();
        }
    }
}