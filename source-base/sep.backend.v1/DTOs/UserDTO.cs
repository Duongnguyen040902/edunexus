using System.Text.Json.Serialization;

public class UserDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string RefreshToken { get; set; }

    public List<UserRoleDTO> UserRoles { get; set; }
}

public class UserRoleDTO
{
    public int Id { get; set; }
    public RoleDTO Role { get; set; }
}

public class RoleDTO
{
    public int Id { get; set; }
    public string RoleName { get; set; }
}

public class RegisterDTO
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string ShortRoleName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}

public class RegisterDTORemake
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
}

public class RegisterResponse
{
    public string Message { get; set; }
    public string Token { get; set; }
}