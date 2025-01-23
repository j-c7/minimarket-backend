namespace Minimarket.DTO.User;

public class SessionDTO
{
    public object? Token { get; set; }

    public ResponseUserDTO? Profile { get; set; }
}