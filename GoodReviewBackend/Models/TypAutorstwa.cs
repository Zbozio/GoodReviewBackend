using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoodReviewBackend.Models;

public partial class TypAutorstwa
{
    public int IdTypu { get; set; }

    public string? NazwaTypu { get; set; }
    [JsonIgnore]

    public virtual ICollection<Udzial> Udzials { get; set; } = new List<Udzial>();
}
