using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.User;

public class UserProfileDTO
{
    private string? _name;

    [Required(ErrorMessage = "El nombre es requerido")]
    public string? Name
    {
        get => _name;
        set => _name = value!.Trim();
    }

    private string? _email;

    [Required(ErrorMessage = "El email es requerido")]
    public string? Email
    {
        get => _email;
        set => _email = value!.Trim().ToLower();
    }

    private string? _password;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string? Password
    {
        get => _password;
        set => _password = value!.Trim();
    }

    private string? _role = "admin";

    [Required(ErrorMessage = "El rol es requerido")]
    public string? Role
    {
        get => _role;
        set => _role = value!.Trim().ToLower();
    }
}