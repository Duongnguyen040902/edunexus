﻿namespace sep.backend.v1.Requests.Auth
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
