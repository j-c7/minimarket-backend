using System;
using System.Collections.Generic;

namespace Minimarket.Entity;

public partial class UserProfile
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? RegisterDate { get; set; }
}
