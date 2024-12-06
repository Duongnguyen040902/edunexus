namespace sep.backend.v1.Requests.Auth
{
    public class ForgotPasswordRequest
    {
        public int Mode { get; set; }
        public string Email { get; set; }
    }
}
