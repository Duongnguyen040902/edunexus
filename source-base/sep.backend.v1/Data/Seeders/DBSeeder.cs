using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Common.Const;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace sep.backend.v1.Data.Seeders
{
    public class DBSeeder
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<DBSeeder> _logger;

        public DBSeeder(ApplicationContext context, ILogger<DBSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (!_context.Database.CanConnect())
            {
                _logger.LogError("Không thể kết nối đến cơ sở dữ liệu. Hủy bỏ việc tạo dữ liệu.");
                return;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Xóa dữ liệu hiện có
                await DeleteExistingDataAsync();

                // Tạo dữ liệu mới
                await SeedSubscriptionPlansAsync();
                await SeedSchoolsAsync();
                await SeedSchoolSubscriptionPlansAsync();
                await SeedTeachersAsync();
                await SeedClassesAsync();
                await SeedSchoolYearsAsync();
                await SeedSemestersAsync();
                await SeedSubjectsAsync();
                await SeedTimeSlotsAsync();
                await SeedNotificationCategoriesAsync();
                await SeedClassApplicationCategoriesAsync();
                await SeedPupilsAsync();
                await SeedBusSupervisorsAsync();
                await SeedBusRoutesAsync();
                await SeedBusesAsync();
                await SeedBusRoutesAsync();
                await SeedBusStopsAsync();
                await SeedClubsAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Tạo dữ liệu cơ sở dữ liệu thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo dữ liệu cơ sở dữ liệu. Giao dịch đã được hoàn tác.");
            }
        }

        private async Task DeleteExistingDataAsync()
        {
            _logger.LogInformation("Đang xóa dữ liệu hiện có...");

            _context.Teachers.RemoveRange(_context.Teachers);
            _context.Classes.RemoveRange(_context.Classes);
            _context.SchoolYears.RemoveRange(_context.SchoolYears);
            _context.SchoolSubscriptionPlans.RemoveRange(_context.SchoolSubscriptionPlans);
            _context.Schools.RemoveRange(_context.Schools);
            _context.SubscriptionPlans.RemoveRange(_context.SubscriptionPlans);
            _context.Subjects.RemoveRange(_context.Subjects);
            _context.TimeSlots.RemoveRange(_context.TimeSlots);
            _context.NotificationCategories.RemoveRange(_context.NotificationCategories);
            _context.ClassApplicationCategory.RemoveRange(_context.ClassApplicationCategory);
            _context.Pupils.RemoveRange(_context.Pupils);
            _context.BusSupervisors.RemoveRange(_context.BusSupervisors);
            _context.BusRoutes.RemoveRange(_context.BusRoutes);
            _context.Buses.RemoveRange(_context.Buses);
            _context.BusStops.RemoveRange(_context.BusStops);
            _context.Clubs.RemoveRange(_context.Clubs);


            await _context.SaveChangesAsync();

            _logger.LogInformation("Xóa dữ liệu hiện có thành công.");
        }

        private async Task SeedSubscriptionPlansAsync()
        {
            if (_context.SubscriptionPlans.Any())
            {
                _logger.LogInformation("Các gói đăng ký đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các gói đăng ký...");
            var subscriptionPlans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan { Name = "Gói Cơ Bản", Description = "Gói đăng ký cơ bản", Price = 100, DurationDays = 30, MaxActiveAccounts = 100  },
                new SubscriptionPlan { Name = "Gói Tiêu Chuẩn", Description = "Gói đăng ký tiêu chuẩn", Price = 200, DurationDays = 60,MaxActiveAccounts = 100 },
                new SubscriptionPlan { Name = "Gói Cao Cấp", Description = "Gói đăng ký cao cấp", Price = 300, DurationDays = 90, MaxActiveAccounts = 100 }
            };

            await _context.SubscriptionPlans.AddRangeAsync(subscriptionPlans);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các gói đăng ký thành công.");
        }

        private async Task SeedSchoolsAsync()
        {
            if (_context.Schools.Any())
            {
                _logger.LogInformation("Các trường học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các trường học...");
            var school = new School
            {
                Name = "Trường Trung Học Greenwood",
                Address = "123 Đường Chính",
                PhoneNumber = "123-456-7890",
                PrincipalName = "John Doe",
                PrincipalPhone = "123-456-7891",
                WebsiteLink = "http://greenwoodhigh.edu",
                StandardCode = "GH123",
                DateOfEstablishment = DateTime.SpecifyKind(new DateTime(1990, 1, 1), DateTimeKind.Utc),
                FAX = "123-456-7892",
                Username = "greenwood.admin",
                Password = BCrypt.Net.BCrypt.HashPassword("string"),
                ShortRoleName = ShortRoleName.SCHOOL_ADMIN,
                AccountStatus = 1,
                RefreshToken = Guid.NewGuid().ToString(), // Ensure RefreshToken is not null
                RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc), // Example expiry date
                Email = "admin@greenwoodhigh.edu" // Set Email property
            };

            try
            {
                await _context.Schools.AddAsync(school);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Tạo các trường học và gói đăng ký thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo các trường học.");
                throw;
            }
        }

        private async Task SeedSchoolSubscriptionPlansAsync()
        {
            if (_context.SchoolSubscriptionPlans.Any())
            {
                _logger.LogInformation("Các gói đăng ký trường học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các gói đăng ký trường học...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");
            var subscriptionPlans = _context.SubscriptionPlans.ToList();

            if (school == null || !subscriptionPlans.Any())
            {
                _logger.LogError("Không tìm thấy trường học hoặc gói đăng ký. Hủy bỏ việc tạo các gói đăng ký trường học.");
                return;
            }

            var schoolSubscriptionPlans = new List<SchoolSubscriptionPlan>
            {
                new SchoolSubscriptionPlan
                {
                    SchoolId = school.Id,
                    SubscriptionPlanId = subscriptionPlans[0].Id,
                    Status = 1,
                    StartDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(1), DateTimeKind.Utc)
                },
                new SchoolSubscriptionPlan
                {
                    SchoolId = school.Id,
                    SubscriptionPlanId = subscriptionPlans[1].Id,
                    Status = 1,
                    StartDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(2), DateTimeKind.Utc)
                },
                new SchoolSubscriptionPlan
                {
                    SchoolId = school.Id,
                    SubscriptionPlanId = subscriptionPlans[2].Id,
                    Status = 1,
                    StartDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(DateTime.UtcNow.AddMonths(3), DateTimeKind.Utc)
                }
            };

            await _context.SchoolSubscriptionPlans.AddRangeAsync(schoolSubscriptionPlans);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các gói đăng ký trường học thành công.");
        }

        private async Task SeedTeachersAsync()
        {
            if (_context.Teachers.Any())
            {
                _logger.LogInformation("Các giáo viên đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các giáo viên...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các giáo viên.");
                return;
            }

            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Gender = true,
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1980, 5, 15), DateTimeKind.Utc),
                    PhoneNumber = "123-456-7893",
                    Address = "456 Đường Oak",
                    SchoolId = school.Id,
                    Username = "alice.johnson",
                    Password = BCrypt.Net.BCrypt.HashPassword("string"),
                    ShortRoleName = ShortRoleName.TEACHER,
                    AccountStatus = 1,
                    RefreshToken = Guid.NewGuid().ToString(), // Ensure RefreshToken is not null
                    RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc), // Example expiry date
                    Email = "alice.johnson@greenwoodhigh.edu" // Set Email property
                },
                new Teacher
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    Gender = true,
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1975, 8, 20), DateTimeKind.Utc),
                    PhoneNumber = "123-456-7894",
                    Address = "789 Đường Pine",
                    SchoolId = school.Id,
                    Username = "bob.smith",
                    Password = BCrypt.Net.BCrypt.HashPassword("string"),
                    ShortRoleName = ShortRoleName.TEACHER,
                    AccountStatus = 1,
                    RefreshToken = Guid.NewGuid().ToString(), // Ensure RefreshToken is not null
                    RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc), // Example expiry date
                    Email = "bob.smith@greenwoodhigh.edu" // Set Email property
                },
                new Teacher
                {
                    FirstName = "Carol",
                    LastName = "Williams",
                    Gender = false,
                    DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 3, 10), DateTimeKind.Utc),
                    PhoneNumber = "123-456-7895",
                    Address = "101 Đường Maple",
                    SchoolId = school.Id,
                    Username = "carol.williams",
                    Password = BCrypt.Net.BCrypt.HashPassword("string"),
                    ShortRoleName = ShortRoleName.TEACHER,
                    AccountStatus = 1,
                    RefreshToken = Guid.NewGuid().ToString(), // Ensure RefreshToken is not null
                    RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc), // Example expiry date
                    Email = "carol.williams@greenwoodhigh.edu" // Set Email property
                }
            };

            await _context.Teachers.AddRangeAsync(teachers);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các giáo viên thành công.");
        }

        private async Task SeedClassesAsync()
        {
            if (_context.Classes.Any())
            {
                _logger.LogInformation("Các lớp học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các lớp học...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các lớp học.");
                return;
            }

            var classes = new List<Class>
{
    new Class { Name = "Lớp 1A", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 1B", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 1C", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 2A", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 2B", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 2C", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 3A", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 3B", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 3C", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 4A", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 4B", SchoolId = school.Id, Status = 1 },
    new Class { Name = "Lớp 4C", SchoolId = school.Id, Status = 1 }
};


            await _context.Classes.AddRangeAsync(classes);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các lớp học thành công.");
        }

        private async Task SeedSchoolYearsAsync()
        {
            if (_context.SchoolYears.Any())
            {
                _logger.LogInformation("Các năm học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các năm học...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các năm học.");
                return;
            }

            var schoolYears = new List<SchoolYear>
{
    new SchoolYear { Name = "2022-2023", StartDate = DateTime.SpecifyKind(new DateTime(2022, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2023, 6, 30), DateTimeKind.Utc), IsActive = true, SchoolId = school.Id },
    new SchoolYear { Name = "2023-2024", StartDate = DateTime.SpecifyKind(new DateTime(2023, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2024, 6, 30), DateTimeKind.Utc), IsActive = false, SchoolId = school.Id },
    new SchoolYear { Name = "2024-2025", StartDate = DateTime.SpecifyKind(new DateTime(2024, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2025, 6, 30), DateTimeKind.Utc), IsActive = true, SchoolId = school.Id }
};

            await _context.SchoolYears.AddRangeAsync(schoolYears);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các năm học thành công.");
        }
        private async Task SeedSemestersAsync()
        {
            if (_context.Semesters.Any())
            {
                _logger.LogInformation("Các học kỳ đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các học kỳ...");
            var schoolYear = _context.SchoolYears.FirstOrDefault(sy => sy.Name == "2022-2023");
            var schoolYear2 = _context.SchoolYears.FirstOrDefault(sy => sy.Name == "2023-2024");
            var schoolYear3 = _context.SchoolYears.FirstOrDefault(sy => sy.Name == "2024-2025");

            if (schoolYear == null)
            {
                _logger.LogError("Không tìm thấy năm học. Hủy bỏ việc tạo các học kỳ.");
                return;
            }

            var semesters = new List<Semester>
{
    new Semester { SemesterName = "Mùa Thu 2022", SemesterCode = "F22", StartDate = DateTime.SpecifyKind(new DateTime(2022, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2022, 12, 31), DateTimeKind.Utc), IsActive = false, SchoolYearId = schoolYear.Id },
    new Semester { SemesterName = "Mùa Xuân 2023", SemesterCode = "S23", StartDate = DateTime.SpecifyKind(new DateTime(2023, 1, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2023, 6, 30), DateTimeKind.Utc), IsActive = false, SchoolYearId = schoolYear.Id },
    new Semester { SemesterName = "Mùa Thu 2023", SemesterCode = "F23", StartDate = DateTime.SpecifyKind(new DateTime(2023, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2023, 12, 31), DateTimeKind.Utc), IsActive = false, SchoolYearId = schoolYear2.Id },
    new Semester { SemesterName = "Mùa Xuân 2024", SemesterCode = "S24", StartDate = DateTime.SpecifyKind(new DateTime(2024, 1, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2024, 6, 30), DateTimeKind.Utc), IsActive = false, SchoolYearId = schoolYear2.Id },
    new Semester { SemesterName = "Mùa Thu 2024", SemesterCode = "F24", StartDate = DateTime.SpecifyKind(new DateTime(2024, 9, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2024, 12, 31), DateTimeKind.Utc), IsActive = true, SchoolYearId = schoolYear3.Id },
    new Semester { SemesterName = "Mùa Xuân 2025", SemesterCode = "S25", StartDate = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc), EndDate = DateTime.SpecifyKind(new DateTime(2025, 6, 30), DateTimeKind.Utc), IsActive = false, SchoolYearId = schoolYear3.Id }
};

            await _context.Semesters.AddRangeAsync(semesters);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các học kỳ thành công.");
        }



        private async Task SeedSubjectsAsync()
        {
            if (_context.Subjects.Any())
            {
                _logger.LogInformation("Các môn học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các môn học...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các môn học.");
                return;
            }

            var subjects = new List<Subject>
            {
                new Subject { Name = "Toán học", Code = "MATH101", SchoolId = school.Id },
                new Subject { Name = "Khoa học", Code = "SCI101", SchoolId = school.Id },
                new Subject { Name = "Lịch sử", Code = "HIST101", SchoolId = school.Id },
                new Subject { Name = "Văn học", Code = "LIT101", SchoolId = school.Id } // Môn học mới
            };


            await _context.Subjects.AddRangeAsync(subjects);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các môn học thành công.");
        }

        private async Task SeedTimeSlotsAsync()
        {
            if (_context.TimeSlots.Any())
            {
                _logger.LogInformation("Các khung giờ đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các khung giờ...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các khung giờ.");
                return;
            }

            var timeSlots = new List<TimeSlot>
            {
                new TimeSlot { Name = "Tiết 1", StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(8, 45, 0), IsActive = true, SchoolId = school.Id },
                new TimeSlot { Name = "Tiết 2", StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(9, 45, 0), IsActive = true, SchoolId = school.Id },
                new TimeSlot { Name = "Tiết 3", StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(10, 45, 0), IsActive = true, SchoolId = school.Id },
                new TimeSlot { Name = "Tiết 4", StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(11, 45, 0), IsActive = true, SchoolId = school.Id },
                new TimeSlot { Name = "Tiết 5", StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(13, 45, 0), IsActive = true, SchoolId = school.Id }
            };


            await _context.TimeSlots.AddRangeAsync(timeSlots);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các khung giờ thành công.");
        }


        private async Task SeedNotificationCategoriesAsync()
        {
            if (_context.NotificationCategories.Any())
            {
                _logger.LogInformation("Các loại thông báo đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các loại thông báo...");
            var notificationCategories = new List<NotificationCategory>
            {
                new NotificationCategory { Name = "Khẩn cấp" },
                new NotificationCategory { Name = "Chung" },
            };

            await _context.NotificationCategories.AddRangeAsync(notificationCategories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các loại thông báo thành công.");
        }

        private async Task SeedClassApplicationCategoriesAsync()
        {
            if (_context.ClassApplication.Any())
            {
                _logger.LogInformation("Các loại đơn xin học đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các loại đơn xin học...");
            var classApplicationCategories = new List<ClassApplicationCategory>
            {
                new ClassApplicationCategory { Name = "Đơn xin nghỉ học" },
                new ClassApplicationCategory { Name = "Đơn xin chuyển lớp" },
                new ClassApplicationCategory { Name = "Đơn Khác" }
            };

            await _context.ClassApplicationCategory.AddRangeAsync(classApplicationCategories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các loại đơn xin học thành công.");
        }

        private async Task SeedPupilsAsync()
        {
            if (_context.Pupils.Any())
            {
                _logger.LogInformation("Các học sinh đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các học sinh...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các học sinh.");
                return;
            }

            var pupils = new List<Pupil>
    {
        new Pupil
        {
            FirstName = "Nguyễn",
            LastName = "Văn A",
            Gender = true,
            Username = "nguyenvana",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "nguyenvana@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Trần",
            LastName = "Thị B",
            Gender = false,
            Username = "tranthib",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "tranthib@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Lê",
            LastName = "Văn C",
            Gender = true,
            Username = "levanc",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "levanc@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Phạm",
            LastName = "Thị D",
            Gender = false,
            Username = "phamthid",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "phamthid@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Hoàng",
            LastName = "Văn E",
            Gender = true,
            Username = "hoangvane",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "hoangvane@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Đỗ",
            LastName = "Thị F",
            Gender = false,
            Username = "dothif",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "dothif@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Vũ",
            LastName = "Văn G",
            Gender = true,
            Username = "vuvang",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "vuvang@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Ngô",
            LastName = "Thị H",
            Gender = false,
            Username = "ngothih",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "ngothih@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Bùi",
            LastName = "Văn I",
            Gender = true,
            Username = "buivani",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "buivani@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        },
        new Pupil
        {
            FirstName = "Nguyễn",
            LastName = "Thị J",
            Gender = false,
            Username = "nguyenthij",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "nguyenthij@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.DONNOR,
        }
    };

            await _context.Pupils.AddRangeAsync(pupils);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các học sinh thành công.");
        }

        private async Task SeedBusSupervisorsAsync()
        {
            if (_context.BusSupervisors.Any())
            {
                _logger.LogInformation("Các giám sát xe buýt đã được tạo.");
                return;
            }

            _logger.LogInformation("Đang tạo các giám sát xe buýt...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Hủy bỏ việc tạo các giám sát xe buýt.");
                return;
            }

            var busSupervisors = new List<BusSupervisor>
    {
        new BusSupervisor
        {
            FirstName = "Lê",
            LastName = "Văn C",
            Gender = true,
            Username = "levanc",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "levanc@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.BUS_SUPER_VISOR,
        },
        new BusSupervisor
        {
            FirstName = "Phạm",
            LastName = "Thị D",
            Gender = false,
            Username = "phamthid",
            Password = BCrypt.Net.BCrypt.HashPassword("string"),
            SchoolId = school.Id,
            AccountStatus = 1,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpiryDate = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc),
            Email = "phamthid@greenwoodhigh.edu",
            ShortRoleName = ShortRoleName.BUS_SUPER_VISOR,
        }
    };

            await _context.BusSupervisors.AddRangeAsync(busSupervisors);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Tạo các giám sát xe buýt thành công.");
        }

        private async Task SeedBusRoutesAsync()
        {
            if (_context.BusRoutes.Any())
            {
                _logger.LogInformation("Các tuyến xe buýt đã tồn tại.");
                return;
            }

            _logger.LogInformation("Đang thêm dữ liệu các tuyến xe buýt...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Dừng thêm dữ liệu tuyến xe buýt.");
                return;
            }

            var busRoutes = new List<BusRoute>
            {
                new BusRoute { Name = "Tuyến 1", Description = "Tuyến chính", SchoolId = school.Id, Status = 1 },
                new BusRoute { Name = "Tuyến 2", Description = "Tuyến phụ", SchoolId = school.Id, Status = 1 }
            };

            await _context.BusRoutes.AddRangeAsync(busRoutes);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Thêm dữ liệu các tuyến xe buýt thành công.");
        }

        private async Task SeedBusesAsync()
        {
            if (_context.Buses.Any())
            {
                _logger.LogInformation("Các xe buýt đã tồn tại.");
                return;
            }

            _logger.LogInformation("Đang thêm dữ liệu các xe buýt...");
            var busRoute1 = _context.BusRoutes.FirstOrDefault(br => br.Name == "Tuyến 1");
            var busRoute2 = _context.BusRoutes.FirstOrDefault(br => br.Name == "Tuyến 2");

            if (busRoute1 == null || busRoute2 == null)
            {
                _logger.LogError("Không tìm thấy tuyến xe buýt. Dừng thêm dữ liệu xe buýt.");
                return;
            }

            var buses = new List<Bus>
            {
                new Bus { Name = "Xe buýt 1", DriverName = "Tài xế A", DriverPhone = "123456789", LicensePlate = "ABC-123", SeatNumber = 40, BusRouteId = busRoute1.Id, Status = 1 },
                new Bus { Name = "Xe buýt 2", DriverName = "Tài xế B", DriverPhone = "987654321", LicensePlate = "XYZ-789", SeatNumber = 40, BusRouteId = busRoute1.Id, Status = 1 },
                new Bus { Name = "Xe buýt 3", DriverName = "Tài xế C", DriverPhone = "123123123", LicensePlate = "DEF-456", SeatNumber = 40, BusRouteId = busRoute2.Id, Status = 1 },
                new Bus { Name = "Xe buýt 4", DriverName = "Tài xế D", DriverPhone = "321321321", LicensePlate = "GHI-789", SeatNumber = 40, BusRouteId = busRoute2.Id, Status = 1 }
            };

            await _context.Buses.AddRangeAsync(buses);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Thêm dữ liệu các xe buýt thành công.");
        }

        private async Task SeedBusStopsAsync()
        {
            if (_context.BusStops.Any())
            {
                _logger.LogInformation("Các điểm dừng xe buýt đã tồn tại.");
                return;
            }

            _logger.LogInformation("Đang thêm dữ liệu các điểm dừng xe buýt...");
            var busRoute1 = _context.BusRoutes.FirstOrDefault(br => br.Name == "Tuyến 1");
            var busRoute2 = _context.BusRoutes.FirstOrDefault(br => br.Name == "Tuyến 2");

            if (busRoute1 == null || busRoute2 == null)
            {
                _logger.LogError("Không tìm thấy tuyến xe buýt. Dừng thêm dữ liệu điểm dừng xe buýt.");
                return;
            }

            var busStops = new List<BusStop>
            {
                new BusStop { Name = "Điểm dừng 1", PickUpTime = new TimeSpan(7, 0, 0), ReturnTime = new TimeSpan(16, 0, 0), Address = "123 Đường Chính", BusRouteId = busRoute1.Id, Status = 1, Index = 1 },
                new BusStop { Name = "Điểm dừng 2", PickUpTime = new TimeSpan(7, 15, 0), ReturnTime = new TimeSpan(16, 15, 0), Address = "456 Đường Phụ", BusRouteId = busRoute1.Id, Status = 1, Index = 2 },
                new BusStop { Name = "Điểm dừng 3", PickUpTime = new TimeSpan(7, 30, 0), ReturnTime = new TimeSpan(16, 30, 0), Address = "789 Đường Chính", BusRouteId = busRoute1.Id, Status = 1, Index = 3 },
                new BusStop { Name = "Điểm dừng 4", PickUpTime = new TimeSpan(7, 45, 0), ReturnTime = new TimeSpan(16, 45, 0), Address = "101 Đường Phụ", BusRouteId = busRoute2.Id, Status = 1, Index = 1 },
                new BusStop { Name = "Điểm dừng 5", PickUpTime = new TimeSpan(8, 0, 0), ReturnTime = new TimeSpan(17, 0, 0), Address = "202 Đường Chính", BusRouteId = busRoute2.Id, Status = 1, Index = 2 },
                new BusStop { Name = "Điểm dừng 6", PickUpTime = new TimeSpan(8, 15, 0), ReturnTime = new TimeSpan(17, 15, 0), Address = "303 Đường Phụ", BusRouteId = busRoute2.Id, Status = 1, Index = 3 }
            };

            await _context.BusStops.AddRangeAsync(busStops);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Thêm dữ liệu các điểm dừng xe buýt thành công.");
        }

        private async Task SeedClubsAsync()
        {
            if (_context.Clubs.Any())
            {
                _logger.LogInformation("Các câu lạc bộ đã tồn tại.");
                return;
            }

            _logger.LogInformation("Đang thêm dữ liệu các câu lạc bộ...");
            var school = _context.Schools.FirstOrDefault(s => s.Name == "Trường Trung Học Greenwood");

            if (school == null)
            {
                _logger.LogError("Không tìm thấy trường học. Dừng thêm dữ liệu câu lạc bộ.");
                return;
            }

            var clubs = new List<Club>
    {
        new Club { Name = "Câu lạc bộ Bóng đá", Description = "Câu lạc bộ dành cho những người yêu thích bóng đá", Status = 1, SchoolId = school.Id },
        new Club { Name = "Câu lạc bộ Âm nhạc", Description = "Câu lạc bộ dành cho những người yêu thích âm nhạc", Status = 1, SchoolId = school.Id },
        new Club { Name = "Câu lạc bộ Khoa học", Description = "Câu lạc bộ dành cho những người yêu thích khoa học", Status = 1, SchoolId = school.Id },
        new Club { Name = "Câu lạc bộ Văn học", Description = "Câu lạc bộ dành cho những người yêu thích văn học", Status = 1, SchoolId = school.Id }
    };

            await _context.Clubs.AddRangeAsync(clubs);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Thêm dữ liệu các câu lạc bộ thành công.");
        }



    }
}