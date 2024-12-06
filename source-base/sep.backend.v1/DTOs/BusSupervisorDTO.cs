namespace sep.backend.v1.DTOs
{
    public class BusSupervisorDetailDTO
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int SchoolId { get; set; }
    }
    public class BusSupervisorDTO 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int SchoolId { get; set; }
        public string Image { get; set; }
    }

    public class ProfileBusSupervisorDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int? SchoolId { get; set; }
    }

    public class UpdateProfileBusSupervisorDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
    }

    public class CreateBusSupervisorDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
    public class BusSupervisorAccountDetailDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public string GenderName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int SchoolId { get; set; }
        public int AccountStatus { get; set; }
        public string AccountStatusName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
    public class UpdateBusSupervisorDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int AccountStatus { get; set; }
    }
}
