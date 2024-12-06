namespace sep.backend.v1.Requests.Auth
{
    public class LoginRequest
    {
        public int Mode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
