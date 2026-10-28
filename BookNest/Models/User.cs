using System;
using System.Collections.Generic;

namespace BookNest.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
