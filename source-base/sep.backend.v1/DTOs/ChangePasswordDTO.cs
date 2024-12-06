namespace sep.backend.v1.DTOs
{
    public class ChangePasswordDTO
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
