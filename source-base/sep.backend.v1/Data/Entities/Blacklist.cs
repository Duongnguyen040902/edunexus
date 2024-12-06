namespace sep.backend.v1.Data.Entities;

public class Blacklist
{
    public int Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
}