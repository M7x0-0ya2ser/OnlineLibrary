using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlineLibrary.Models;

public partial class BorrowedBook
{
    public DateOnly? Dateofreturn { get; set; }

    public int Ordernumber { get; set; }

    public bool? Isaccepted { get; set; }

    public string? Bookisbn { get; set; }

    public int? Userid { get; set; }

    [JsonIgnore]
    public virtual Book? BookisbnNavigation { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
