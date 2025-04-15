using System;
using System.Collections.Generic;

namespace OnlineLibrary.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Category { get; set; }

    public string? Racknumber { get; set; }

    public int Price { get; set; }

    public int? Stocknumber { get; set; }

}
