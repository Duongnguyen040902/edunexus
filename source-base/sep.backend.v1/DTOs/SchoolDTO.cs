using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.DTOs;

public class SchoolDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WebsiteLink { get; set; }
    public string? SchoolName { get; set; }
    public string? Email { get; set; }
    public string? StandardCode { get; set; }
    public string AccountStatus { get; set; }
    public string? StatusName { get; set; }
    public string? Image { get; set; }
    public DateTime? DateOfEstablishment { get; set; }
    public ICollection<SchoolSubscriptionPlanDTO>? SchoolSubscriptionPlans { get; set; }
}

public class CreateSchoolDTO
{
    public CreateSchoolDTO()
    {
        Role = ShortRoleName.SCHOOL_ADMIN;
        AccountStatus = (int)Statuses.Active;
        Password = BCrypt.Net.BCrypt.HashPassword(Configs.DEFAULT_PASSWORD);
    }

    public string Username { get; set; }
    public string Password { get; set; }
    public string SchoolName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string Role { get; set; }
    public int AccountStatus { get; set; }
    public int? SubscriptionPlanId { get; set; }
    public string? PaymentMethod { get; set; }
}

public class UpdateSchoolDTO
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WebsiteLink { get; set; }
    public string? SchoolName { get; set; }
    public int AccountStatus { get; set; }
    public string? Email { get; set; }
    public string? StandardCode { get; set; }
    public DateTime? DateOfEstablishment { get; set; }
    // public List<int> SubscriptionPlanIds { get; set; } //TODO:QA - SubscriptionPlanIds
}
public class SchoolInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalPhone { get; set; }
        public string? WebsiteLink { get; set; }
        public string? Image { get; set; }
        public string? StandardCode { get; set; }
        public DateTime? DateOfEstablishment { get; set; }
        public string? FAX { get; set; }
    }
     public class UpdateInfoSchoolDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PrincipalName { get; set; }
        public string? PrincipalPhone { get; set; }
        public string? WebsiteLink { get; set; }
        public string? StandardCode { get; set; }
        public DateTime? DateOfEstablishment { get; set; }
        public string? FAX { get; set; }

        public IFormFile? ImageFile { get; set; } 
    }
