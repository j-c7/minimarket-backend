using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.User;

public class LoginDTO
{
    private string? _email;

    [Required(ErrorMessage = "El email es requerido")]
    public string? Email
    {
        get => _email;
        set => _email = value!.Trim();
    }

    private string? _password;

    [Required(ErrorMessage = "La contraseña es requerida")]
    public string? Password
    {
        get => _password;
        set => _password = value!.Trim();
    }

}