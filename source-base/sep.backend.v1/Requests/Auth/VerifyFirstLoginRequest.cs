namespace sep.backend.v1.Requests.Auth;

public class VerifyFirstLoginRequest
{
    public int id { get; set; }
    public int Mode { get; set; }
    public string Email { get; set; }
}