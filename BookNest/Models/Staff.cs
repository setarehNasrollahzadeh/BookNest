using System;
using System.Collections.Generic;

namespace BookNest.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
