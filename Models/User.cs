using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineLibrary.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateOnly? Dateofbirth { get; set; }

    public string Email { get; set; } = null!;

    public bool? Isadmin { get; set; }

    public bool? Isaccepted { get; set; }

    [JsonIgnore]
    public byte[]? Passwordhash { get; set; }

    [JsonIgnore]
    public byte[]? Passwordsalt { get; set; }

    
}
