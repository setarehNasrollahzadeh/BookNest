using System;
using System.Collections.Generic;

namespace BookNest.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual User User { get; set; } = null!;
}
