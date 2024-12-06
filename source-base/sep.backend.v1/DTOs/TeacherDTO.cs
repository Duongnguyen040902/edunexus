namespace sep.backend.v1.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int SchoolId { get; set; }
    }

    public class ProfileTeacherDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public int SchoolId { get; set; }
        public List<SubjectDTO>? ListSubject { get; set; }

    }
    public class TeacherDetailDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool Gender { get; set; }
        public string GenderName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AccountStatusName { get; set;}
        public int AccountStatus { get; set;}
        public List<int> SubjectIds { get; set; }
        public List<string> Subjects { get; set; }
        public string Image { get; set; }
    }


    public class CreateTeacherDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<int>? SubjectIds { get; set; }

    }

    public class UpdateTeacherDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int AccountStatus { get; set; }
        public List<int>? SubjectIds { get; set; }
    }

    public class TeacherAccountDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

    }
     public class ViewTeacherAssignDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AssignTeacherToClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class UpdateProfileTeacherDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public IFormFile? Image { get; set; }
    }
    
    public class TeacherAssignClubDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string Image { get; set; }
    }   
}
