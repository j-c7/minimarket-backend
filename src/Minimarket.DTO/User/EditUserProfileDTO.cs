using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.User;

public class EditUserProfileDTO
{
    [Required(ErrorMessage = "El Id es obligatorio")]
    public int? Id { get; set; }

    private string? _name;

    public string? Name
    {
        get => _name;
        set => _name = value!.Trim();
    }

    private string? _email;

    public string? Email
    {
        get => _email;
        set => _email = value!.Trim().ToLower();
    }

    private string? _password;

    public string? Password
    {
        get => _password;
        set => _password = value!.Trim();
    }

    private string? _role = "admin";

    public string? Role
    {
        get => _role;
        set => _role = value!.Trim().ToLower();
    }
}