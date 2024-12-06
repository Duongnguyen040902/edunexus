﻿namespace sep.backend.v1.Data.Entities
{
    public class BaseUserEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string ShortRoleName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public int AccountStatus { get; set; }
    }
}
