using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class PupilDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string? Image { get; set; }   
    }
    
    public class CreatePupilDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
    }

    public class PupilDetailDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public string GenderName { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
        public int AccountStatus { get; set; }
        public string AccountStatusName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }

    public class UpdatePupilDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int AccountStatus { get; set; }
    }
       public class ViewAdminPupilDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AddPupilToClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    
    public class ProfilePupilDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? ClassName { get; set; }
    }

    public class UpdateProfilePupilDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public IFormFile? Image { get; set; }
    }
    public class PupilAssignToClubDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string? Image { get; set; }
    }
}
