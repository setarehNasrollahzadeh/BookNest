using System;
using System.Collections.Generic;

namespace BookNest.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
