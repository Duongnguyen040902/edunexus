namespace sep.backend.v1.Requests.Auth
{
    public class VerifyEmailRequest
    {
        public string Token { get; set; }
        public string VerificationCode { get; set; }
    }
}
